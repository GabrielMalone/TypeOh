using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
// -----------------------------------------------------------------------
public class PlayerKeyInput : MonoBehaviour
{
    [Header("Letter Spawn Position")]
    public Transform letterSpawnPosition;
    public Transform player_1_correctSpawnPosition;
    public Transform player_1_incorrectSpawnPosition;

    [Header("Letter Spawn Time To Live Settings")]
    public float correctLetterTTL = 1.0f;
    public float inccorectLetterTTL = 2.0f;
    public float correctCombo = 0;
    public float incorrectCombo = 0;

    [Header("Sound FX Settings")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    [Header("Game State Manager")]
    public GameObject stateManager;
    private GameStateManager gameStateManager;

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
        // set up letter color change
        LetterBehavior letterBehavior = letterObject.GetComponent<LetterBehavior>();
        Renderer renderer = letterObject.GetComponentInChildren<Renderer>();

        // current word player is currently on
        string curWord = gameStateManager.currentWord;
        // current position in that word (cur char)
        int curIndex = gameStateManager.curIndex;
        // get that char
        char curWordCurLetter = curWord[curIndex];
        
        // are we correct?
        // Debug.Log($"Current Player Letter: {curPlayerLetter}");
        // Debug.Log($"Current Game word: {curWord}");
        // Debug.Log($"Current Game letter: {curWordCurLetter}");

        // correct path
        if (curPlayerLetter[0] == curWordCurLetter)
        {
            // increment correct index pos
            gameStateManager.curIndex += 1;
            // set color for correct on letter to spawn
            Material correctMat = letterBehavior.player_1_correctMaterial;
            renderer.material = correctMat;
  
            GameObject newLetter = Instantiate(letterObject, player_1_correctSpawnPosition);
            LetterBehavior lb = newLetter.GetComponent<LetterBehavior>();

            // set time to live
            correctCombo ++;
            incorrectCombo = 0;
            lb.timeToLive = correctLetterTTL;

            audioSource.PlayOneShot(correctSound);
        }
        // incorrect path
        else
        {
            Material incorrectMat = letterBehavior.player_1_incorrectMaterial;
            renderer.material = incorrectMat;
            GameObject newLetter =  Instantiate(letterObject, player_1_incorrectSpawnPosition);
            LetterBehavior lb = newLetter.GetComponent<LetterBehavior>();
            lb.timeToLive = inccorectLetterTTL;
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
        // Debug.Log($"{curKey} was pressed!");
        GameObject curLetterObj = GameStateManager.letterObjects[curKey[0]];
        KeyEventCoordinator(curKey, curLetterObj);
    }
    // -------------------------------------------------------------------

}
