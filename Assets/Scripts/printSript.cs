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
    private float updateTime = 0.5f;
    private float prevTime = 0f;
    private float totalDistance;
    public List<GameObject> statLetters = new List<GameObject>();
    private Collider platformCollider;
    private Collider finishCollider;
    private float prevDistance;
    private bool distanceSet = false;
    private bool percentLoaded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GSM = GameStateManager.GetComponent<GameStateManager>();
        prevTime = Time.time;
        // let's delay this like 3 seconds since platforms needs to settle. 
        Invoke(nameof(InitializeDistance), 3f);
        material = player_1 ? p1_percent_mat : p2_percent_mat;
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (GSM.gameStarted && !percentLoaded)
        {
            percentLoaded = true;
            UpdatePercentDone(true);
        }
        if (distanceSet)
            UpdatePercentDone();
        
    }
    // -------------------------------------------------------------------
    void UpdatePercentDone(bool firstTimeRunning = false)
    {
        float current_distance = Mathf.Abs(finishLine.transform.position.y - platform.transform.position.y);
        currentPercent = Mathf.Clamp01(1 - (current_distance / totalDistance)) * 100f;

        if (player_1)
        {
            GSM.p1_percent_complete = currentPercent;
        }
        else
        {
            GSM.p2_percent_complete = currentPercent;
        }

        bool distanceChanged = Mathf.Abs(current_distance - prevDistance) >= 0.01;

        if (firstTimeRunning || (distanceChanged && Time.time - prevTime > updateTime))
        {
            ClearOldStats();
            prevTime = Time.time;
            int floorPercent = Mathf.RoundToInt(currentPercent);
            //Debug.Log($"current percent: {floorPercent}");
            GSM.CenterAndSpawnWord(floorPercent.ToString() + "%", gameObject, true, statLetters, false);
            float rotation = player_1 ? -65f : 65f;
            gameObject.transform.Rotate(0f, rotation, 0f);
            SetStatMaterial();
            foreach(GameObject letter in statLetters)
            {
                letter.GetComponent<LetterBehavior>().isPercent = true;
            }
        }

        prevDistance = current_distance;

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
        prevDistance = totalDistance;
        distanceSet = true;
    }
}
