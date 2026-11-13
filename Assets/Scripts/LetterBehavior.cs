using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LetterBehavior : MonoBehaviour

{
    [Header("Time to Live")]
    public float timeToLive = 3.0f;

    [Header("Material Settings")]
    public Material NextLetterMaterial;

    [Header("Letter Size")]
    private float letterScale = 0.75f;

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
    public bool isPercent = false;
    public bool isCountdown = false;
    public bool isOnPlatform = false;
    private UnityEngine.Vector3 lastScale;
    private float rndAdjst;
    public bool comboFX = false;
    // -------------------------------------------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rndAdjst = Random.value;
        Destroy(gameObject, timeToLive);
        gameObject.transform.localScale = Vector3.one * letterScale;
        renderer = gameObject.GetComponentInChildren<Renderer>();
        // if (!scaling && !isPercent && !isOnPlatform && !isCountdown)
        // {
        //     StartCoroutine(CursorScale(1f, 0.5f));
        // }
    }
    // -------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        // if (currentlySelected && !blinking)
        // {
        //     StartCoroutine(CursorBlink());
        // }
        if (currentlySelected && !rotating)
        {
            StartCoroutine(CursorRotate());
        }
        if (p1_goldenLetter || p2_goldenLetter && !blinking)
        {
            StartCoroutine(CursorBlink());
        }
        if (isCountdown && !scaling)
        {
            StartCoroutine(CursorScale(0.5f));
        }
        if (isPercent && !scaling)
        {
            StartCoroutine(CursorScale(0.75f));
        }

    }
    // -------------------------------------------------------------------
    public IEnumerator CursorRotate(float speed = 2f)
    {
        rotating = true;

        float startTime = Time.time;
        float i;

        float start = 0;
        float end = 360;

        if (Random.value > 0.5)
        {
            start = 360;
            end = 0;
        }
        
        UnityEngine.Quaternion ogRotation = transform.rotation;

        while (Time.time - startTime < speed)
        {
            float t = (Time.time - startTime) / speed;
            i = Mathf.Lerp(
                start,
                end,
                t
            );
            transform.rotation = ogRotation * Quaternion.Euler(0f, i, 0f); ;
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
    public IEnumerator CursorScale(float scale, float scaleTime = 2f)
    {
        scaling = true;
        float startTime = Time.time;
        Vector3 ogScale = transform.localScale;

        while (isPercent)
        {
            // take a number that keeps increasing and make it
            // go back and forth between these two numbers
            float t = Mathf.PingPong(Time.time / scaleTime, 1f);
            float i = Mathf.Lerp(1f, scale, t);
            transform.localScale = Vector3.one * i;
            yield return null;
        }

        while (Time.time - startTime < scaleTime)
        {
            // total game time clock for percent
            float elapsed = Time.time - startTime;
            // take a number that keeps increasing and make it
            // go back and forth between these two numbers
            float t = elapsed / scaleTime;
            float i = Mathf.Lerp(1f, scale, t);
            transform.localScale = Vector3.one * i;
            yield return null;
        }

        startTime = Time.time;

        while (Time.time - startTime < scaleTime)
        {
            // total game time clock for percent
            float elapsed = Time.time - startTime;
            // take a number that keeps increasing and make it
            // go back and forth between these two numbers
            float t = elapsed / scaleTime;
            float i = Mathf.Lerp(scale, 1f, t);
            transform.localScale = Vector3.one * i;
            yield return null;
        }

        transform.localScale = ogScale;
        scaling = false;
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
    public void TypedFX()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce((Random.value > 0.5 ? Vector3.up : Vector3.down) * 10, ForceMode.Impulse);
        rb.AddTorque((Random.value > 0.5 ? Vector3.up : Vector3.down) * 10, ForceMode.Impulse);
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
