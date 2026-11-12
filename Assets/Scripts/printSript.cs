using System.Collections.Generic;
using UnityEngine;

public class printSript : MonoBehaviour
{
    [Header("Print Settings")]
    public GameObject spawnPoint;
    public GameObject GameStateManager;
    public GameObject finishLine;
    public GameObject platform;
    public bool player_1 = false;
    public Material p1_percent_mat;
    public Material p2_percent_mat;
    private Material material;
    private GameStateManager GSM;
    private float currentPercent = 0;
    private float totalDistance;
    public List<GameObject> statLetters = new List<GameObject>();
    private Collider platformCollider;
    private Collider finishCollider;
    private float prevDifference;
    private bool distanceSet = false;
    private bool needToLoadPercent = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GSM = GameStateManager.GetComponent<GameStateManager>();
        // let's delay this like 3 seconds since platforms needs to settle. 
        Invoke(nameof(InitializeDistance), 3f);
        material = player_1 ? p1_percent_mat : p2_percent_mat;
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (!GSM.gameStarted) return;
        if (distanceSet)
        {
            UpdatePercentDone();
        }
    }
    // -------------------------------------------------------------------
    void UpdatePercentDone()
    {
        float current_difference = Mathf.Abs(finishLine.transform.position.y - platform.transform.position.y);
        currentPercent = Mathf.Clamp01(1 - (current_difference / totalDistance)) * 100f;
        bool differenceChanged = Mathf.Abs(current_difference - prevDifference) >= 0.2;
        // Debug.Log($"current distnace: {current_difference} | prev distance : {prevDifference} | diff: {current_difference - prevDifference}");

        if (differenceChanged || needToLoadPercent)
        {   
            needToLoadPercent = false;
            ClearOldStats();
            int floorPercent = Mathf.RoundToInt(currentPercent);
            GSM.CenterAndSpawnWord(floorPercent.ToString() + "%", gameObject, true, statLetters, false);
            float rotation = player_1 ? -65f : 65f;
            gameObject.transform.Rotate(0f, rotation, 0f);
            SetStatMaterial();
            foreach(GameObject letter in statLetters)
            {
                letter.GetComponent<LetterBehavior>().isPercent = true;
            }
            prevDifference = current_difference;
        }
        if (player_1)
        {
            GSM.p1_percent_complete = currentPercent;
            // Debug.Log($"current percent complete p1: {GSM.p1_percent_complete}");
            if (currentPercent >= 99.9f)
            {
                ClearOldStats();
                GSM.CenterAndSpawnWord("100%", gameObject, true, statLetters, false);
                float rotation = player_1 ? -65f : 65f;
                gameObject.transform.Rotate(0f, rotation, 0f);
                SetStatMaterial();
                foreach (GameObject letter in statLetters)
                {
                    letter.GetComponent<LetterBehavior>().isPercent = true;
                }
                prevDifference = current_difference;
                GSM.player_1_winner = true;
                GSM.endGame = true;
                GSM.gameStarted = false;
            }
        }
        else
        {
            GSM.p2_percent_complete = currentPercent;
            // Debug.Log($"current percent complete p2: {GSM.p2_percent_complete}");
            if (currentPercent >= 99.9f)
            {
                ClearOldStats();
                GSM.CenterAndSpawnWord("100%", gameObject, true, statLetters, false);
                float rotation = player_1 ? -65f : 65f;
                gameObject.transform.Rotate(0f, rotation, 0f);
                SetStatMaterial();
                foreach (GameObject letter in statLetters)
                {
                    letter.GetComponent<LetterBehavior>().isPercent = true;
                }
                prevDifference = current_difference;
                GSM.player_2_winner = true;
                GSM.endGame = true;
                GSM.gameStarted = false;
            }
        }
    }
    // -------------------------------------------------------------------
    void ClearOldStats()
    {
        foreach(GameObject letter in statLetters)
        {
            Rigidbody rb = letter.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            Destroy(letter);
        }

        statLetters.Clear();
    }
    // -------------------------------------------------------------------
    void SetStatMaterial()
    {
        foreach (GameObject letter in statLetters)
        {
            Renderer renderer = letter.GetComponentInChildren<Renderer>();
            renderer.material = material;
        }
    }
    // -------------------------------------------------------------------
    void InitializeDistance()
    {
        platformCollider = platform.GetComponent<Collider>();
        finishCollider = finishLine.GetComponent<Collider>();
        totalDistance = Mathf.Abs(platformCollider.transform.position.y - finishCollider.transform.position.y);
        GSM.totalPlatformDistance = totalDistance;
        prevDifference = totalDistance;
        distanceSet = true;
    }
}
