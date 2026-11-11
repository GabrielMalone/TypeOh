using System.Collections;
using System.Numerics;
using System.Security.Cryptography;
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
    public bool p1_goldenLetter = false;
    public bool p2_goldenLetter = false;
    public AudioSource audioSource;
    public AudioClip collisionNoise;
    private bool collisionPlayed = false;
    private Renderer renderer;
    private bool blinking = false;
    public bool currentlySelected = false;
    private bool rotating = false;
    private bool scaling = false;
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
        if (currentlySelected && !scaling)
        {
            StartCoroutine(CursorScale());
        }
        if (currentlySelected && !rotating)
        {
            StartCoroutine(CursorRotate());
        }
        if (currentlySelected && !blinking)
        {
            StartCoroutine(CursorBlink());
        }
    }
    // -------------------------------------------------------------------
    public IEnumerator CursorRotate()
    {
        rotating = true;

        float rotateTime = 2f;
        float startTime = Time.time;
        float i;

        UnityEngine.Quaternion ogRotation = transform.rotation;

        while (Time.time - startTime < rotateTime)
        {
            float t = (Time.time - startTime) / rotateTime;
            i = Mathf.Lerp(
                0,
                360,
                t
            );
            transform.rotation = ogRotation * UnityEngine.Quaternion.Euler(0f, i, 0f); ;
            yield return null;
        }
        transform.rotation = ogRotation;

        rotating = false;
    }
    // -------------------------------------------------------------------
    public IEnumerator CursorBlink()
    {
        blinking = true;
        Material material = renderer.material;
        Color ogEmission = material.GetColor("_EmissiveColor");

        float blinkTime = 0.2f;
        float startTime = Time.time;
        float i;

        while (Time.time - startTime < blinkTime)
        {
            float t = (Time.time - startTime) / blinkTime;
            i = Mathf.Lerp(
                1,
                0f,
                t
            );
            material.SetColor("_EmissiveColor", ogEmission * i);
            yield return null;
        }
        material.SetColor("_EmissiveColor", ogEmission * 0);
        startTime = Time.time;
        while (Time.time - startTime < blinkTime)
        {
            float t = (Time.time - startTime) / blinkTime;
            i = Mathf.Lerp(
                0f,
                1,
                t
            );
            material.SetColor("_EmissiveColor", ogEmission * i);
            yield return null;
        }
        material.SetColor("_EmissiveColor", ogEmission);
        blinking = false;
    }
    // -------------------------------------------------------------------
    public IEnumerator CursorScale()
    {
        scaling = true;
    
        float scaleTime = 1f;
        float s = 1.25f;
        float startTime = Time.time;
        float i;

        while (Time.time - startTime < scaleTime)
        {
            float t = (Time.time - startTime) / scaleTime;
            i = Mathf.Lerp(
                1,
                0.5f,
                t
            );
            transform.localScale = new UnityEngine.Vector3(i*s, i*s, i*s);
            yield return null;
        }
        startTime = Time.time;
        while (Time.time - startTime < scaleTime)
        {
            float t = (Time.time - startTime) / scaleTime;
            i = Mathf.Lerp(
                0.5f,
                1,
                t
            );
            transform.localScale = new UnityEngine.Vector3(i*s, i*s, i*s);
            yield return null;
        }
        scaling = false;
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
