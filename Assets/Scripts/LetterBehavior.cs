using System.Collections;
using System.Numerics;
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
    public AudioSource audioSource;
    public AudioClip collisionNoise;
    private bool collisionPlayed = false;
    private Renderer renderer;
    private bool blinking = false;
    public bool currentlySelected = false;
    // -------------------------------------------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Destroy(gameObject, timeToLive);
       // gameObject.transform.localScale = Vector3.one * letterScale;
       renderer = gameObject.GetComponentInChildren<Renderer>();
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
       if (currentlySelected && !blinking)
        {
            StartCoroutine(CursorBlink());
        }
    }
    // -------------------------------------------------------------------
    public IEnumerator CursorBlink()
    {
        blinking = true;
        Material material = renderer.material;
        Color ogEmission = material.GetColor("_EmissiveColor");
        
        float blinkTime = 0.2f;
        float startTime = Time.time;
        float intensity;

        while (Time.time - startTime < blinkTime)
        {
            float t = (Time.time - startTime) / blinkTime;
            intensity = Mathf.Lerp(
                1,
                0,
                t
            );
            material.SetColor("_EmissiveColor", ogEmission * intensity);
            yield return null;
        }
    
        material.SetColor("_EmissiveColor", ogEmission * 0);

        startTime = Time.time;

        while (Time.time - startTime < blinkTime)
        {
            float t = (Time.time - startTime) / blinkTime;
            intensity = Mathf.Lerp(
                0,
                1,
                t
            );
            material.SetColor("_EmissiveColor", ogEmission * intensity);
            yield return null;
        }

        material.SetColor("_EmissiveColor", ogEmission);
        blinking = false;
    }
    // -------------------------------------------------------------------
    public void SetLetterScale(float scale)
    {
        gameObject.transform.localScale = UnityEngine.Vector3.one * scale;
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
    public void TypedFX()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce((Random.value > 0.5 ? UnityEngine.Vector3.up : UnityEngine.Vector3.down) * 10, ForceMode.Impulse);
        rb.AddTorque((Random.value > 0.5 ? UnityEngine.Vector3.up : UnityEngine.Vector3.down) * 10, ForceMode.Impulse);
    }
    // -------------------------------------------------------------------
    void OnCollisionEnter(Collision collision)
    {
        if(collisionPlayed) return;
        collisionPlayed = true;

        // letter or platform
        // if (collision.gameObject.layer != 3 || collision.gameObject.layer != 7) return;
        if (!collision.rigidbody) return;

        float speed = collision.relativeVelocity.magnitude;
        float mass = collision.rigidbody.mass;
        float volume = Mathf.Clamp01(speed * mass / 1f);
        
        // Debug.Log($"volume for letters: {volume} | speed: {speed} | mass: {mass} |  maxImpact: {1f}");
        audioSource.PlayOneShot(collisionNoise, volume);
    }

}
