using UnityEngine;

public class PipeMovement : MonoBehaviour
{

    public float p_Speed = 1.0f;
    public float p_KillAtX = -10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.left * p_Speed * Time.fixedDeltaTime;
        if (transform.position.x < p_KillAtX)
        {
            Destroy(gameObject);
        }
    }
}
