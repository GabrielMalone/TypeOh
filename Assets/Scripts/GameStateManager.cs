using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    [Header("Game State Settings")]
    public string currentWord = "";
    public int curIndex = 0;
    public int player_1_position = 0;

    [Header("Player Settings")]
    public GameObject player_1;
    public GameObject player_2;

    [Header("Player 1 Word Spawn Settings")]
    public GameObject wordSpawnPlayer1;
    public float distanceBetweenLetters = 0.4f;
    public float letterScale = 1f;
    public GameObject wordSpawnArea;

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
    public static Dictionary<string, char> keyboardMap = new Dictionary<string, char>();
    public static List<GameObject> spawnedLetters = new List<GameObject>();
    public static List<string> englishWords;


    // -------------------------------------------------------------------
    // TODO -> 
    // 1 ) quantify what is a win and make it clear (like platform past a certain plane)
    // 2 ) some telemetry might be fun, like wpm / accuracy
    // 3 ) fun facts on words being displayed (definitions)?
    // 4 ) P1 and P2 arrows over where each is in the word. 
    // 5 ) if player laps another player, what happens?
    // 6 ) sound for letters going away from TTL expiring
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

        // map for player 2 keyboard
        // which was mapped w/ outside program
        
        keyboardMap["Digit1"] = 'A';
        keyboardMap["Digit2"] = 'B';
        keyboardMap["Digit3"] = 'C';
        keyboardMap["Digit4"] = 'D';
        keyboardMap["Digit5"] = 'E';
        keyboardMap["Digit6"] = 'F';
        keyboardMap["Digit7"] = 'G';
        keyboardMap["Digit8"] = 'H';
        keyboardMap["Digit9"] = 'I';
        keyboardMap["Digit0"] = 'J';
        keyboardMap["Numpad1"] = 'K';
        keyboardMap["Numpad2"] = 'L';
        keyboardMap["Numpad3"] = 'M';
        keyboardMap["Numpad4"] = 'N';
        keyboardMap["Numpad5"] = 'O';
        keyboardMap["Numpad6"] = 'P';
        keyboardMap["Numpad7"] = 'Q';
        keyboardMap["Numpad8"] = 'R';
        keyboardMap["Numpad9"] = 'S';
        keyboardMap["Numpad0"] = 'T';
        keyboardMap["UpArrow"] = 'U';
        keyboardMap["DownArrow"] = 'V';
        keyboardMap["LeftArrow"] = 'W';
        keyboardMap["RightArrow"] = 'X';
        keyboardMap["Comma"] = 'Y';
        keyboardMap["Period"] = 'Z';

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
        CenterAndSpawnWord();
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
            CenterAndSpawnWord();
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
    public void UpdateWordMaterials()
    {
        if (currentWord.Length <= 0) return;
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
    GameObject SetLetterObj(char letter, bool mainWord = false)
    {
        // set time to live
        GameObject curLetterObj = letterObjects[letter];
        LetterBehavior lb = curLetterObj.GetComponent<LetterBehavior>();
        lb.timeToLive = 100000f;
        lb.letterScale = letterScale;

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
    // -------------------------------------------------------------------
    void CenterAndSpawnWord()
    {
        MeshCollider mc = wordSpawnArea.GetComponent<MeshCollider>();
        // get the center point of our wordSpawn area
        int lenWord = currentWord.Length;
        
        float xMin = mc.bounds.min.x;
        float xMax = mc.bounds.max.x;
        float xDistance = xMin - xMax;
        
        float xCenter = xDistance / 2;
        int centerLetterIndex = lenWord / 2;

        float farLeft = xCenter - (distanceBetweenLetters * (lenWord - centerLetterIndex));

        // first spawn to the left of center
        float nxtPosition = 0;

        // Debug.Log($"lenword: {lenWord} xCenter: {xCenter} centerIndex: {centerLetterIndex}");

        for (int i = 0 ; i < centerLetterIndex ; i ++)
        {
            char letter = currentWord[i];
            GameObject letterObj = Instantiate(
                // letter gameobj
                SetLetterObj(letter),
                // start far left then scooch right
                wordSpawnPlayer1.transform.position + new Vector3(farLeft + nxtPosition, 0, 0),
                // set rotation
                Quaternion.Euler(0, 180, 0)
            );
            nxtPosition += distanceBetweenLetters;
            // prevent tipping over
            Rigidbody rb = letterObj.GetComponent<Rigidbody>();
            if (rb!=null)
                rb.constraints = RigidbodyConstraints.FreezeAll;
            // keep track of these letter objects
            spawnedLetters.Add(letterObj);            
        }
      
        nxtPosition = 0;
        // fix weird spacing on odd lengthed words
        if (lenWord % 2 != 0) xCenter -= distanceBetweenLetters;
        // then spawn to the right of center
        for (int i = centerLetterIndex ; i < lenWord ; i ++)
        {
            char letter = currentWord[i];
            GameObject letterObj = Instantiate(
                // letter gameobj
                SetLetterObj(letter),
                // start far left then scooch right
                wordSpawnPlayer1.transform.position + new Vector3(xCenter + nxtPosition, 0, 0),
                // set rotation
                Quaternion.Euler(0, 180, 0)
            );
            nxtPosition += distanceBetweenLetters;
            // prevent tipping over
            Rigidbody rb = letterObj.GetComponent<Rigidbody>();
            if (rb!=null)
                rb.constraints = RigidbodyConstraints.FreezeAll;
            // keep track of these letter objects
            spawnedLetters.Add(letterObj);            
        }
    }


}
