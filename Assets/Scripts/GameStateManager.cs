using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Header("Game State Settings")]
    public string currentWord = "";
    public int curIndex = 0;
    public int player_1_position = 0;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CurWordIsFinished())
        {
            GetNewWord();
            Debug.Log("FINISHED WORD!");
        }
    }

    public void GetNewWord()
    {
        // for now just reset
        curIndex = 0;
    }

    public bool CurWordIsFinished()
    {
        return curIndex >= currentWord.Length;
    }
}
