using Unity.VisualScripting.FullSerializer;
using UnityEditor.Rendering;
using UnityEngine;

public class moon : MonoBehaviour
{
    public float spintRate = 25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * -spintRate * Time.deltaTime);
        transform.Rotate(new Vector3(0,0,0.5f) * spintRate * Time.deltaTime); 
    }
}


