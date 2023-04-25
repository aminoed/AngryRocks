using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Red : MonoBehaviour
{
    public Animator enemyAnim;
    public Rigidbody2D birdRb;
    public int life = 1;
    public GameObject enemyGen;
    public float enemySpeed;
    private Vector2 targetPosition;
    public int score = 0;
    public Vector2 direction;
    private float flipX;
    
    bool isLowTU;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        birdRb = GetComponent<Rigidbody2D>();
        enemyGen = GameObject.Find("EnemyGenerator");
    }

    // Start is called before the first frame update
    void Start()
    {
        isLowTU = enemyGen.GetComponent<GenerateEnemies>().isLowTU;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpeed()
    {
        bool isCenter;
        if (this.gameObject.tag == "Red" || this.gameObject.tag == "Green" || this.gameObject.tag == "Blue" || isLowTU)
        {
            isCenter = true;
        }
        else
        {
            isCenter = Random.Range(0, 1f) < 0.6 ? true : false;
        }
        if (isCenter)
        {
            targetPosition = new Vector2(0, 0.45f);
        }
        else
        {
            Quaternion targetQuaternion = Quaternion.Euler(0, 0, Random.Range(45.0f, 150.0f));
            // Debug.Log("=======angle: "+ targetQuaternion.eulerAngles);
            Vector2 dir = (Vector2)transform.position - new Vector2(0, 0.45f);
            targetPosition = targetQuaternion * dir;
            // Debug.Log("=======targetPosition: "+ targetPosition);
        }
        direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.right = direction.normalized;
        birdRb.velocity = direction * enemySpeed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("=======Trigger name:" + other.gameObject.name);

        if (other.CompareTag("Bullet"))
        {
            life--;
            score = getScore(this.gameObject.tag);
            enemyGen.GetComponent<GenerateEnemies>().RemoveFromList(gameObject, true);
            ObjectPool.Instance.PushObject(gameObject);

        }
        if (other.CompareTag("Player"))
        {
            enemyGen.GetComponent<GenerateEnemies>().RemoveFromList(gameObject, false);
            ObjectPool.Instance.PushObject(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Boarder"))
        {
            enemyGen.GetComponent<GenerateEnemies>().RemoveFromList(gameObject, false);
            ObjectPool.Instance.PushObject(gameObject);
        }
    }

    private int getScore(string tag)
    {
        switch (tag)
        {
            case "Bird":
                return 5;
            case "Green":
                return 10;
            case "Red":
                return 20;
            case "Blue":
                return 30;
            default:
                return 0;
        }
    }

}
