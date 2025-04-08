using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float PlayerSpeed = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W was pressed");
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S was pressed");
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A was pressed");
        } else if(Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D was pressed");
        }
    }
}
