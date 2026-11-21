using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.Cinemachine;
using System.Collections.Generic;
// -----------------------------------------------------------------------
public class KeyInputPlayer1 : MonoBehaviour
{

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
    private bool wrong_flicker = false;
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
    public KeyInputPlayer2 keyInputPlayer2;
    private GameStateManager GSM;
    private char lastIncorrectLetter;
    private bool gameOver = false;
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
        }
        if (GSM.player_1_winner)
        {
            gameOver = true;
            shaft_light.color = Color.lawnGreen;
            shaft_light.intensity += 600;
        }
        if (!animatingCombo)
            StartCoroutine(ComboShaftFX());

        if (GSM.P1InTheLead()) 
            comboDecayMultiplier = 2;
        else 
            comboDecayMultiplier = 1;

        comboTracker =  Mathf.Max(0, comboTracker -= Time.deltaTime * comboDecayMultiplier);
        // Debug.Log($"p1 current combo: {comboTracker}");
        comboNeeded = Mathf.Max(comboNeeded, keyInputPlayer2.comboNeeded);
    }
    // -------------------------------------------------------------------
    void CheckForKeyInput()
    {
        if (Keyboard.current == null) return;
        List<KeyControl> curPressedKeys = GetCurrentKeys();
        // iterate through list, for player 1, 
        // if standard kayboard a - z take the first a -z that 
        // was pressed this frame as their input
        // for player 2 take last non a-z that was pressed
        if (curPressedKeys.Count < 1) return;
        string curKey = "";
        foreach(KeyControl key in curPressedKeys)
        {
            string strkKey = key.keyCode.ToString();
            Debug.Log($" P1 cur pressed keys: {strkKey}");
            if (GameStateManager.keyboardMap.ContainsKey(strkKey)) continue;
            curKey = strkKey;
            break;
        }
        if (curKey == "") return;
        char cur_key = curKey[0];
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
        string word = GSM.p1_word;
        int index = GSM.p1_curIndex ;

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

        List<GameObject> spawnedLetters = 
            GameStateManager.p1_spawnedLetters;
         

        GameObject newLetter = Instantiate(letterObject, correctSpawnPosition);
        Material material = GSM.p1_platformMat;
        newLetter.GetComponent<LetterBehavior>().isOnPlatform = true;
        newLetter.GetComponentInChildren<Renderer>().material = material;
        newLetter.GetComponent<LetterBehavior>().SetLetterScale(0.5f);

        if (GSM.p1_goldenLetterIndex == GSM.p1_curIndex)
        {
            GoldenFx(newLetter);
            goldenLetterAudioSource.Play();
        }

        spawnedLetters[GSM.p1_curIndex].SetActive(false);
        GSM.p1_curIndex++;
        GSM.player_1_correct_cnt++;
        GSM.player_1_platformLetters.Add(newLetter);
        GSM.p1_word_set = false;

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
        
        Material material = GSM.p1_platformMat;
        
        audio_source.PlayOneShot(incorrect_sound, 0.2f);
        bool goldenLetter = false;
        comboTracker = 0;
        GameObject newLetter = Instantiate(letterObject, incorrectSpawnPosition);
        lastIncorrectLetter = newLetter.name[0];    
        newLetter.GetComponent<LetterBehavior>().isOnPlatform=true;
        newLetter.GetComponentInChildren<Renderer>().material = material;
        newLetter.GetComponent<LetterBehavior>().SetLetterScale(0.5f);
        GSM.player_2_platformLetters.Add(newLetter);

        if (GSM.p1_goldenLetterIndex == GSM.p1_curIndex)
        {
            goldenLetter = true;
            GoldenFx(newLetter);
        }

        GSM.player_1_incorrect_cnt++;

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
            GSM.player_1_platformLetters.Count :
             GSM.player_2_platformLetters.Count;

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
      if (GSM.player_1_platformLetters.Count > 0)
        {
            if (GSM.player_1_platformLetters.Count < 1) return;
            Destroy(GSM.player_1_platformLetters[^1]);
            GSM.player_1_platformLetters.RemoveAt(GSM.player_1_platformLetters.Count - 1);
        }
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

        if (GSM.gameStarted && Mathf.Abs(cur_range - desired_range) > 2)
            comboAudioSource.Play();
        // Debug.Log($"cur range: {cur_range} | desired range: {desired_range}");
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
            GSM.ApplyUpwardForce(true);
            GSM.JumbleLetters(true);
            ShakeCamera();
            yield return new WaitForSeconds(2.0f);
            audio_source.PlayOneShot(GSM.chuckle);
        }
    }
    // -------------------------------------------------------------------

    // -------------------------------------------------------------------
}

