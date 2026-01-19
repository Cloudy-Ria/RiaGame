using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private Rigidbody2D rb;
    private Transform target;
    private bool facingRight = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        target = pointB;
    }

    void FixedUpdate()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        rb.linearVelocity = dir * speed;

        // разворот
        if (dir.x > 0 && !facingRight) Flip();
        if (dir.x < 0 && facingRight) Flip();

        // смена точки
        if (Vector2.Distance(transform.position, target.position) < 0.2f)
            target = target == pointA ? pointB : pointA;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}
