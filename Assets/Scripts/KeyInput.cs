using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.Cinemachine;
using System.Collections.Generic;
// -----------------------------------------------------------------------
public class KeyInput : MonoBehaviour
{

    [Header("Player Selection")]
    public bool player_1 = false;
    public bool player_2 = false;
    [SerializeField] public static bool computer = false;

    [Header("Computer Player Settings")]
    public float speed = 1f;
    public float accuracy = 0.5f;
    private float timeLastTyped = 0f;

    [Header("Letter Spawn Position")]
    public Transform player_1_correctSpawnPosition;
    public Transform player_1_incorrectSpawnPosition;
    public Transform player_2_correctSpawnPosition;
    public Transform player_2_incorrectSpawnPosition;
    private Transform correctSpawnPosition;
    private Transform incorrectSpawnPosition;

    [Header("Letter Material Settings")]
    public Material player_1_correct_material;
    public Material player_2_correct_material;
    private Material correct_material;

    [Header("platform letter scale settings")]
    public float letterScale = 1f;

    [Header("Mistake Settings")]
    public float mistakeSpawnInterval = 0.2f;
    public CinemachineImpulseSource impulseSource;

    [Header("Correct Settings")]
    public int comboTracker;
    public int comboNeeded = 3;
    public Light p1_combo_shaft;
    public Light p2_combo_shaft;
    private Light combo_shaft;
    private bool animatingCombo = false;
    public Light player_1_shaft_light;
    public Light player_2_shaft_light;
    private Light shaft_light;
    private bool flickering = false;
    private bool wrong_flicker = false;
    private float ogIntensity;
    private Color ogColor;

    [Header("Sound FX Settings")]
    public AudioSource p1_comboAudioSource;
    public AudioSource p2_comboAudioSource;
    private AudioSource comboAudioSource;
    public AudioSource p1_audio_source;
    public AudioSource p2_audio_source;
    public AudioClip p1_correct_sound;
    public AudioClip p2_correct_sound;
    public AudioClip p1_incorrect_sound;
    public AudioClip p2_incorrect_sound;
    public AudioSource goldenLetterAudioSource;
    private AudioSource audio_source;
    private AudioClip correct_sound;
    private AudioClip incorrect_sound;

