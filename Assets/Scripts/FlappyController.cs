using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class FlappyController : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private BoxCollider2D m_Collider;

    public GameManager m_GameManager;
    public float p_FlapForce = 10.0f;
    public float p_MaxVelocityY = 5.0f;

    public float p_MaxYPos = 1.0f;
    private bool e_Flapping = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            e_Flapping = true;
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {
        if (e_Flapping)
        {
            m_Rigidbody.AddForce(new Vector2(0.0f, p_FlapForce), ForceMode2D.Impulse);
            e_Flapping = false;
        }

        if (m_Rigidbody.velocity.y > p_MaxVelocityY)
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, p_MaxVelocityY);
        }
    }

    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "Ground")
        {
            m_GameManager.GameOver();
        }
    }

    // OnTriggerEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Score")
        {
            m_GameManager.AddScore();
        }
    }

    public void Restart()
    {
        m_Rigidbody.velocity = Vector2.zero;
        transform.position = new Vector2(0.0f, 0.0f);
    }

}
