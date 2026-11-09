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
    private GameStateManager GSM;
    private float currentPercent = 0;
    private float updateTime = 0.2f;
    private float prevTime = 0f;
    private float totalDistance;
    public List<GameObject> statLetters = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GSM = GameStateManager.GetComponent<GameStateManager>();
        prevTime = Time.time;
        // let's delay this like 3 seconds since platforms needs to settle. 
        Invoke(nameof(InitializeDistance), GSM.countDownTime);
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (GSM.gameStarted)
        {
            UpdatePercentDone();
        }
    }
    // -------------------------------------------------------------------
    void UpdatePercentDone()
    {
        if (Time.time - prevTime > updateTime)
        {
            float current_distance = Mathf.Abs(finishLine.transform.position.y - platform.transform.position.y);
            ClearOldStats();
            prevTime = Time.time;
            currentPercent = Mathf.Abs((1 - (current_distance / totalDistance)) * 100);
            int floorPercent = Mathf.FloorToInt(currentPercent);
            //Debug.Log($"current percent: {floorPercent}");
            GSM.CenterAndSpawnWord(floorPercent.ToString() + "%", gameObject, true, statLetters, false);
            float rotation = player_1 ? -65f : 65f;
            gameObject.transform.Rotate(0f, rotation, 0f);
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
    void InitializeDistance()
    {
        totalDistance = Mathf.Abs(platform.transform.position.y - finishLine.transform.position.y);
    }
}
