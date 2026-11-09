using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{

    [Header("Sound FX Settings")]
    public AudioSource audioSource;
    public AudioClip clankSound;
    public float maxImpact = 10f;

    [Header("Boundary Settings")]
    public GameObject boundaryBox;

    private HashSet<GameObject> lettersCollided = new HashSet<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //DrawWireframe();
    }

    void OnCollisionEnter(Collision collision)
    {
       // ClankSfxBasedOnPhysics(collision);
    }

    void ClankSfxBasedOnPhysics(Collision collision)
    {
        // so we just play the collision sound once
        if (lettersCollided.Contains(collision.gameObject)) return;
        lettersCollided.Add(collision.gameObject);

        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            float speed = collision.relativeVelocity.magnitude;
            float mass = collision.rigidbody.mass;
            // change impact to a percent and normalize it with cap of 1
            float volume = speed * mass / maxImpact;
             // Debug.Log($"volume for letters: {volume} | speed: {speed} | mass: {mass} |  maxImpact: {maxImpact}");
            audioSource.PlayOneShot(clankSound, volume);
        }

    }
}
