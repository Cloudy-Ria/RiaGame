using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private Rigidbody2D rb;
    private Transform currentPoint;
    private bool facingRight = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        currentPoint = pointB;
    }

    void FixedUpdate()
    {
        Vector2 direction = (currentPoint.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        HandleFlip(direction.x);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.2f)
        {
            currentPoint = currentPoint == pointA ? pointB : pointA;
        }
    }

    void HandleFlip(float moveX)
    {
        if (moveX > 0 && !facingRight)
            Flip();
        else if (moveX < 0 && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if (pointA && pointB)
            Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
