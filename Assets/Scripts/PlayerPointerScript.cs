using Unity.VisualScripting;
using UnityEngine;

public class PlayerPointerScript : MonoBehaviour
{
    [Header("Settings")]
    public bool player_1 = false;
    public GameObject gameStateManager;
    private GameStateManager gsm;
    private float offsetX;
    private float offsetY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gsm = gameStateManager.GetComponent<GameStateManager>();
        offsetX = player_1 ? 0.15f : 0.15f;
        offsetY = player_1 ? -1.5f : 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentPlayerLetterObject();
    }


    void GetCurrentPlayerLetterObject()
    {
        int curIndex = player_1 ? gsm.p1_curIndex :  gsm.p2_curIndex;

        if (curIndex > GameStateManager.spawnedLetters.Count - 1) return;

        GameObject curLetterObj = GameStateManager.spawnedLetters[curIndex];

        if(curLetterObj == null) return;

        gameObject.transform.position = new Vector3(
            curLetterObj.transform.position.x - offsetX,
            curLetterObj.transform.position.y + offsetY,
            curLetterObj.transform.position.z
        );
    }
    


}
