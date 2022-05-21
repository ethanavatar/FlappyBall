using UnityEngine;

public class PipesManager : MonoBehaviour
{
    public GameObject p_PipePrefab;
    public GameObject p_ScoreTriggerPrefab;
    public float p_PipeSpawnInterval_s = 1f;
    public float p_PipeMinY = -1.0f;
    public float p_PipeMaxY = 1.0f;

    public float p_GapSize = 1.0f;
    private float m_NextPipeSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        m_NextPipeSpawnTime = p_PipeSpawnInterval_s;
        SpawnPipes();
    }

    // Update is called once per frame
    void Update()
    {
        m_NextPipeSpawnTime -= Time.deltaTime;
        
        if (m_NextPipeSpawnTime <= 0.0f)
        {
            SpawnPipes();
            m_NextPipeSpawnTime = p_PipeSpawnInterval_s;
        }
    }

    private void SpawnPipes()
    {
        float masterY = Random.Range(p_PipeMinY, p_PipeMaxY);
        GameObject pipe1head = Instantiate(p_PipePrefab, transform.position, Quaternion.identity);
        pipe1head.transform.position = new Vector3(pipe1head.transform.position.x, masterY - p_GapSize, pipe1head.transform.position.z);

        GameObject pipe2 = Instantiate(p_PipePrefab, transform.position, Quaternion.identity);
        pipe2.transform.position = new Vector3(pipe2.transform.position.x, masterY + p_GapSize, pipe2.transform.position.z);
        pipe2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);

        GameObject scoreTrigger = Instantiate(p_ScoreTriggerPrefab, transform.position, Quaternion.identity);
        scoreTrigger.transform.position = new Vector3(scoreTrigger.transform.position.x, scoreTrigger.transform.position.y, scoreTrigger.transform.position.z);
        scoreTrigger.transform.SetParent(pipe1head.transform);
    }
}
