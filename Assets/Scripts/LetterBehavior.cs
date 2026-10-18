
using UnityEngine;



public class LetterBehavior : MonoBehaviour

{
    [Header("Time to Live")]
    public float timeToLive = 3.0f;

    [Header("Material Settings")]
    public Material player_1_correctMaterial;
    public Material player_1_incorrectMaterial;
    public Material player_1_currentLetter;
    public Material player_1_completedLetter;
    public Material player_2_correctMaterial;
    public Material player_2_incorrectMaterial;
    public Material player_2_currentLetter;
    public Material player_2_completedLetter;

    public Material NextLetterMaterial;
    public Material CurLetterMaterial;
    public Material FinishedLetterMaterial;

    [Header("Letter Size")]
    public float letterScale = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLetterSize();
    }


    void UpdateLetterSize()
    {
        gameObject.transform.localScale = Vector3.one * letterScale;
    }

}
