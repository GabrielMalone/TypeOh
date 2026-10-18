using UnityEngine;

public class PlatformScript : MonoBehaviour
{

    [Header("Sound FX Settings")]
    public AudioSource audioSource;
    public AudioClip clankSound;
    public float maxImpact = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        ClankSfxBasedOnPhysics(collision);
    }


    void ClankSfxBasedOnPhysics(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            float mass = rb.mass;
            float speed = collision.relativeVelocity.magnitude;
            Vector2 s = collision.impulse;
            float impact = mass * speed;
            // change impact to a percent and normalize it with cap of 1
            float volume = Mathf.Clamp01(impact / maxImpact);
            audioSource.PlayOneShot(clankSound, volume);
        }
    }

}
