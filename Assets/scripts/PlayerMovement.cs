using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    Vector2 movement;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    Vector2 lastMove;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 1)
            movement = movement.normalized;

        if (movement != Vector2.zero)
            lastMove = movement;

        // Animator parametreleri
        anim.SetBool("isMoving", movement != Vector2.zero);
        anim.SetFloat("moveX", movement.x);
        anim.SetFloat("moveY", movement.y);

        // Flip (sol)
        if (movement.x < 0)
            sr.flipX = true;
        else if (movement.x > 0)
            sr.flipX = false;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}