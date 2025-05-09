using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       MoverseHorizontal();
       Saltar();
    }

    void MoverseHorizontal()
    {
        rb.linearVelocityX = 0;
        animator.SetInteger("Estado",0);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.linearVelocityX = 10;
            sr.flipX = false;
            animator.SetInteger("Estado",1);
        }
         if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocityX = -10;
            sr.flipX = true;
            animator.SetInteger("Estado",1);
        }
    }
    void Saltar()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.linearVelocityY = 10;
        }
    }
}
