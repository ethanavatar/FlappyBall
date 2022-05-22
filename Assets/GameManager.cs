using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject m_ScoreTextObject;
    public GameObject m_HighScoreTextObject;
    public GameObject m_GameOverTextObject;
    public GameObject m_PlayTextObject;
    public GameObject m_QuitTextObject;

    private Text m_ScoreText;
    private Text m_HighScoreText;

    int m_Score = 0;

    public int m_HighScore = 0;

    [SerializeField]
    private bool m_IsPaused = false;

    private bool m_IsWebGL = false;

    void Awake()
    {
        #if UNITY_WEBGL
        m_IsWebGL = true;
        #endif

        m_GameOverTextObject.SetActive(false);
        m_PlayTextObject.SetActive(true);
        m_QuitTextObject.SetActive(false);

        if (!m_IsWebGL)
        {
            m_QuitTextObject.SetActive(true);
        }

        m_ScoreText = m_ScoreTextObject.GetComponent<Text>();
        m_HighScoreText = m_HighScoreTextObject.GetComponent<Text>();

        m_HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Start()
    {
        Pause(true);
        m_HighScoreText.text = $"Best: {m_HighScore.ToString()}";
    }

    void Update()
    {
        if (!m_IsPaused)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            m_PlayTextObject.SetActive(false);
            #if !UNITY_WEBGL
            m_QuitTextObject.SetActive(false);
            #endif

            Restart();

            return;
        }

        if (!m_IsWebGL && Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.SetInt("HighScore", m_HighScore);
            Application.Quit();
        }
    }


    void Pause(bool state)
    {
        m_IsPaused = state;

        if (m_IsPaused)
        {
            Time.timeScale = 0;
            m_GameOverTextObject.SetActive(true);
            m_PlayTextObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            m_GameOverTextObject.SetActive(false);
            m_PlayTextObject.SetActive(false);
        }
    }

    public void Restart()
    {
        m_GameOverTextObject.SetActive(false);

        Time.timeScale = 1.0f;

        ClearPipes();

        m_Score = 0;
        m_ScoreText.text = m_Score.ToString();

        Pause(false);
    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("HighScore", m_HighScore);

        Pause(true);
        m_GameOverTextObject.SetActive(true);
        m_PlayTextObject.SetActive(true);

        if (!m_IsWebGL)
        {
            m_QuitTextObject.SetActive(true);
        }
    }

    public void AddScore()
    {
        m_Score++;
        m_ScoreText.text = m_Score.ToString();

        if (m_Score > m_HighScore)
        {
            m_HighScore = m_Score;
            m_HighScoreText.text = $"Best: {m_HighScore.ToString()}";
        }
    }

    private void ClearPipes()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject pipe in pipes)
        {
            Destroy(pipe);
        }
    }
}