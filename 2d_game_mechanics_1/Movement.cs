using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Declare the Rigidbody2D variable
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask jumpableGround;
    public GameObject spawnPoint;
    public float speed;
    [SerializeField] private int numDashes;
    [SerializeField] private int dashForce;

    private int dashesLeft;
    private bool isGrounded;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        float dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, 14f);
        }

        // Dash logic
        if (Input.GetAxis("Horizontal") != 0 && !IsGrounded() && rb.velocity.y <= 0)
        {
            if (dashesLeft > 0)
            {
                Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
                rb.velocity = Vector3.zero;
                rb.AddForce(dir.normalized * dashForce);
                dashesLeft--;
            }
        }
    }

    void FixedUpdate()
    {
        if (IsGrounded())
        {
            dashesLeft = numDashes;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("object"))
        {
            transform.position = spawnPoint.transform.position;
        }
    }
}
