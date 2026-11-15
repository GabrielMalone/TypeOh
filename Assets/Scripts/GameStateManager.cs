using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor.Callbacks;

public class GameStateManager : MonoBehaviour
{
    [Header("Game State Settings")]
    public string p1_word = "";
    public string p2_word = "";
    public int p1_curIndex = 0;
    public int p2_curIndex = 0;
    public float maxPlatformForce = 4900f;
    public float comboForce = 1000f;
    public int p1_goldenLetterIndex = -1;
    public int p2_goldenLetterIndex = -1;
    public float goldenMassMultiplier = 10f;
    public float goldenLetterScaler = 1f;

    [Header("Game Start Settings")]
    public int countDownTime = 3;
    public float musicFadeInTime = 3f;
    public GameObject p1_platform;
    public GameObject p2_platform;
    public float totalPlatformDistance;

    [Header("Game Music")]
    public AudioSource gameMusicSource;
    public AudioClip gameMusic;
    public AudioSource countDownSource;
    public AudioClip beep_1;
    public AudioClip calmMusic;

    // sfx currently scattered throughout game
    [Header("Game SFX")]
    public AudioClip chuckle;
    public AudioClip scream;
    public AudioClip [] screams;
    public AudioClip computerEngageSound;
    public AudioSource ambientAudioSource;


    [Header("Player Settings")]
    public GameObject player_1;
    public GameObject player_2;
    public Material player_1_material;
    public Material player_2_material;
    public GameObject player_1_platform;
    public GameObject player_2_platform;
    public Material p1_platformMat;
    public Material p2_platformMat;

    [Header("Player Stats")]
    public SpringJoint player_1_spring;
    public SpringJoint player_2_spring;
    public GameObject finishLine;
    public float player_1_correct_cnt;
    public float player_2_correct_cnt;
    public float player_1_incorrect_cnt;
    public float player_2_incorrect_cnt;
    public GameObject player_1_stats_area;
    public GameObject player_2_stats_area;
    public List<GameObject> player_1_platformLetters = new List<GameObject>();
    public List<GameObject> player_2_platformLetters = new List<GameObject>();
    public float p1_percent_complete;
    public float p2_percent_complete;

    [Header("End Game Settings")]
    public bool gameStarted = false;
    public bool player_1_winner = false;
    public bool player_2_winner = false;
    public bool endGame = false;
    public AudioClip victorySound;

    [Header("Player Word Spawn Settings")]
    public float distanceBetweenLetters = 0.4f;
    public float letterScale = 1f;
    public GameObject p1_wordSpawnArea;
    public GameObject p2_wordSpawnArea;
    public GameObject countDownSpawnArea;
    public Material GoldenLetterMat;
    public Material basicLetterMat;
    public Material p1_Material;
    public Material p2_Material;

    [Header("specialFX")]
    public GameObject wordResetFX;

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
    public GameObject ONE;
    public GameObject TWO;
    public GameObject THREE;
    public GameObject FOUR;
    public GameObject FIVE;
    public GameObject SIX;
    public GameObject SEVEN;
    public GameObject EIGHT;
    public GameObject NINE;
    public GameObject ZERO;
    public GameObject PERCENT;

    public static Dictionary<char, GameObject> letterObjects = new Dictionary<char, GameObject>();
    public static Dictionary<string, char> keyboardMap = new Dictionary<string, char>();
    public static List<GameObject> p1_spawnedLetters = new List<GameObject>();
    public static List<GameObject> p2_spawnedLetters = new List<GameObject>();
    public static List<string> englishWordsHard;
    public static List<string> englishWordsEasy;

    public bool p1_word_set = false;
    public bool p2_word_set = false;
    private bool victorySoundPlayed = false;
    // -------------------------------------------------------------------
    // TODO -> 
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

