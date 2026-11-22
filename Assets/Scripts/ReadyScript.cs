using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadyScript : MonoBehaviour
{
    public bool p1;
    public List<GameObject> p1_letters;
    public List<GameObject> p2_letters;
    public GameStateManager GSM;
    public Material p1_readymat;
    public Material p2_readymat;
    public AudioClip[] yays;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GSM.CenterAndSpawnWord(
            p1 ? "P1 (R)EADY ?" : "P2 (R)EADY ?", 
            gameObject, 
            false, 
            1.5f, 
            p1 ? p1_letters : p2_letters, 
            false
        );

        List<GameObject> letters = p1 ? p1_letters : p2_letters;

        foreach (GameObject letter in letters)
        {
            LetterBehavior lb = letter.GetComponent<LetterBehavior>();
            lb.SetLetterMaterial(p1 ? p1_readymat : p2_readymat);
            lb.isPercent = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForReadyClick();
    }

    void CheckForReadyClick()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SetLettersReady(true);
            GSM.p1Ready = true;
        }
        if (Keyboard.current.numpad8Key.wasPressedThisFrame)
        {
            SetLettersReady(false);
            GSM.p2Ready = true;
        }
        if (GSM.p1Ready && GSM.p2Ready)
        {
            
            List<GameObject> letters = p1 ? p1_letters : p2_letters;


            for(int i = 0 ; i < letters.Count ; i ++)
            {
                Destroy(letters[i], 3f);
            }
        }
    }


    void SetLettersReady(bool p1)
    {

        float f = 3f;

        List<GameObject> letters = p1 ? p1_letters : p2_letters;

        foreach(GameObject letter in letters)
        {
            Rigidbody rb = letter.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(new Vector3(f * Random.value, f * Random.value, f * Random.value), ForceMode.Impulse);
            rb.AddTorque(new Vector3(f * Random.value, f * Random.value, f * Random.value), ForceMode.Impulse);
            LetterBehavior lb = letter.GetComponent<LetterBehavior>();
            AudioClip s = yays[(int)(Random.value * yays.Length)];
            lb.audioSource.PlayOneShot(s);
        }
    }

}
