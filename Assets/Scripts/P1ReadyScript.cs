using UnityEngine;
using UnityEngine.InputSystem;

public class P1ReadyScript : MonoBehaviour
{

    public GameObject[] letters;
    public GameStateManager GSM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForReadyClick();
    }

    void CheckForReadyClick()
    {
  
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("cliking");
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider is BoxCollider)
                {
                    Debug.Log($"Clicked a BoxCollider! {gameObject.name}");
                    SetLettersReady();
                    GSM.p1Ready = true;
                    GSM.p2Ready = true;
                }
            }
        }
        
    }


    void SetLettersReady()
    {
        foreach(GameObject letter in letters)
        {
            Rigidbody rb = letter.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            LetterBehavior lb = letter.GetComponent<LetterBehavior>();
            AudioClip s = GSM.screams[(int)(Random.value * GSM.screams.Length)];
            lb.audioSource.PlayOneShot(GSM.chuckle);
        }
    }


}
