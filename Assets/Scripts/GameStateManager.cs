using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    [Header("Game State Settings")]
    public string currentWord = "";
    public int curIndex = 0;
    public int player_1_position = 0;

    [Header("Player 1 Word Spawn Settings")]
    public GameObject wordSpawnPlayer1;
    public float distanceBetweenLetters = 0.4f;

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

    public static Dictionary<char, GameObject> letterObjects = new Dictionary<char, GameObject>();
    public static List<GameObject> spawnedLetters = new List<GameObject>();
    public static List<string> englishWords;

    // -------------------------------------------------------------------
    void Awake()
    {
        letterObjects['A'] = A;
        letterObjects['B'] = B;
        letterObjects['C'] = C;
        letterObjects['D'] = D;
        letterObjects['E'] = E;
        letterObjects['F'] = F;
        letterObjects['G'] = G;
        letterObjects['H'] = H;
        letterObjects['I'] = I;
        letterObjects['J'] = J;
        letterObjects['K'] = K;
        letterObjects['L'] = L;
        letterObjects['M'] = M;
        letterObjects['N'] = N;
        letterObjects['O'] = O;
        letterObjects['P'] = P;
        letterObjects['Q'] = Q;
        letterObjects['R'] = R;
        letterObjects['S'] = S;
        letterObjects['T'] = T;
        letterObjects['U'] = U;
        letterObjects['V'] = V;
        letterObjects['W'] = W;
        letterObjects['X'] = X;
        letterObjects['Y'] = Y;
        letterObjects['Z'] = Z;

        // load all english words
        // https://github.com/dwyl/english-words
        TextAsset file = Resources.Load<TextAsset>("words_alpha");

        // put them in the list
        englishWords = new List<string>(
            file.text.Split('\n')
        );

    }
    // -------------------------------------------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWord = GetNewWord();
        SpawnCurrentWord();
        Debug.Log($"CURRENT WORD: {currentWord}");
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (CurWordIsFinished())
        {
            // get rid of old letters
            foreach (GameObject letter in spawnedLetters)
            {
                Destroy(letter);
            }
            spawnedLetters.Clear();
            // pick a new world
            currentWord = GetNewWord();
            // spawn letters for new word
            SpawnCurrentWord();
            Debug.Log("FINISHED WORD!");
        }
        UpdateWordMaterials();
    }
    // -------------------------------------------------------------------
    public string GetNewWord()
    {
        // for now just reset
        curIndex = 0;
        int index = Random.Range(0, englishWords.Count);
        return englishWords[index].ToUpper().Trim();

    }
    // -------------------------------------------------------------------
    public bool CurWordIsFinished()
    {
        return curIndex >= currentWord.Length;
    }
    // -------------------------------------------------------------------
    void SpawnCurrentWord()
    {
        float nxtPosition = 0;
        foreach (char letter in currentWord)
        {
            GameObject letterObj = Instantiate(
                // letter gameobj
                SetLetterObj(letter),
                // scooch over to the right a bit
                wordSpawnPlayer1.transform.position + new Vector3(nxtPosition, 0, 0),
                // set rotation
                Quaternion.Euler(0, 180, 0)
            );
            nxtPosition += distanceBetweenLetters;
            // keep track of these letter objects
            spawnedLetters.Add(letterObj);
        }
    }
    // -------------------------------------------------------------------
    public void UpdateWordMaterials()
    {
        for (int i = 0; i < currentWord.Length; i++)
        {

            char letter = currentWord[i];
            GameObject curLetterObj = spawnedLetters[i];

            LetterBehavior lb = curLetterObj.GetComponent<LetterBehavior>();
            Renderer renderer = curLetterObj.GetComponentInChildren<Renderer>();

            // highlight the current letter
            if (curIndex == i && letter == currentWord[curIndex])
            {
                renderer.material = lb.CurLetterMaterial;
            }
            else
            {
                renderer.material = lb.NextLetterMaterial;
            }

        }
    }
    // -------------------------------------------------------------------
    GameObject SetLetterObj(char letter)
    {
        // set time to live
        GameObject curLetterObj = letterObjects[letter];
        LetterBehavior lb = curLetterObj.GetComponent<LetterBehavior>();
        lb.timeToLive = 100000f;

        // set up letter color change
        LetterBehavior letterBehavior = curLetterObj.GetComponent<LetterBehavior>();
        Renderer renderer = curLetterObj.GetComponentInChildren<Renderer>();

        if (letter == currentWord[curIndex])
        {
            renderer.material = letterBehavior.CurLetterMaterial;
        }
        else
        {
            renderer.material = letterBehavior.NextLetterMaterial;
        }

        return curLetterObj;
    }


}
