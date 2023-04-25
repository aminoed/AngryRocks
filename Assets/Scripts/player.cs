using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject rockPrefab;
    public float autoInterval;
    // public float autoSpeed;
    private GameObject rock = null;
    public Vector2 mousePos;
    public Vector2 cursorPos;
    public Vector2 direction;
    public Animator anim;
    public bool isAuto;
    private SpriteRenderer sprite;
    protected float flipY;
    protected float flipX;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        flipY = transform.localScale.y;
        flipX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.y < transform.position.y)
        {
            transform.localScale = new Vector2(flipX, flipY);
        }
        if (isAuto)
        {
            ShootAutomatically();
        }
        else
        {
            ShootManually();
        }
    }

    private void ShootManually()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.up = direction;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.Play("cursor_grab");
            rock = ObjectPool.Instance.GetObject(rockPrefab);
            rock.transform.position = transform.position;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.Play("cursor_release");
            rock.GetComponent<Rock>().SetSpeed(direction);
        }
    }

    private void ShootAutomatically()
    {
        transform.RotateAround(new Vector3(0, 0.45f, 0), new Vector3(0f, 0f, 1f), 1f);
        // transform.Rotate(Vector3.forward * autoSpeed);
        if (autoInterval > 0)
        {
            autoInterval -= Time.deltaTime;
        }
        else
        {
            Vector2[] directions = new Vector2[4];

            directions[0] = (transform.rotation * Vector2.up).normalized;
            directions[1] = (transform.rotation * Vector2.down).normalized;
            directions[2] = (transform.rotation * Vector2.left).normalized;
            directions[3] = (transform.rotation * Vector2.right).normalized;

            for (int i = 0; i < 4; i++)
            {
                rock = ObjectPool.Instance.GetObject(rockPrefab);
                rock.transform.position = transform.position;

                rock.GetComponent<Rock>().SetSpeed(directions[i]);
            }
            autoInterval = 0.1f;
        }
    }
}