        letterObjects['1'] = ONE;
        letterObjects['2'] = TWO;
        letterObjects['3'] = THREE;
        letterObjects['4'] = FOUR;
        letterObjects['5'] = FIVE;
        letterObjects['6'] = SIX;
        letterObjects['7'] = SEVEN;
        letterObjects['8'] = EIGHT;
        letterObjects['9'] = NINE;
        letterObjects['0'] = ZERO;
        letterObjects['%'] = PERCENT;

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
        // https://gib.com/dwyl/english-words
        TextAsset file = Resources.Load<TextAsset>("words_alpha");

        // put them in the list
        englishWordsHard = new List<string>(
            file.text.Split('\n')
        );

        file = Resources.Load<TextAsset>("google-10000-english");

        englishWordsEasy = new List<string>(
            file.text.Split('\n')
        );

    }
    // -------------------------------------------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p1_word = GetNewWord(true);
        p2_word = GetNewWord(false);
        p1_wordSpawnArea.SetActive(false);
        p2_wordSpawnArea.SetActive(false);
        CenterAndSpawnWord(p1_word, p1_wordSpawnArea, true);
        CenterAndSpawnWord(p2_word, p2_wordSpawnArea, false);
        StartCoroutine(FadeInMusic());
        StartCoroutine(StartGame());
        Invoke(nameof(InitializeDistance), countDownTime);
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        // p1 branch
        if (CurWordIsFinished(true))
        {
            // get rid of old letters
            foreach (GameObject letter in p1_spawnedLetters)
            {
                Destroy(letter);
            }
            p1_spawnedLetters.Clear();
            // pick a new world
            p1_word = GetNewWord(true);
            // spawn letters for new word
   
            CenterAndSpawnWord(p1_word, p1_wordSpawnArea, true);
            PlaceGoldenLetter(true);
            p1_word_set = false;
        }
        // move highlighted material 
        // wordset was wrong 
        // choice for name
        // so ignore
        if (!p1_word_set)
        {
            UpdateWordMaterials(true);
            p1_word_set = true;

        }

        // RemoveGoldenLetter(true);
           
        // p2 branch
        if (CurWordIsFinished(false))
        {
            // get rid of old letters
            foreach (GameObject letter in p2_spawnedLetters)
            {
                Destroy(letter);
            }
            p2_spawnedLetters.Clear();
            // pick a new world
            p2_word = GetNewWord(false);
            // spawn letters for new word
            CenterAndSpawnWord(p2_word, p2_wordSpawnArea, false);
            PlaceGoldenLetter(false);
        }

        if (!p2_word_set)
        {
            UpdateWordMaterials(false);
            p2_word_set = true;
        }

        // RemoveGoldenLetter(false);

        AdjustMusicVolume();

        if ((player_1_winner || player_2_winner) && !victorySoundPlayed)
        {
            victorySoundPlayed = true;
            countDownSource.PlayOneShot(victorySound);
        }
        
        CheckForReset();
        Player2Toggle();
        
    }
    // -------------------------------------------------------------------
    void CheckForReset()
    {
        if (Keyboard.current.leftShiftKey.isPressed && Keyboard.current.rKey.wasPressedThisFrame)
        {
            letterObjects = new Dictionary<char, GameObject>();
            keyboardMap = new Dictionary<string, char>();
            p1_spawnedLetters = new List<GameObject>();
            p2_spawnedLetters = new List<GameObject>();
            englishWordsHard.Clear();
            englishWordsEasy.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    // -------------------------------------------------------------------
    void Player2Toggle()
    {
        if (Keyboard.current.leftShiftKey.isPressed && Keyboard.current.cKey.wasPressedThisFrame)
        {
            KeyInputPlayer2.computer = !KeyInputPlayer2.computer;
            Debug.Log($"Turning computer player to: {KeyInputPlayer2.computer}");
            gameMusicSource.PlayOneShot(computerEngageSound);
        }
    }
    // -------------------------------------------------------------------
    public string GetNewWord(bool p1)
    {
        if (p1)
            p1_curIndex = 0;
        else
            p2_curIndex = 0;

        float hardChance;
        List<string> wordList;
        string player = p1 ? "player 1" : "player 2";
        string word = "";
        // cybernetic system 2
        bool wordFound = false;

        while (!wordFound)
        {
            // losing get easy word
            if (p1 && P2InTheLead())
            {
                hardChance = 0.2f;
            }
            // losing get easy word
            else if (!p1 && P1InTheLead())
            {
                hardChance = 0.2f;
            }
            // im in the lead, get harder word
            else
            {
                hardChance = 0.8f;
            }
            // hard v easy word assignment
            if (Random.value < hardChance)
            {
                wordList = englishWordsHard;
                Debug.Log($"hard word assigned for {player}");
            }
            else
            {
                wordList = englishWordsEasy;
                Debug.Log($"easy word assigned for {player}");
            }
            int index = Random.Range(0, wordList.Count);
            word = wordList[index].ToUpper().Trim();
            // word length check
            if (word.Length > 2 && word.Length < 11)
            {
                wordFound = true;
            }
        }
        return word;
    }
    // -------------------------------------------------------------------
    public bool CurWordIsFinished(bool p1)
    {
        if (p1)
            return p1_curIndex >= p1_word.Length;
        else
            return p2_curIndex >= p2_word.Length;
    }
    // -------------------------------------------------------------------
    // Formula: Gross WPM = (Total Characters ÷ 5) ÷ Time in Minutes
    public float GetPlayerWMP(string player)
    {
        switch (player)
        {
            case "player1":
                return player_1_correct_cnt / 5 / (Time.time / 60);
            case "player2":
                return player_2_correct_cnt / 5 / (Time.time / 60);
            default:
                return 0f;
        }
    }
    // -------------------------------------------------------------------
    public void UpdateWordMaterials(bool p1)
    {
        string word = p1 ? p1_word : p2_word;

        List<GameObject> spawnedLetters = 
            p1 ? 
            p1_spawnedLetters : 
            p2_spawnedLetters;

        if (word.Length < 1) return;
        if (spawnedLetters.Count < 1) return;

        for (int i = 0; i < word.Length; i++)
        {
            GameObject curLetterObj = spawnedLetters[i];
            LetterBehavior lb = curLetterObj.GetComponent<LetterBehavior>();

            if (p1 && p1_goldenLetterIndex == p1_curIndex &&lb.p1_goldenLetter)
            {
                lb.currentlySelected = true;
            }


            if (p1 && lb.p1_goldenLetter)
            {
                continue;
            } 
            if (!p1 && lb.p2_goldenLetter)
            {
                continue;
            } 
            // highlight the current letter
            if (p1 && p1_curIndex == i)
            {
                lb.SetLetterMaterial(player_1_material);
                lb.currentlySelected = true;
            } 
            // highlight the current letter
            else if (!p1 && p2_curIndex == i)
            {
                // change these to some diff color for p2 
                lb.SetLetterMaterial(player_2_material);
                lb.currentlySelected = true;
            }
            else 
            {
                Material basic = p1 ? p1_Material : p2_Material;
                lb.SetLetterMaterial(basic);
            }
        }
    }
    // -------------------------------------------------------------------
    public void CenterAndSpawnWord(

            string currentWord,
            GameObject spawnArea,
            bool p1,
            float distanceBetweenLetters = 1.5f,
            List<GameObject> otherList = null,
            bool mainWord = true
        )
    {
        List<GameObject> spawnedLetters = p1 ? p1_spawnedLetters : p2_spawnedLetters;

        MeshCollider mc = spawnArea.GetComponent<MeshCollider>();
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

        for (int i = 0; i < centerLetterIndex; i++)
        {
            char letter = currentWord[i];
            GameObject letterObj = Instantiate(
                // letter gameobj
                letterObjects[letter],
                // start far left then scooch right
                spawnArea.transform.position + new Vector3(farLeft + nxtPosition, 0, 0),
                // set rotation
                Quaternion.Euler(0, 180, 0)
            );
            // give parent to letter
            letterObj.transform.SetParent(spawnArea.transform, true);
            nxtPosition += distanceBetweenLetters;
            // prevent tipping over
            Rigidbody rb = letterObj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.constraints = RigidbodyConstraints.FreezeAll;
            // keep track of these letter objects
            if (mainWord)
                spawnedLetters.Add(letterObj);
            else
                otherList?.Add(letterObj);
        }

        nxtPosition = 0;
        // fix weird spacing on odd lengthed words
        if (lenWord % 2 != 0) xCenter -= distanceBetweenLetters;
        // then spawn to the right of center
        for (int i = centerLetterIndex; i < lenWord; i++)
        {
            char letter = currentWord[i];
            GameObject letterObj = Instantiate(
                // letter gameobj
                letterObjects[letter],
                // start far left then scooch right
                spawnArea.transform.position + new Vector3(xCenter + nxtPosition, 0, 0),
                // set rotation
                Quaternion.Euler(0, 180, 0)
            );
            // give parent to letter
            letterObj.transform.SetParent(spawnArea.transform, true);
            nxtPosition += distanceBetweenLetters;
            // prevent tipping over
            Rigidbody rb = letterObj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.constraints = RigidbodyConstraints.FreezeAll;
            // keep track of these letter objects
            if (mainWord)
                spawnedLetters.Add(letterObj);
            else
                otherList?.Add(letterObj);
        }
    }
    // -------------------------------------------------------------------
    public float GetPlatformWeight(SpringJoint spring)
    {
        Vector3 force = spring.currentForce;
        float downForce = Mathf.Abs(force.y);
        return force.y;
    }
    // -------------------------------------------------------------------
    public void PlaceGoldenLetter(bool p1)
    {   
        // will set up some cybernetic stuff here
        // greater chance of oglden 
        float goldenChance;

        if (p1 && P1InTheLead())
        {
            goldenChance = 0f;
        }
        else if (!p1 && P2InTheLead())
        {
            goldenChance = 0f;
        }
        else
        {
            goldenChance = 1f;
        }

        if (Random.value >= goldenChance)
        {
            if (p1) p1_goldenLetterIndex  = -1;
            if (!p1) p2_goldenLetterIndex = -1;
            return;
        }
    
        List<GameObject> spawnedLetters = p1 ? p1_spawnedLetters : p2_spawnedLetters;
        string cur_word = p1 ? p1_word : p2_word;
        int rnd_inx = Mathf.FloorToInt(Random.Range(0, spawnedLetters.Count));
        rnd_inx = Mathf.FloorToInt(cur_word.Length / 2);
        GameObject goldenLetter = spawnedLetters[rnd_inx];
        LetterBehavior lb = goldenLetter.GetComponent<LetterBehavior>();

        if (p1)
        {
            p1_goldenLetterIndex = rnd_inx;
            lb.p1_goldenLetter = true;
        }
            
        else
        {
            p2_goldenLetterIndex = rnd_inx;
            lb.p2_goldenLetter = true;
        }
            
        lb.SetLetterScale(goldenLetterScaler);
        lb.SetLetterMaterial(GoldenLetterMat);
    }
    // -------------------------------------------------------------------
    public void ApplyUpwardForce(bool p1)
    {

        List<GameObject> letters = p1 ? player_2_platformLetters : player_1_platformLetters;

        int screamIndex = 0;

        foreach(GameObject letter in letters)
        {
            letter.layer = LayerMask.NameToLayer("Letter");
            LetterBehavior lb = letter.GetComponent<LetterBehavior>();
            AudioClip s = screams[screamIndex ++ % screams.Length];
            lb.audioSource.PlayOneShot(s);

            Rigidbody r = letter.GetComponent<Rigidbody>();
            r.AddForce(Vector3.up * comboForce, ForceMode.Impulse);
        }

        Rigidbody rb;
        if (p1)
        {
            rb = player_2_platform.GetComponent<Rigidbody>();
        }
        else
        {
            rb = player_1_platform.GetComponent<Rigidbody>();
        }

        rb.AddForce(Vector3.up * comboForce, ForceMode.Impulse);


    }
    // -------------------------------------------------------------------
    public void JumbleLetters(bool p1)
    {
        List<GameObject> letters = p1 ? p2_spawnedLetters : p1_spawnedLetters;
        foreach (GameObject letter in letters)
        {
            float speed = Random.value * 0.5f;
            LetterBehavior lb = letter.GetComponent<LetterBehavior>();
            StartCoroutine(lb.CursorRotate(speed));
            StartCoroutine(lb.CursorScale(2f, speed));
        }
    }
    // -------------------------------------------------------------------
    public IEnumerator StartGame()
    {
     
        List<GameObject> countdownLetters = new List<GameObject>();
        // need spawn point for countdown numbers
        for (int i = 0; i < countDownTime; i++)
        {
            CenterAndSpawnWord(
                (countDownTime - i).ToString(), 
                countDownSpawnArea, 
                false,
                1f, 
                countdownLetters, 
                false
            );
            countDownSource.Play();
            countdownLetters[0].GetComponent<LetterBehavior>().isCountdown = true;
            yield return new WaitForSeconds(1f);
            Destroy(countdownLetters[0]);
            countdownLetters.Clear();

        }

        countDownSpawnArea.transform.position = 
            new Vector3(
                1.94f, 
                countDownSpawnArea.transform.position.y, 
                countDownSpawnArea.transform.position.z
            );

        CenterAndSpawnWord("GO", countDownSpawnArea, false, 1.25f, countdownLetters, false);
        countDownSource.clip = beep_1;
        countDownSource.Play();

        foreach (GameObject letter in countdownLetters)
        {
            letter.GetComponentInChildren<Renderer>().material = basicLetterMat;

        }
        yield return new WaitForSeconds(0.1f);

        foreach (GameObject letter in countdownLetters) Destroy(letter);
        countdownLetters.Clear();
        gameStarted = true;
        p1_wordSpawnArea.SetActive(true);
        p2_wordSpawnArea.SetActive(true);
        ambientAudioSource.Play();
    }
    // -------------------------------------------------------------------
    public IEnumerator FadeInMusic()
    {
        float timeStarted = Time.time;

        gameMusicSource.clip = calmMusic;
        gameMusicSource.volume = 0;
        gameMusicSource.Play();

        while (Time.time - timeStarted < musicFadeInTime)
        {
            float t = (Time.time - timeStarted) / musicFadeInTime;
            gameMusicSource.volume = Mathf.Lerp(
                0f,
                0.5f,
                t
            );
            yield return null;
        }

        // Debug.Log("Playing Music!");
        gameMusicSource.volume = 0.5f;

    }
    // -------------------------------------------------------------------
    public void AdjustMusicVolume()
    {
        float p1_distance = Mathf.Abs(player_1_platform.transform.position.y - finishLine.transform.position.y);
        float p2_distance = Mathf.Abs(player_2_platform.transform.position.y - finishLine.transform.position.y);
        float min_distance = Mathf.Min(p1_distance, p2_distance);
        float currentPercent = Mathf.Clamp01(1 - (min_distance / totalPlatformDistance));
        // Debug.Log($"volume: {currentPercent}");
        // gameMusicSource.volume = currentPercent;
    }
    // -------------------------------------------------------------------
    void InitializeDistance()
    {
        totalPlatformDistance = 
            Mathf.Abs(
                player_1_platform.transform.position.y - finishLine.transform.position.y
            );
    }
    // -------------------------------------------------------------------
    public bool P1InTheLead()
    {
        return p1_percent_complete > p2_percent_complete;
    }
    // -------------------------------------------------------------------
    public bool P2InTheLead()
    {
        return p1_percent_complete < p2_percent_complete;
    }
    // -------------------------------------------------------------------
}