    [Header("Game State Manager")]
    public GameObject stateManager;
    private GameStateManager GSM;
    private char lastIncorrectLetter;
    private bool gameOver = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GSM = stateManager.GetComponent<GameStateManager>();
        shaft_light = player_1 ? player_1_shaft_light : player_2_shaft_light;
        correct_material = player_1 ? player_1_correct_material : player_2_correct_material;
        correctSpawnPosition = player_1 ? player_1_correctSpawnPosition : player_2_correctSpawnPosition;
        incorrectSpawnPosition = player_1 ? player_1_incorrectSpawnPosition : player_2_incorrectSpawnPosition;
        audio_source = player_1 ? p1_audio_source : p2_audio_source;
        correct_sound = player_1 ? p1_correct_sound : p2_correct_sound;
        incorrect_sound = player_1 ? p1_incorrect_sound : p2_incorrect_sound;
        ogIntensity = shaft_light.intensity;
        ogColor = shaft_light.color;
        combo_shaft = player_1 ? p2_combo_shaft : p1_combo_shaft;
        comboAudioSource = player_1 ? p1_comboAudioSource : p2_comboAudioSource;
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (!gameOver && GSM.gameStarted)
        {
            CheckForKeyInput();
            ComptuerPlayer();
        }
        if (player_1 && GSM.player_1_winner || player_2 && GSM.player_2_winner)
        {
            gameOver = true;
            shaft_light.color = Color.lawnGreen;
            shaft_light.intensity += 600;
        }
        if (!animatingCombo)
            StartCoroutine(ComboShaftFX());
    }
    // -------------------------------------------------------------------
    void CheckForKeyInput()
    {
        if (Keyboard.current == null) return;
        KeyControl curPressedKey = GetCurrentKey();

        if (curPressedKey == null) return;
        string curKey = curPressedKey.keyCode.ToString();

        if (player_1 && GameStateManager.keyboardMap.ContainsKey(curKey)) return;
        if (player_2 && !GameStateManager.keyboardMap.ContainsKey(curKey)) return;

        char cur_key = curKey[0];
        // map back to reality
        if (player_2)
            cur_key = GameStateManager.keyboardMap[curKey];
        // Debug.Log($"{curKey} was pressed!");

        GameObject curLetterObj = GameStateManager.letterObjects[cur_key];
        KeyEventCoordinator(cur_key.ToString(), curLetterObj);
    }
    // -------------------------------------------------------------------
    KeyControl GetCurrentKey()
    {
        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                return key;
            }
        }
        return null;
    }
    // -------------------------------------------------------------------
    void KeyEventCoordinator(string curPlayerLetter, GameObject letterObject)
    {
        string word = player_1 ? GSM.p1_word : GSM.p2_word;
        int index = player_1 ? GSM.p1_curIndex : GSM.p2_curIndex;

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
            player_1 ? 
            GameStateManager.p1_spawnedLetters : 
            GameStateManager.p2_spawnedLetters;

        GameObject newLetter = Instantiate(letterObject, correctSpawnPosition);
        Material material = player_1 ? GSM.p1_Material : GSM.p2_Material;
        newLetter.GetComponent<LetterBehavior>().isOnPlatform = true;
        newLetter.GetComponentInChildren<Renderer>().material = material;

        if (player_1 && GSM.p1_goldenLetterIndex == GSM.p1_curIndex)
        {
            GoldenFx(newLetter);
            goldenLetterAudioSource.Play();
        }

        if (!player_1 && GSM.p2_goldenLetterIndex == GSM.p2_curIndex)
        {
            GoldenFx(newLetter);
            goldenLetterAudioSource.Play();
        }
     
        // increment correct index pos
        if (player_1)
        {
            // gravity drop fx
            spawnedLetters[GSM.p1_curIndex].SetActive(false);
            GSM.p1_curIndex++;
            GSM.player_1_correct_cnt++;
            GSM.player_1_platformLetters.Add(newLetter);
            GSM.p1_word_set = false;
        }
        if (!player_1)
        {
            spawnedLetters[GSM.p2_curIndex].SetActive(false);
            GSM.p2_curIndex++;
            GSM.player_2_correct_cnt++;
            GSM.player_2_platformLetters.Add(newLetter);
            GSM.p2_word_set = false;
        }

        // every third letter gets collision
        CollisionManagement(newLetter);
        if (!flickering)
        {
            flickering = true;
            StartCoroutine(LightFX(true));
        }
    }
    // -------------------------------------------------------------------
    void IncorrectKeyPressEvents(GameObject letterObject)
    {
        
        Material material = player_1 ? GSM.p1_Material : GSM.p2_Material;
        
        audio_source.PlayOneShot(incorrect_sound);
        bool goldenLetter = false;
        comboTracker = 0;
        GameObject newLetter = Instantiate(letterObject, incorrectSpawnPosition);
        lastIncorrectLetter = newLetter.name[0];    
        newLetter.GetComponent<LetterBehavior>().isOnPlatform=true;
        newLetter.GetComponentInChildren<Renderer>().material = material;

        if (player_1 && GSM.p1_goldenLetterIndex == GSM.p1_curIndex)
        {
            goldenLetter = true;
            GoldenFx(newLetter);
        }

        if (player_1)
            GSM.player_1_incorrect_cnt++;

        if (!player_1 && GSM.p2_goldenLetterIndex == GSM.p2_curIndex)
        {
            goldenLetter = true;
            GoldenFx(newLetter);
        }

        if (!player_1)
            GSM.player_2_incorrect_cnt++;

        CollisionManagement(newLetter, goldenLetter ? 1 : 2);

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
    void CollisionManagement(GameObject newLetter, int mod = 2)
    {
        // remove collision every once and a while
        if (
            player_1 ?
            GSM.player_1_incorrect_cnt % mod == 0 :
            GSM.player_2_incorrect_cnt % mod == 0
            )
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
        if (player_2 && GSM.player_2_platformLetters.Count > 0)
        {
            Destroy(GSM.player_2_platformLetters[^1]);
            GSM.player_2_platformLetters.RemoveAt(GSM.player_2_platformLetters.Count - 1);
        }
        else if (player_1 && GSM.player_1_platformLetters.Count > 0)
        {
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
        float desired_range = (float)comboTracker / (float)comboNeeded * 23;

        float anim_time = 0.2f;
        float startTime = Time.time;

        if (comboTracker >= comboNeeded)
        {
            shake = true;
        }
        // Debug.Log($"cur range: {cur_range} | desired range: {desired_range}");
        while (Time.time - startTime < anim_time)
        {
            float t = (Time.time - startTime) / anim_time;
            combo_shaft.range = Mathf.Lerp(
                cur_range,
                desired_range,
                t
            );
            if (GSM.gameStarted && Mathf.Abs(cur_range - desired_range) > 2)
                comboAudioSource.Play();
            yield return null;
        }

        combo_shaft.range = desired_range;
        animatingCombo = false;

        if (shake)
        {
            audio_source.PlayOneShot(GSM.chuckle);
            comboTracker = 0;
            GSM.ApplyUpwardForce(player_1);
            GSM.JumbleLetters(player_1);
            ShakeCamera();
        }
    }
    // -------------------------------------------------------------------
    void ComptuerPlayer()
    {   
        if (!computer) return;
 
        char rnd_char = (char)Random.Range(65,90);
        char cur_car = GSM.p2_word[GSM.p2_curIndex];

        if (Time.time - timeLastTyped > speed) 
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

