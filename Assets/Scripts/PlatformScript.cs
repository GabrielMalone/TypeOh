using UnityEngine;

public class PlatformScript : MonoBehaviour
{

    [Header("Sound FX Settings")]
    public AudioSource audioSource;
    public AudioClip clankSound;
    public float maxImpact = 100f;

    [Header("Boundary Settings")]
    public GameObject boundaryBox;
    private LineRenderer lineRenderer;
    private BoxCollider boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = boundaryBox.GetComponent<LineRenderer>();
        boxCollider = boundaryBox.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //DrawWireframe();
    }
    void OnCollisionEnter(Collision collision)
    {
        ClankSfxBasedOnPhysics(collision);
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            Debug.Log($"{gameObject.tag} WINS!");
        }
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

    void DrawWireframe()
    {
        Vector3 center = boxCollider.center;
        Vector3 extents = boxCollider.size * 0.5f;



    }

}
