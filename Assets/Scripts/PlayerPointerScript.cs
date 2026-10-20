using Unity.VisualScripting;
using UnityEngine;

public class PlayerPointerScript : MonoBehaviour
{
    [Header("Settings")]
    public GameObject gameStateManager;
    private GameStateManager gsm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gsm = gameStateManager.GetComponent<GameStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentPlayerLetterObject();
    }


    void GetCurrentPlayerLetterObject()
    {
        int curIndex = gsm.curIndex;
        GameObject curLetterObj = GameStateManager.spawnedLetters[curIndex];

        if(curLetterObj == null) return;

        gameObject.transform.position = new Vector3(
            curLetterObj.transform.position.x,
            curLetterObj.transform.position.y + 1.5f,
            curLetterObj.transform.position.z
        );
    }
    


}
