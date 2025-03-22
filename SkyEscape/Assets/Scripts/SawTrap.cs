using UnityEngine;

public class SawTrap : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform leftPoint;    // 圆锯移动的左端点
    public Transform rightPoint;   // 圆锯移动的右端点
    public float moveSpeed = 1f;   // 移动速度

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f; // 每秒旋转的角度

    private Vector3 nextPos;       // 圆锯的下一个目标位置

    void Start()
    {
        // 起始目标设为 rightPoint，如果没设置，就用当前位置
        if (rightPoint != null)
            nextPos = rightPoint.position;
        else
            nextPos = transform.position;
    }

    void Update()
    {
        MoveSaw();
        RotateSaw();
    }

    /// <summary>
    /// 在两个端点之间来回移动
    /// </summary>
    private void MoveSaw()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, nextPos) < 0.01f)
        {
            if (nextPos == rightPoint.position)
                nextPos = leftPoint.position;
            else
                nextPos = rightPoint.position;
        }
    }

    /// <summary>
    /// 让圆锯在Z轴上自转
    /// </summary>
    private void RotateSaw()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 使用碰撞检测：OnCollisionEnter2D
    /// => 圆锯的 Collider2D "Is Trigger" 不勾选
    /// => Player + SawTrap 至少一方是 Dynamic Rigidbody2D
    /// => SawTrap 通常设为 Kinematic，Player 设为 Dynamic
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果与玩家碰撞
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("SawTrap collided with the Player!");
            // 调用玩家脚本的 DieByTrap()，让玩家执行死亡流程
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.DieBySawTrap();
            }
        }
    }
}
