using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
// -----------------------------------------------------------------------
public class KeyInput : MonoBehaviour
{

    [Header("Player Selection")]
    public bool player_1 = false;
    public bool player_2 = false;

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

    [Header("Correct Settings")]
    public int comboTracker;
    public int comboNeeded = 3;
    public Light player_1_shaft_light;
    public Light player_2_shaft_light;
    private Light shaft_light;
    private bool flickering = false;
    private bool wrong_flicker = false;
    private float ogIntensity;
    private Color ogColor;

    [Header("Sound FX Settings")]
    public AudioSource p1_audio_source;
    public AudioSource p2_audio_source;
    public AudioClip p1_correct_sound;
    public AudioClip p2_correct_sound;
    public AudioClip p1_incorrect_sound;
    public AudioClip p2_incorrect_sound;
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
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (!gameOver && GSM.gameStarted)
            CheckForKeyInput();

        if (( player_1 && GSM.player_1_winner || player_2 && GSM.player_2_winner) && !gameOver)
        {
            gameOver = true;
            shaft_light.color = Color.lawnGreen;
            shaft_light.intensity = 600;
        }
    }
    // -------------------------------------------------------------------
    void CheckForKeyInput()
    {
        if (Keyboard.current == null) return;
        KeyControl curPressedKey = GetCurrentKey();

        if (curPressedKey == null) return;
        string curKey = curPressedKey.keyCode.ToString();

        if (player_1 &&  GameStateManager.keyboardMap.ContainsKey(curKey)) return;
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
                // Debug.Log($"Player 2 Pressed: {key}");
                return key;
            }
        }
        return null;
    }
    // -------------------------------------------------------------------

    void KeyEventCoordinator(string curPlayerLetter, GameObject letterObject)
    {
        // set up letter color change
        LetterBehavior lb = letterObject.GetComponent<LetterBehavior>();
        lb.SetLetterMaterial(correct_material);

        // current word player is currently on
        // current position in that word (cur char)
        if (curPlayerLetter[0] == (
            player_1 ?
            GSM.currentWord[GSM.p1_curIndex] : 
            GSM.currentWord[GSM.p2_curIndex])
        )
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

        comboTracker ++;
        // Debug.Log($"combotracker: {comboTracker} | comboNeeded: {comboNeeded}");
        if (comboTracker == comboNeeded)
        {
            comboTracker = 0;
            GSM.ApplyUpwardForce(player_1);
            // comboNeeded += 3;
        }

        // set color for correct on letter to spawn
        GameObject newLetter = Instantiate(letterObject, correctSpawnPosition);
        // increment correct index pos
        if (player_1)
        {
            if (GSM.goldenLetterIndex == GSM.p1_curIndex)
            {
                GoldenFx(newLetter);
                GSM.goldenLetterTriggered = true;
            }
            GSM.p1_curIndex++;
            GSM.player_1_correct_cnt++;
            GSM.player_1_platformLetters.Add(newLetter);
        }
        else
        {
            if (GSM.goldenLetterIndex == GSM.p2_curIndex)
            {
                GoldenFx(newLetter);
                GSM.goldenLetterTriggered = true;
            }
            GSM.p2_curIndex++;
            GSM.player_2_correct_cnt++;
            GSM.player_2_platformLetters.Add(newLetter);
        }
        // every third letter gets collision
        CollisionManagement(newLetter);
        if (!flickering)
        {
            flickering = true;
            StartCoroutine(LightFX(true));
        }
        // audioSource.PlayOneShot(correctSound);
    }
    // -------------------------------------------------------------------
    void IncorrectKeyPressEvents(GameObject letterObject)
    {
        bool goldenLetter = false;
        GameObject newLetter = Instantiate(letterObject, incorrectSpawnPosition);
        lastIncorrectLetter = newLetter.name[0];
        if (player_1)
        {
            if (GSM.goldenLetterIndex == GSM.p1_curIndex)
            {
                goldenLetter = true;
                GoldenFx(newLetter);
            }
            GSM.player_1_incorrect_cnt++;
        }
        else
        {
            if (GSM.goldenLetterIndex == GSM.p2_curIndex)
            {
                goldenLetter = true;
                GoldenFx(newLetter);
            }
            GSM.player_2_incorrect_cnt++;
        }
        CollisionManagement(newLetter, goldenLetter ? 1 : 2);
        RemoveLetterAsPunishment();        
        if (!wrong_flicker)
        {
            wrong_flicker = true;
            StartCoroutine(LightFX(false));
        }
        //Debug.Log($"PLAYER ONE INCORRECT!");
        // audioSource.PlayOneShot(incorrectSound);
    }
    // -------------------------------------------------------------------
    IEnumerator LightFX(bool correct)
    {
        float flickerDuration = 0.2f;
        float startTime = Time.time;
        float multiplier = 5;

        float intensity = ogIntensity;

        if (!correct)
        {
            multiplier = 20;
            shaft_light.color = shaft_light.color = Color.crimson;
        }

        while (intensity < ogIntensity * multiplier)
        {
            float t = (Time.time - startTime) / flickerDuration;
            intensity = Mathf.Lerp(
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
        if (player_1 ? 
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
}

