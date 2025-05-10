using UnityEngine;

public class KunaiController: MonoBehaviour
{
    private string direccion = "Derecha";

    Rigidbody2D rb;
    SpriteRenderer sr;

    void Start()
    {
        // Initialize the Kunai object
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        // Update the Kunai object
        if (direccion == "Derecha")
        {
            rb.linearVelocityX = 15;
            sr.flipY = false;
            
        }
        else if (direccion == "Izquierda")
        {
            rb.linearVelocityX = -15;
            sr.flipY = true;
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with the Kunai object
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(string direction)
    {
        this.direccion = direction;
    }
    
}
