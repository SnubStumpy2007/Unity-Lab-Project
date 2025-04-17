using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody PlayerRB;
    public GameObject PlayerRBPrefab;
    private float PlayerSpeed = 1.0f;
    private float XBoundary = 10.0f;
    private float ZBoundary = 6.0f;
    Vector3 OriginalPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //OriginalPosition = new Vector3(0, 0.5f, 0);
        //Instantiate(PlayerRBPrefab, OriginalPosition, Quaternion.identity);
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //Movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Debug.Log("W was pressed");
            PlayerRB.AddForce(Vector3.forward * PlayerSpeed, ForceMode.Impulse);
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log("S was pressed");
            PlayerRB.AddForce(Vector3.back * PlayerSpeed, ForceMode.Impulse);
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log("A was pressed");
            PlayerRB.AddForce(Vector3.left * PlayerSpeed, ForceMode.Impulse);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("D was pressed");
            PlayerRB.AddForce(Vector3.right * PlayerSpeed, ForceMode.Impulse);
        }

        //Boundaries.  If a player crosses these boundaries, destroy the player, then respawn the player
        if (PlayerRB.position.x >= XBoundary)
        {
            Debug.Log("Player has left the play area X");
            Respawn();
        } else if (PlayerRB.position.z >= ZBoundary)
        {
            Debug.Log("Player has left the play area Z");
            Respawn();
        } else if (PlayerRB.position.x <= -XBoundary)
        {
            Debug.Log("Player has left the play area -X");
            Respawn();
        } else if (PlayerRB.position.z <= -ZBoundary) {
            Debug.Log("Player has left the play area -Z");
            Respawn();
        } 
    }


        public void Respawn()
        {
        PlayerRB.linearVelocity = Vector3.zero;
        PlayerRB.position = OriginalPosition;
        Debug.Log("Player respawned");
        }
    }