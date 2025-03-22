using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movement")]
    public float speed = 5f;
    private float xVelocity;

    [Header("Jump")]
    public float jumpForce = 8f;
    public float doubleJumpForce = 8f;
    private bool canDoubleJump = false;

    [Header("Wall Jump")]
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;
    public float wallJumpForce = 8f;
    public Vector2 wallJumpAngle = new Vector2(1f, 1f);
    private bool isTouchingWall = false;

    [Header("Ground Check")]
    public float checkRadius = 0.2f;
    public LayerMask platform;
    public GameObject groundCheck;

    public bool isOnGround = false;
    private bool playerDead = false;

    //=== 新增：双击检测用变量 ===
    private float lastTapTime = 0f;
    private float doubleTapThreshold = 0.3f; // 两次点击的最大间隔

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 1. 检测地面
        isOnGround = Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, platform);
        anim.SetBool("isOnGround", isOnGround);

        // 2. 如果落地，恢复二段跳
        if (isOnGround) canDoubleJump = true;

        // 3. 检测左右墙
        bool wallOnRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        bool wallOnLeft  = Physics2D.Raycast(transform.position, Vector2.left,  wallCheckDistance, wallLayer);
        isTouchingWall = wallOnRight || wallOnLeft;

        // 4. 如果玩家没死，才允许移动/跳跃
        if (!playerDead)
        {
            //=== (A) 处理触屏/鼠标逻辑 ===
            HandleTouchInput();

            //=== (B) 处理键盘移动 & 跳跃（原逻辑）===
            Movement();
            HandleJump(wallOnLeft, wallOnRight);
        }
    }

    /// <summary>
    /// 原先的左右移动（键盘）
    /// </summary>
    private void Movement()
    {
        // 先获取键盘输入
        float keyboardX = Input.GetAxisRaw("Horizontal");

        // 如果触屏没设置 xVelocity，就用键盘输入，否则用触屏的 xVelocity
        // 这里演示：优先使用触屏xVelocity，如果触屏没按，就 fallback 键盘
        float finalX = Mathf.Abs(xVelocity) > 0.01f ? xVelocity : keyboardX;

        // 应用到刚体
        rb.linearVelocity = new Vector2(finalX * speed, rb.linearVelocity.y);
        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));

        // 根据移动方向翻转角色
        if (Mathf.Abs(finalX) > 0.01f)
            transform.localScale = new Vector3(Mathf.Sign(finalX), 1, 1);

        // 每帧重置 xVelocity（触屏部分）为 0，除非下帧又检测到触屏
        xVelocity = 0f;
    }

    /// <summary>
    /// 原先的跳跃逻辑（键盘Space）
    /// </summary>
    private void HandleJump(bool wallOnLeft, bool wallOnRight)
    {
        // 按下 Jump 键
        if (Input.GetButtonDown("Jump"))
        {
            if (isOnGround)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetTrigger("jump");
            }
            else
            {
                if (canDoubleJump && !isTouchingWall)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
                    anim.SetTrigger("jump");
                    canDoubleJump = false;
                }
                else if (isTouchingWall)
                {
                    if (wallOnLeft)
                        rb.linearVelocity = new Vector2(wallJumpAngle.x * wallJumpForce, wallJumpAngle.y * wallJumpForce);
                    else if (wallOnRight)
                        rb.linearVelocity = new Vector2(-wallJumpAngle.x * wallJumpForce, wallJumpAngle.y * wallJumpForce);

                    anim.SetTrigger("jump");
                }
            }
        }
    }

    /// <summary>
    /// 触屏/鼠标逻辑：单击左半屏=往左，右半屏=往右，双击=跳
    /// </summary>
    private void HandleTouchInput()
    {
        //=== 1) 判断是否有鼠标或触摸输入 ===
        // 在真机上用 touchCount > 0，这里演示用鼠标左键做模拟
        if (Input.GetMouseButtonDown(0))
        {
            // 检测是否双击
            float now = Time.time;
            if (now - lastTapTime < doubleTapThreshold)
            {
                //=== 检测到双击 => 让玩家跳一下
                // 如果玩家在地面，可以跳；或者你也可以做二段跳
                // 简化做法：直接当作一次跳
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetTrigger("jump");
            }
            else
            {
                //=== 单击：不立即移动，而是记录时间
                //   移动会在 GetMouseButton(0) 时检测
            }
            lastTapTime = now;
        }

        //=== 2) 如果鼠标/手指正在按住 => 水平移动
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            // 左半屏 => xVelocity = -1；右半屏 => xVelocity = 1
            if (mousePos.x < Screen.width / 2f)
                xVelocity = -1f;
            else
                xVelocity =  1f;
        }
        // 如果鼠标松开，就不会设置 xVelocity，Movement() 里会自动用键盘输入
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 如果踩到风扇（且在地面上），给向上的速度
        if (other.gameObject.CompareTag("Fan") && isOnGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果撞到尖刺，触发死亡
        if (other.CompareTag("Spike"))
        {
            if (!playerDead)
            {
                playerDead = true;
                anim.SetTrigger("dead");
                // 0.5秒后执行 PlayerDead
                Invoke(nameof(PlayerDead), 0.5f);
            }
        }
    }

    /// <summary>
    /// SawTrap等陷阱用来让玩家死亡
    /// </summary>
    public void DieBySawTrap()
    {
        if (!playerDead)
        {
            playerDead = true;
            anim.SetTrigger("dead");
            Invoke(nameof(PlayerDead), 0.5f);
        }
    }

    // 通知 GameManager 游戏结束
    private void PlayerDead()
    {
        playerDead = true;
        GameManager.instance.GameOver();
    }

    /// <summary>
    /// 复活时调用，重置玩家死亡状态
    /// </summary>
    public void ResetState()
    {
        playerDead = false;
        anim.ResetTrigger("dead");
        transform.position = new Vector2(0f, 0f);
        rb.linearVelocity = Vector2.zero;
    }
}
