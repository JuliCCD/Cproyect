using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveX = speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -speed;
        }

        rb.linearVelocity = new Vector2(moveX, rb.linearVelocity.y);
    }
}
