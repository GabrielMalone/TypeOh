using System.Collections.Generic;
using UnityEngine;

public class printSript : MonoBehaviour
{
    [Header("Print Settings")]
    public GameObject spawnPoint;
    public GameObject GameStateManager;
    public bool player_1 = false;
    private GameStateManager GSM;
    private float currentPercent = 0;
    private float updateTime = 0.2f;
    private float prevTime = 0f;
    private SpringJoint spring;
    public List<GameObject> statLetters = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GSM = GameStateManager.GetComponent<GameStateManager>();
        prevTime = Time.time;
        spring =  player_1 ?  GSM.player_1_spring : GSM.player_2_spring;
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (Time.time - prevTime > updateTime)
        {
            ClearOldStats();
            prevTime = Time.time;
            currentPercent = GSM.GetPlatformWeight(spring) / GSM.maxPlatformForce * 100;
            int floorPercent = Mathf.FloorToInt(currentPercent);
            Debug.Log($"current percent: {floorPercent}");
            GSM.CenterAndSpawnWord(floorPercent.ToString() + "%", gameObject, statLetters, false);
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
}
