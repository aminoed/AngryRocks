using UnityEngine;

public class Rock : MonoBehaviour
{
    public float rockSpeed;
    public GameObject explosionPrefab;
    private Rigidbody2D rockRb;
    private int hitNum = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        rockRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpeed(Vector2 direction)
    {
        rockRb.velocity = direction * rockSpeed;
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Boarder") || other.CompareTag("Player"))
        {
            // isHit = true;
            hitNum++;
            GameObject explosion = ObjectPool.Instance.GetObject(explosionPrefab);
            explosion.transform.position = transform.position;
            ObjectPool.Instance.PushObject(gameObject);
            if (other.CompareTag("Bird"))
            {
                AudioController.instance.BirdAudio();
            }
            else
            {
                AudioController.instance.DiamondAudio();
            }

        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Boarder"))
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
