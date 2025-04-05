using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = 5;
        }
        rb.linearVelocityX = 0 ;
    }
}
