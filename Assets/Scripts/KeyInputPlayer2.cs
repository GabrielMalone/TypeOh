using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.Cinemachine;
using System.Collections.Generic;

// -----------------------------------------------------------------------
public class KeyInputPlayer2 : MonoBehaviour
{

    [Header("Player Selection")]
    [SerializeField] public static bool computer = false;

    [Header("Computer Player Settings")]
    public float speed = 1f;
    public float accuracy = 0.5f;
    public bool computerPaused = false;
    public float computerPauseTime = 2f;
    private float timeLastTyped = 0f;

    [Header("Letter Spawn Position")]
    public Transform correctSpawnPosition;
    public Transform incorrectSpawnPosition;

    [Header("Letter Material Settings")]
    public Material correct_material;


    [Header("platform letter scale settings")]
    public float letterScale = 1f;

    [Header("Mistake Settings")]
    public float mistakeSpawnInterval = 0.2f;
    public CinemachineImpulseSource impulseSource;

    [Header("Correct Settings")]
    public float comboTracker;
    public float comboNeeded = 20f;
    public Light combo_shaft;
    public Light combo_shaft2;
    private bool animatingCombo = false;
    public Light shaft_light;
    private bool flickering = false;
    private float ogIntensity;
    private Color ogColor;

    [Header("Sound FX Settings")]
    public AudioSource comboAudioSource;

    public AudioSource goldenLetterAudioSource;
    public AudioSource audio_source;
    public AudioClip correct_sound;
    public AudioClip incorrect_sound;

    [Header("Game State Manager")]
    public GameObject stateManager;
    public KeyInputPlayer1 keyInputPlayer1;
    private GameStateManager GSM;
    private char lastIncorrectLetter;
    private bool gameOver = false;
    private bool wrong_flicker = false;
    private float comboDecayMultiplier;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GSM = stateManager.GetComponent<GameStateManager>();
        ogIntensity = shaft_light.intensity;
        ogColor = shaft_light.color;
       
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (!gameOver && GSM.gameStarted)
        {
            CheckForKeyInput();
            if (computer && !computerPaused)
            {
                StartCoroutine(ComptuerPlayer());
            } 
        }
        if (GSM.player_2_winner)
        {
            gameOver = true;
            shaft_light.color = Color.lawnGreen;
            shaft_light.intensity += 600;
        }
        if (!animatingCombo)
            StartCoroutine(ComboShaftFX());

