using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public float speed;
    float xVelocity;

    public float checkRadius;
    public LayerMask platform;
    public GameObject groundCheck;
    public bool isOnGround;

    bool playerDead = false;  // **确保 `playerDead` 默认是 `false`**

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, platform);
        anim.SetBool("isOnGround", isOnGround);

        if (!playerDead)  // **如果玩家死亡，就不要继续移动**
        {
            Movement();
        }
    }

    void Movement()
    {
        xVelocity = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(xVelocity * speed, rb.linearVelocity.y); // **修正 `linearVelocity → velocity`**

        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x)); // **Run 动画**

        if (xVelocity != 0)
        {
            transform.localScale = new Vector3(xVelocity, 1, 1);
        }
    }

    // **角色碰到风扇时，被吹起**
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Fan") && isOnGround) // **确保角色在地面上时才被吹起**
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);
        }
    }

    // **角色碰到死亡陷阱**
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))  // **碰到尖刺**
        {
            if (!playerDead)  // **防止重复触发**
            {
                playerDead = true;
                anim.SetTrigger("dead");  // **播放死亡动画**
                Invoke("PlayerDead", 0.5f); // **0.5s 后执行 `PlayerDead()`，给动画时间**
            }
        }
    }

    // **玩家死亡**
    public void PlayerDead()
    {
        playerDead = true;
        GameManager.instance.GameOver();  // **立即调用 `GameOver()`**
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.transform.position, checkRadius);
    }
}
