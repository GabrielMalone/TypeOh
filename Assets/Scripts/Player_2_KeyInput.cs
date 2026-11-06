using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
// -----------------------------------------------------------------------
public class Player_2_KeyInput : MonoBehaviour
{
    [Header("Letter Spawn Position")]
    public Transform letterSpawnPosition;
    public Transform player_2_correctSpawnPosition;
    public Transform player_2_incorrectSpawnPosition;

    [Header("Letter Spawn Time To Live Settings")]
    public float correctLetterTTL = 1.0f;
    public float inccorectLetterTTL = 2.0f;
    public float correctCombo = 0;
    public float incorrectCombo = 0;
    public float maxTimeToLIveCombo = 120f;
    public float comboMultiplier = 1.2f;

    [Header("platform letter scale settings")]
    public float letterScale = 1f;

    [Header("Mistake Settings")]
    public float mistakeSpawnInterval = 0.2f;
    private float lastTimeCheck = 0f; 


    [Header("Sound FX Settings")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    [Header("Game State Manager")]
    public GameObject stateManager;
    private GameStateManager gameStateManager;
    private char lastIncorrectLetter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStateManager = stateManager.GetComponent<GameStateManager>();

    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        CheckForKeyInput();
        
        // keep spamming incorrect until player gets correct
        if (incorrectCombo > 0 && Time.time - lastTimeCheck > mistakeSpawnInterval)
        {
            SpawnIncorrectLetters(lastIncorrectLetter);
            lastTimeCheck = Time.time;
        }
    
    }
    // -------------------------------------------------------------------
    KeyControl GetCurrentKey()
    {
        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                Debug.Log($"Player 2 Pressed: {key}");
                return key;
            }
        }
        return null;
    }
    // -------------------------------------------------------------------
    void KeyEventCoordinator(string curPlayerLetter, GameObject letterObject)
    {
        // set up letter color change
        LetterBehavior letterBehavior = letterObject.GetComponent<LetterBehavior>();
        Renderer renderer = letterObject.GetComponentInChildren<Renderer>();

        letterBehavior.letterScale=letterScale;

        // current word player is currently on
        string curWord = gameStateManager.currentWord;
        // current position in that word (cur char)
        int curIndex = gameStateManager.p2_curIndex;
        // get that char
        char curWordCurLetter = curWord[curIndex];

        // correct path
        if (curPlayerLetter[0] == curWordCurLetter)
        {
            incorrectCombo = 0;
            Debug.Log($"PLAYER TWO CORRECT!");
            // increment correct index pos
            gameStateManager.p2_curIndex += 1;
            gameStateManager.player_2_correct_cnt++;
            // set color for correct on letter to spawn
            Material correctMat = letterBehavior.player_1_correctMaterial;
            renderer.material = correctMat;

            GameObject newLetter = Instantiate(letterObject, player_2_correctSpawnPosition);
            newLetter.transform.localScale =  Vector3.one * letterScale; 
            LetterBehavior lb = newLetter.GetComponent<LetterBehavior>();
            Rigidbody rb = newLetter.GetComponent<Rigidbody>();
            // track total weight on platform
            gameStateManager.player_2_platformLetters.Add(newLetter);

            audioSource.PlayOneShot(correctSound);
        }
        // incorrect path
        else
        {
            Debug.Log($"PLAYER TWO INCORRECT!");
            gameStateManager.player_2_incorrect_cnt ++ ;
            Material incorrectMat = letterBehavior.player_1_incorrectMaterial;
            renderer.material = incorrectMat;
            GameObject newLetter =  Instantiate(letterObject, player_2_incorrectSpawnPosition);
            newLetter.transform.localScale =  Vector3.one * letterScale;


            // set time to live
            incorrectCombo++;

            correctCombo = 0;
    

            lastIncorrectLetter = newLetter.name[0];
            audioSource.PlayOneShot(incorrectSound);
        }
    }
    // -------------------------------------------------------------------
    void CheckForKeyInput()
    {
        if (Keyboard.current == null) return;

        KeyControl curPressedKey = GetCurrentKey();

        if (curPressedKey == null) return;

        string curKey = curPressedKey.keyCode.ToString();

        Debug.Log($"{curKey} was pressed!");

        if (!GameStateManager.keyboardMap.ContainsKey(curKey)) return;

        // need to map back to original keys here
        char cur_key = GameStateManager.keyboardMap[curKey];

        GameObject curLetterObj = GameStateManager.letterObjects[cur_key];

        KeyEventCoordinator(cur_key.ToString(), curLetterObj);
    }
    // -------------------------------------------------------------------
    void SpawnIncorrectLetters(char lastIncorrectLetter)
    {
        GameObject letterObject = GameStateManager.letterObjects[lastIncorrectLetter];
        LetterBehavior letterBehavior = letterObject.GetComponent<LetterBehavior>();
        Renderer renderer = letterObject.GetComponentInChildren<Renderer>();
        Material incorrectMat = letterBehavior.player_1_incorrectMaterial;
        renderer.material = incorrectMat;
        GameObject newLetter =  Instantiate(letterObject, player_2_incorrectSpawnPosition);
        // fixes late scaling
        newLetter.transform.localScale =  Vector3.one * letterScale;     
    }


}
