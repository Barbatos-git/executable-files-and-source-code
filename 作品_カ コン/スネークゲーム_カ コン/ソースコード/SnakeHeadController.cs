using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class SnakeHeadController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Vector2 moveDirection = Vector2.down;
    public Vector2 nextDirection = Vector2.down;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;
        HeadInput();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        moveDirection = nextDirection;
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    // 矢印キーによる移動方向の入力処理
    void HeadInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && moveDirection != Vector2.down)
            nextDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && moveDirection != Vector2.up)
            nextDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection != Vector2.right)
            nextDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && moveDirection != Vector2.left)
            nextDirection = Vector2.right;
    }

    void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
    }

    public Vector2 GetMoveDirection() => moveDirection;

    public void StopMovement()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
    }

    // 自分の体にぶつかったら死亡処理を呼ぶ
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SnakeBodyPart>())
        {
            SnakeGameManager.Instance.OnPlayerDied();
        }
    }
}
