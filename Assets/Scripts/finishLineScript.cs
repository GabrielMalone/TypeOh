using UnityEngine;
public class finishLineScript : MonoBehaviour
{
    public GameObject stateManager;
    private GameStateManager GSM;
    private LineRenderer lineRenderer;
    private BoxCollider boxCollider;
    private Vector3 bounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // -------------------------------------------------------------------
    void Start()
    {
        GSM = stateManager.GetComponent<GameStateManager>();
        // lineRenderer = GetComponent<LineRenderer>();
        // lineRenderer.positionCount = 0;
        // boxCollider = GetComponent<BoxCollider>();
        // RenderLine();
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        
    }
    void RenderLine()
    {   
        Bounds bounds = boxCollider.bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        
        AddPoint(new Vector3(min.x, min.y, min.z));
        AddPoint(new Vector3(max.x, min.y, min.z));
        // AddPoint(new Vector3(max.x, max.y, min.z));
        // AddPoint(new Vector3(min.x, min.y, max.z));
        // AddPoint(new Vector3(max.x, min.y, max.z));
        // AddPoint(new Vector3(min.x, max.y, max.z));
        // AddPoint(new Vector3(max.x, max.y, max.z));

        Debug.Log($"bounds: {bounds}");
    }
    // -------------------------------------------------------------------
    void AddPoint(Vector3 newPoint)
    {
        lineRenderer.positionCount ++ ;
        int lastIndex = lineRenderer.positionCount - 1;
        lineRenderer.SetPosition(lastIndex, newPoint);
    }
    // -------------------------------------------------------------------
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log($"Finish Line Crossed: {collision.gameObject.name}");

        // if (collision.gameObject.name == "p1_collector")
        // {
        //     GSM.player_1_winner = true;
        //     GSM.endGame = true;
        //     GSM.gameStarted = false;
        // }
        // if (collision.gameObject.name == "p2_collector")
        // {
        //     GSM.player_2_winner = true;
        //     GSM.endGame = true;
        //     GSM.gameStarted = false;
        // }
    }
    // ------------------------------------------------------------------- 
}
