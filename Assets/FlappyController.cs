using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class FlappyController : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private BoxCollider2D m_Collider;
    public GameObject m_ScoreTextObject;
    public GameObject m_HighScoreTextObject;
    public GameObject m_GameOverTextObject;
    public GameObject m_PlayTextObject;
    public GameObject m_QuitTextObject;
    private Text m_ScoreText;
    private Text m_HighScoreText;
    public float p_FlapForce = 10.0f;
    public float p_MaxVelocityY = 5.0f;

    public float p_MaxYPos = 1.0f;
    private bool e_Flapping = false;
    int m_Score = 0;
    int m_HighScore = 0;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<BoxCollider2D>();
        m_ScoreText = m_ScoreTextObject.GetComponent<Text>();
        m_HighScoreText = m_HighScoreTextObject.GetComponent<Text>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 0.0f;
        m_HighScore = PlayerPrefs.GetInt("HighScore", 0);
        m_HighScoreText.text = $"Best: {m_HighScore.ToString()}";

        m_GameOverTextObject.SetActive(false);
        m_PlayTextObject.SetActive(true);
        m_QuitTextObject.SetActive(false);
        #if !UNITY_WEBGL
        m_QuitTextObject.SetActive(true);
        #endif
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            e_Flapping = true;
        }
        if (Time.timeScale == 0.0f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Restart();
                m_PlayTextObject.SetActive(false);
                #if !UNITY_WEBGL
                m_QuitTextObject.SetActive(false);
                #endif
            }

            #if !UNITY_WEBGL
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerPrefs.SetInt("HighScore", m_HighScore);
                Application.Quit();
            }
            #endif
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
            GameOver();
        }
    }

    // OnTriggerEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Score")
        {
            m_Score++;
            m_ScoreText.text = m_Score.ToString();

            if (m_Score > m_HighScore)
            {
                m_HighScore = m_Score;
                m_HighScoreText.text = $"Best: {m_HighScore.ToString()}";
            }            
        }
    }

    private void GameOver()
    {
        PlayerPrefs.SetInt("HighScore", m_HighScore);

        Time.timeScale = 0.0f;
        m_GameOverTextObject.SetActive(true);
        m_PlayTextObject.SetActive(true);
        #if !UNITY_WEBGL
        m_QuitTextObject.SetActive(true);
        #endif
    }

    private void Restart()
    {
        m_GameOverTextObject.SetActive(false);

        Time.timeScale = 1.0f;
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject pipe in pipes)
        {
            Destroy(pipe);
        }
        m_Rigidbody.velocity = Vector2.zero;
        m_Rigidbody.position = new Vector2(0.0f, 0.0f);

        m_Score = 0;
        m_ScoreText.text = m_Score.ToString();
    }
}
