using UnityEngine;

public class finishLineScript : MonoBehaviour
{
    public GameObject stateManager;
    private GameStateManager GSM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // -------------------------------------------------------------------
    void Start()
    {
        GSM = stateManager.GetComponent<GameStateManager>();
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {

    }
    // -------------------------------------------------------------------
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log($"Finish Line Crossed: {collision.gameObject.name}");

        if (collision.gameObject.name == "p1_collector")
        {
            GSM.player_1_winner = true;
            GSM.endGame = true;
        }
        if (collision.gameObject.name == "p2_collector")
        {
            GSM.player_2_winner = true;
            GSM.endGame = true;
        }
    }
    // ------------------------------------------------------------------- 
}