        // decay cmmbo over time to punish intentionally slow and precise typing
        if (GSM.P2InTheLead()) 
            comboDecayMultiplier = 2;
        else 
            comboDecayMultiplier = 1;
        comboTracker = Mathf.Max(0, comboTracker -= Time.deltaTime * comboDecayMultiplier);
        // Debug.Log($"p2 current combo: {comboTracker}");
        comboNeeded = Mathf.Max(comboNeeded, keyInputPlayer1.comboNeeded);
    }
    // -------------------------------------------------------------------
    void CheckForKeyInput()
    {
        if (Keyboard.current == null) return;
        List<KeyControl> curPressedKeys = GetCurrentKeys();
        if (curPressedKeys.Count < 1) return;
        string curKey = "";
        foreach (KeyControl key in curPressedKeys)
        {
            string strkKey = key.keyCode.ToString();
            Debug.Log($" P2 cur pressed keys: {strkKey}");
            if (!GameStateManager.keyboardMap.ContainsKey(strkKey)) continue;
            curKey = strkKey;
            break;
        }
        // map back to reality
        if (curKey == "") return;
        char cur_key = GameStateManager.keyboardMap[curKey];
        // Debug.Log($"{curKey} was pressed!");
        GameObject curLetterObj = GameStateManager.letterObjects[cur_key];
        KeyEventCoordinator(cur_key.ToString(), curLetterObj);
    }
    // -------------------------------------------------------------------
    List<KeyControl> GetCurrentKeys()
    {
        List<KeyControl> pressedKeys = new List<KeyControl>();

        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                pressedKeys.Add(key);
            }
        }

        return pressedKeys;
    }
    // -------------------------------------------------------------------
    void KeyEventCoordinator(string curPlayerLetter, GameObject letterObject)
    {
        string word = GSM.p2_word;
        int index = GSM.p2_curIndex;

        if (curPlayerLetter[0] == word[index])
        {
            CorrectKeyPressEvents(letterObject);
        }
        else
        {
            IncorrectKeyPressEvents(letterObject);
        }
    }
    // -------------------------------------------------------------------
    void CorrectKeyPressEvents(GameObject letterObject)             
    {
        audio_source.clip = correct_sound;
        audio_source.Play();
        comboTracker++;
        // Debug.Log($"combotracker: {comboTracker} | comboNeeded: {comboNeeded}");

        List<GameObject> spawnedLetters = GameStateManager.p2_spawnedLetters;

        GameObject newLetter = Instantiate(letterObject, correctSpawnPosition);
        Material material =  GSM.p2_platformMat;
        newLetter.GetComponent<LetterBehavior>().isOnPlatform = true;
        newLetter.GetComponentInChildren<Renderer>().material = material;
        newLetter.GetComponent<LetterBehavior>().SetLetterScale(0.5f);

        if (GSM.p2_goldenLetterIndex == GSM.p2_curIndex)
        {
            GoldenFx(newLetter);
            goldenLetterAudioSource.Play();
        }

        // increment correct index pos
        spawnedLetters[GSM.p2_curIndex].SetActive(false);
        GSM.p2_curIndex++;
        GSM.player_2_correct_cnt++;
        GSM.player_2_platformLetters.Add(newLetter);
        GSM.p2_word_set = false;
        

        // every other letter gets collision
        CollisionManagement(newLetter, 2, true);
        if (!flickering)
        {
            flickering = true;
            StartCoroutine(LightFX(true));
        }
    }
    // -------------------------------------------------------------------
    void IncorrectKeyPressEvents(GameObject letterObject)
    {
        Material material =  GSM.p2_platformMat;
        
        audio_source.PlayOneShot(incorrect_sound, 0.2f);
        bool goldenLetter = false;
        comboTracker = 0;
        GameObject newLetter = Instantiate(letterObject, incorrectSpawnPosition);
        lastIncorrectLetter = newLetter.name[0];    
        newLetter.GetComponent<LetterBehavior>().isOnPlatform=true;
        newLetter.GetComponentInChildren<Renderer>().material = material;
        newLetter.GetComponent<LetterBehavior>().SetLetterScale(0.5f);
        GSM.player_1_platformLetters.Add(newLetter);

        if (GSM.p2_goldenLetterIndex == GSM.p2_curIndex)
        {
            goldenLetter = true;
            GoldenFx(newLetter);
        }

        GSM.player_2_incorrect_cnt++;
        
        CollisionManagement(newLetter, goldenLetter ? 1 : 2, false);
        
        RemoveLetterAsPunishment();

        if (!wrong_flicker)
        {
            wrong_flicker = true;
            StartCoroutine(LightFX(false));
        }
    }
    // -------------------------------------------------------------------
    IEnumerator LightFX(bool correct)
    {
        float flickerDuration = 0.2f;
        float startTime = Time.time;
        float multiplier = 5f;

        if (!correct)
        {
            multiplier = 20f;
            shaft_light.color = Color.crimson;
        }

        while (Time.time - startTime < flickerDuration)
        {
            float t = (Time.time - startTime) / flickerDuration;
            float intensity = Mathf.Lerp(
                ogIntensity,
                ogIntensity * multiplier,
                t
            );
            // Debug.Log($"LIGHT INTENSITY: {intensity}");
            shaft_light.intensity = intensity;
            yield return null;
        }

        shaft_light.intensity = ogIntensity;
        if (correct)
        {
            flickering = false;
        }

        if (!correct)
        {
            wrong_flicker = false;
            shaft_light.color = ogColor;
        }
    }
    // -------------------------------------------------------------------
    void CollisionManagement(GameObject newLetter, int mod = 2, bool correct = true)
    {
        int count = correct ? 
            GSM.player_2_platformLetters.Count :
             GSM.player_1_platformLetters.Count;

        // remove collision every once and a while
        if (count % mod == 0)
        {
            int layer = LayerMask.NameToLayer("GhostLetter");
            newLetter.layer = layer;

            foreach (Transform child in newLetter.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = layer;
            }
        }
    }
    // -------------------------------------------------------------------
    void RemoveLetterAsPunishment()
    {
        if (GSM.player_2_platformLetters.Count < 1) return;
        Destroy(GSM.player_2_platformLetters[^1]);
        GSM.player_2_platformLetters.RemoveAt(GSM.player_2_platformLetters.Count - 1);
    }
    // -------------------------------------------------------------------
    void GoldenFx(GameObject goldenLetter)
    {
        goldenLetter.GetComponent<LetterBehavior>().SetLetterMaass(GSM.goldenMassMultiplier);
        goldenLetter.GetComponent<LetterBehavior>().SetLetterMaterial(GSM.GoldenLetterMat);
        goldenLetter.GetComponent<LetterBehavior>().SetLetterScale(1.25f);
    }
    // -------------------------------------------------------------------
    void ShakeCamera()
    {
        impulseSource.GenerateImpulse();
    }
    // -------------------------------------------------------------------
    IEnumerator ComboShaftFX()
    {
        bool shake = false;
        animatingCombo = true;
        
        float cur_range = combo_shaft.range;
    
        // 23 is just figuring out height of shaft in camera
        float desired_range = (float)comboTracker / (float)comboNeeded * shaft_light.range;
  
        float anim_time = 0.2f;
        float startTime = Time.time;

        if (comboTracker >= comboNeeded)
        {
            shake = true;
            comboNeeded = Mathf.Clamp(comboNeeded + 3, 3, 20);
            Debug.Log($"combo needed: {comboNeeded}");
        }
        
        // Debug.Log($"cur range: {cur_range} | desired range: {desired_range}");
        if (GSM.gameStarted && Mathf.Abs(cur_range - desired_range) > 2)
            comboAudioSource.Play();

        while (Time.time - startTime < anim_time)
        {
            float t = (Time.time - startTime) / anim_time;

            combo_shaft.range = Mathf.Lerp(
                cur_range,
                desired_range,
                t
            );
            yield return null;
        }

        combo_shaft.range = desired_range;

        animatingCombo = false;

        if (shake)
        {
            comboTracker = 0;
            GSM.ApplyUpwardForce(false);
            GSM.JumbleLetters(false);
            ShakeCamera();
            yield return new WaitForSeconds(2.0f);
            audio_source.PlayOneShot(GSM.chuckle);
        }
    }
    // -------------------------------------------------------------------
    IEnumerator ComptuerPlayer()
    {
        float speedMultiplier = Random.value * speed * 10;

        if (Random.value < 0.5f)
        {
            speedMultiplier *= -1;
        }

        float curSpeed = speed + speedMultiplier;
        float minSpeed = speed * 2;
        float maxSpeed = speed / 2;

        // Debug.Log($"speedMultiplier: {speedMultiplier} | curSpeed : {curSpeed} | minSpeed {minSpeed} | maxSpeed {maxSpeed}");
 
        curSpeed = Mathf.Clamp(curSpeed, minSpeed, maxSpeed);

        //Debug.Log($"speedMultiplier: {speedMultiplier} | curSpeed : {curSpeed} | minSpeed {minSpeed} | maxSpeed {maxSpeed}");
        char rnd_char = (char)Random.Range(65,90);
        char cur_car = GSM.p2_word[GSM.p2_curIndex];

        // take a pause of variable length every variable amount of time 
        // pause length and when to pause length should be inside a window

        computerPaused = Random.value < 0.05f;

        if (computerPaused)
        {
            float curPauseTime = Random.value * computerPauseTime;
            yield return new WaitForSeconds(Random.value * curPauseTime);
            // Debug.Log($"computer pausing for  : {curPauseTime}");
            computerPaused = false;
        }

        else if (Time.time - timeLastTyped > curSpeed) 
        {
            timeLastTyped = Time.time;
            if (Random.value < accuracy)
            {
                GameObject curLetterObj = GameStateManager.letterObjects[cur_car];
                KeyEventCoordinator(cur_car.ToString(), curLetterObj);
            }
            else
            {
                GameObject curLetterObj = GameStateManager.letterObjects[rnd_char];
                KeyEventCoordinator(rnd_char.ToString(), curLetterObj);
            }   
        }
    }
    // -------------------------------------------------------------------
}

