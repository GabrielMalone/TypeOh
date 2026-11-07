using UnityEngine;

public class PlatformScript : MonoBehaviour
{

    [Header("Sound FX Settings")]
    public AudioSource audioSource;
    public AudioClip clankSound;
    public float maxImpact = 100f;

    [Header("Boundary Settings")]
    public GameObject boundaryBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //DrawWireframe();
    }
    void ClankSfxBasedOnPhysics(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            float speed = collision.relativeVelocity.magnitude;
         
            // change impact to a percent and normalize it with cap of 1
            float volume = Mathf.Clamp01(speed / maxImpact);
            //audioSource.PlayOneShot(clankSound, volume);
        }
    }
}
