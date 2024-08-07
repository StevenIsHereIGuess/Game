using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask jumpableGround;
    public GameObject spawnPoint;
    public float speed;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    public float diveSpeed;
    public float diveAngle;
    public float decelerationTime;

    private bool isGrounded;
    private bool isDashing;
    private bool isDiving;
    private bool canDash = true;
    private float dashDirection;
    private float diveDirection;

    private Coroutine currentCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float dirX = Input.GetAxis("Horizontal");

        if (!isDashing && !isDiving)
        {
            rb.velocity = new Vector2(dirX * speed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, 14f);
            }

            if (Input.GetKeyDown(KeyCode.E) && canDash)
            {
                dashDirection = dirX != 0 ? Mathf.Sign(dirX) : transform.localScale.x; // Dash in the direction of movement or facing direction
                StartCoroutine(Dash());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                diveDirection = dirX != 0 ? Mathf.Sign(dirX) : transform.localScale.x; // Dive in the direction of movement or facing direction
                StartCoroutine(Dive(diveDirection));
            }
        }
        else if (isDashing && Input.GetKeyDown(KeyCode.Q))
        {
            StopCurrentAction();
            diveDirection = dirX != 0 ? Mathf.Sign(dirX) : transform.localScale.x; // Dive in the direction of movement or facing direction
            StartCoroutine(Dive(diveDirection));
        }
        else if (isDiving && Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StopCurrentAction();
            dashDirection = dirX != 0 ? Mathf.Sign(dirX) : transform.localScale.x; // Dash in the direction of movement or facing direction
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0);

        yield return new WaitForSeconds(dashDuration);

        StartCoroutine(Decelerate(dashDirection, dashSpeed));

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator Dive(float direction)
    {
        isDiving = true;
        float diveVelocityX = Mathf.Cos(diveAngle * Mathf.Deg2Rad) * diveSpeed * direction;
        float diveVelocityY = Mathf.Sin(diveAngle * Mathf.Deg2Rad) * diveSpeed;

        rb.velocity = new Vector2(diveVelocityX, -diveVelocityY);

        yield return new WaitForSeconds(0.5f); // Adjust the duration of the dive as needed

        StartCoroutine(Decelerate(direction, diveSpeed));

        isDiving = false;
    }

    private IEnumerator Decelerate(float direction, float initialSpeed)
    {
        float timeElapsed = 0;

        while (timeElapsed < decelerationTime)
        {
            rb.velocity = new Vector2(Mathf.Lerp(initialSpeed * direction, 0, timeElapsed / decelerationTime), rb.velocity.y);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = new Vector2(0, rb.velocity.y);
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

    private void StopCurrentAction()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        isDashing = false;
        isDiving = false;
    }
}
