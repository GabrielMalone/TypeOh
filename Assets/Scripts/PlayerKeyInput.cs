using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
// -----------------------------------------------------------------------
public class PlayerKeyInput : MonoBehaviour
{

    [Header("Alphabet Game Objects")]
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    public GameObject E;
    public GameObject F;
    public GameObject G;
    public GameObject H;
    public GameObject I;
    public GameObject J;
    public GameObject K;
    public GameObject L;
    public GameObject M;
    public GameObject N;
    public GameObject O;
    public GameObject P;
    public GameObject Q;
    public GameObject R;
    public GameObject S;
    public GameObject T;
    public GameObject U;
    public GameObject V;
    public GameObject W;
    public GameObject X;
    public GameObject Y;
    public GameObject Z;


    [Header("Letter Spawn Position")]
    public Transform letterSpawnPosition;
    public Transform player_1_correctSpawnPosition;
    public Transform player_1_incorrectSpawnPosition;


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
        LetterBehavior letterBehavior = letterObject.GetComponent<LetterBehavior>();
        Renderer renderer = letterObject.GetComponentInChildren<Renderer>();

        
        // current word player is currently on
        string curWord = gameStateManager.currentWord;
        // current position in that word (cur char)
        int curIndex = gameStateManager.curIndex;
        // get that char
        char curWordCurLetter = curWord[curIndex];
        
        // are we correct?
        Debug.Log($"Current Player Letter: {curPlayerLetter}");
        Debug.Log($"Current Game word: {curWord}");
        Debug.Log($"Current Game letter: {curWordCurLetter}");

        if (curPlayerLetter[0] == curWordCurLetter)
        {
            gameStateManager.curIndex += 1;
            Material correctMat = letterBehavior.player_1_correctMaterial;
            renderer.material = correctMat;
            Instantiate(letterObject, player_1_correctSpawnPosition);
        }
        else
        {
            Material incorrectMat = letterBehavior.player_1_incorrectMaterial;
            renderer.material = incorrectMat;
            Instantiate(letterObject, player_1_incorrectSpawnPosition);
        }
    }
    // -------------------------------------------------------------------
    void CheckForKeyInput()
    {
        if (Keyboard.current == null) return;

        KeyControl curPressedKey = GetCurrentKey();

        if (curPressedKey == null) return;

        string curKey = curPressedKey.keyCode.ToString();

        switch (curKey)
        {
            case "A":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, A);
                return;

            case "B":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, B);
                return;

            case "C":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, C);
                return;

            case "D":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, D);
                return;

            case "E":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey,E);
                return;

            case "F":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, F);
                return;

            case "G":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, G);
                return;

            case "H":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, H);
                return;

            case "I":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, I);
                return;

            case "J":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, J);
                return;

            case "K":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, K);
                return;

            case "L":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, L);
                return;

            case "M":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, M);
                return;

            case "N":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, N);
                return;

            case "O":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, O);
                return;

            case "P":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, P);
                return;

            case "Q":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, Q);
                return;

            case "R":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, R);
                return;

            case "S":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, S);
                return;

            case "T":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, T);
                return;

            case "U":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, U);
                return;

            case "V":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, V);
                return;

            case "W":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, W);
                return;

            case "X":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, X);
                return;

            case "Y":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, Y);
                return;

            case "Z":
                Debug.Log($"{curKey} was pressed!");
                KeyEventCoordinator(curKey, Z);
                return;

            default:
                return;
        }

    }
    // -------------------------------------------------------------------

}
