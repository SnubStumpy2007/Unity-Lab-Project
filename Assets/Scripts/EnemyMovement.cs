using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody EnemyRB;
    public GameObject EnemyRBPrefab;
    private GameObject Player;
    private float EnemySpeed = 10.0f;
    private float XBoundary = 10.0f;
    private float ZBoundary = 6.0f;
    Vector3 OriginalPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyRB = GetComponent<Rigidbody>();
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 LookDirection = (Player.transform.position - transform.position).normalized;
        EnemyRB.AddForce(LookDirection * EnemySpeed * Time.deltaTime);


        if (EnemyRB.position.x >= XBoundary)
        {
            Debug.Log("Enemy has left the play area X");
            Respawn();
        }
        else if (EnemyRB.position.z >= ZBoundary)
        {
            Debug.Log("Enemy has left the play area Z");
            Respawn();
        }
        else if (EnemyRB.position.x <= -XBoundary)
        {
            Debug.Log("Enemy has left the play area -X");
            Respawn();
        }
        else if (EnemyRB.position.z <= -ZBoundary)
        {
            Debug.Log("Enemy has left the play area -Z");
            Respawn();
        }
    }

    public void Respawn()
    {
        EnemyRB.linearVelocity = Vector3.zero;
        EnemyRB.position = OriginalPosition;
        Debug.Log("Enemy respawned");
    }
}
