using System.Collections;
using UnityEngine;

public class LetterBehavior : MonoBehaviour

{
    [Header("Time to Live")]
    public float timeToLive = 3.0f;

    [Header("Material Settings")]
    public Material NextLetterMaterial;

    [Header("Letter Size")]
    public float letterScale = 1f;

    [Header("Special Settings")]
    public bool goldenLetter = false;
    // -------------------------------------------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, timeToLive);
       // gameObject.transform.localScale = Vector3.one * letterScale;
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
       
    }
    // -------------------------------------------------------------------
    public void SetLetterScale(float scale)
    {
        gameObject.transform.localScale = Vector3.one * scale;
    }
    // -------------------------------------------------------------------
    public void SetLetterMaterial(Material material)
    {
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        renderer.material = material;
    }
    // -------------------------------------------------------------------
    public void SetLetterMaass(float mass)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = mass;
    }
    // -------------------------------------------------------------------

}
