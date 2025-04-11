using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody PlayerRB;
    private float PlayerSpeed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
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
        } else if(Input.GetKeyDown(KeyCode.D))
        {
            //Debug.Log("D was pressed");
            PlayerRB.AddForce(Vector3.right * PlayerSpeed, ForceMode.Impulse);
        }
    }
}
