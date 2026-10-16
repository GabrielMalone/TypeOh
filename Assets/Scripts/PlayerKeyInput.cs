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
        }
        else
        {
            Material incorrectMat = letterBehavior.player_1_incorrectMaterial;
            renderer.material = incorrectMat;
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
                GameObject curLetterObj = Instantiate(A, letterSpawnPosition);
                // set material here
                KeyEventCoordinator(curKey, curLetterObj);
                // renderer.setMaterial = material for different eveents there. 

                return;

            case "B":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(B, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "C":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(C, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "D":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(D, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "E":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(E, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "F":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(F, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "G":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(G, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "H":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(H, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "I":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(I, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "J":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(J, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "K":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(K, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "L":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(L, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "M":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(M, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "N":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(N, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "O":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(O, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "P":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(P, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "Q":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(Q, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "R":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(R, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "S":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(S, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "T":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(T, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "U":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(U, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "V":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(V, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "W":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(W, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "X":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(X, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "Y":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(Y, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            case "Z":
                Debug.Log($"{curKey} was pressed!");
                curLetterObj = Instantiate(Z, letterSpawnPosition);
                KeyEventCoordinator(curKey, curLetterObj);
                return;

            default:
                return;
        }

    }
    // -------------------------------------------------------------------

}
