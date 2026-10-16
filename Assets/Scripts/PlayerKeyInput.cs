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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
                Instantiate(A, letterSpawnPosition);
                return;

            case "B":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(B, letterSpawnPosition);
                return;

            case "C":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(C, letterSpawnPosition);
                return;

            case "D":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(D, letterSpawnPosition);
                return;

            case "E":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(E, letterSpawnPosition);
                return;

            case "F":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(F, letterSpawnPosition);
                return;

            case "G":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(G, letterSpawnPosition);
                return;

            case "H":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(H, letterSpawnPosition);
                return;

            case "I":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(I, letterSpawnPosition);
                return;

            case "J":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(J, letterSpawnPosition);
                return;

            case "K":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(K, letterSpawnPosition);
                return;

            case "L":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(L, letterSpawnPosition);
                return;

            case "M":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(M, letterSpawnPosition);
                return;

            case "N":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(N, letterSpawnPosition);
                return;

            case "O":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(O, letterSpawnPosition);
                return;

            case "P":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(P, letterSpawnPosition);
                return;

            case "Q":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(Q, letterSpawnPosition);
                return;

            case "R":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(R, letterSpawnPosition);
                return;

            case "S":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(S, letterSpawnPosition);
                return;

            case "T":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(T, letterSpawnPosition);
                return;

            case "U":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(U, letterSpawnPosition);
                return;

            case "V":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(V, letterSpawnPosition);
                return;

            case "W":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(W, letterSpawnPosition);
                return;

            case "X":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(X, letterSpawnPosition);
                return;

            case "Y":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(Y, letterSpawnPosition);
                return;

            case "Z":
                Debug.Log($"{curKey} was pressed!");
                Instantiate(Z, letterSpawnPosition);
                return;

            default:
                return;
        }

    }
    // -------------------------------------------------------------------

}
