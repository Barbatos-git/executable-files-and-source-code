using System.Collections.Generic;
using UnityEngine;

public class SnakeGirlController : MonoBehaviour
{
    public GameObject bodyPrefab; // 身后添加的角色 prefab
    public GameObject applePrefab;
    public float moveSpeed = 2f;
    public float gap = 0.32f;
    public float speedIncrease = 0.5f;
    public float minGap = 0.08f; // 插值间隔，越小越平滑
    private float distanceSinceLastRecord = 0f;

    private Vector2 moveDirection = Vector2.down;
    private Vector2 nextDirection = Vector2.down;
    private Rigidbody2D rb;

    private List<Transform> followers = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        positionHistory.Insert(0, transform.position);
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        MoveHead();
        UpdateFollowers();
        CleanUpHistory();
        UpdateAnimation();
    }

    void HandleInput()
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

    void MoveHead()
    {
        moveDirection = nextDirection;
        rb.linearVelocity = moveDirection * moveSpeed;

        // 移動距離を累積して記録
        distanceSinceLastRecord += (rb.linearVelocity * Time.fixedDeltaTime).magnitude;
        if (distanceSinceLastRecord >= minGap)
        {
            positionHistory.Insert(0, transform.position);
            distanceSinceLastRecord = 0f;
        }
    }

    void UpdateFollowers()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            float dist = (i + 1) * gap;
            Vector3 currentPos = GetPositionAtDistance(dist);
            Vector3 prevPos = GetPositionAtDistance(dist - 0.01f);

            followers[i].position = currentPos;

            Vector3 dir = prevPos - currentPos; 

            if (dir != Vector3.zero)
            {
                var anim = followers[i].GetComponent<Animator>();
                anim?.SetFloat("MoveX", dir.x);
                anim?.SetFloat("MoveY", dir.y);
            }
        }
    }

    Vector3 GetPositionAtDistance(float distance)
    {
        float accumulated = 0f;
        for (int i = 0; i < positionHistory.Count - 1; i++)
        {
            Vector3 p1 = positionHistory[i];
            Vector3 p2 = positionHistory[i + 1];
            float d = Vector3.Distance(p1, p2);

            if (accumulated + d >= distance)
            {
                float t = (distance - accumulated) / d;
                return Vector3.Lerp(p1, p2, t);
            }

            accumulated += d;
        }

        // 経路の長さを超えている場合、最後の位置を返す
        return positionHistory[positionHistory.Count - 1];
    }

    void CleanUpHistory()
    {
        int maxHist = Mathf.RoundToInt((followers.Count + 2) * gap / minGap);
        if (positionHistory.Count > maxHist)
            positionHistory.RemoveRange(maxHist, positionHistory.Count - maxHist);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            Destroy(other.gameObject);
            AddFollower();
            IncreaseSpeed();
            SpawnApple();
        }
    }

    void AddFollower()
    {
        Vector3 spawnPos = followers.Count == 0 ? transform.position : followers[followers.Count - 1].position;
        GameObject newFollower = Instantiate(bodyPrefab, spawnPos, Quaternion.identity);
        followers.Add(newFollower.transform);
    }

    void IncreaseSpeed()
    {
        moveSpeed = Mathf.Min(moveSpeed + speedIncrease, 20f);
    }

    void SpawnApple()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f)
        );
        Instantiate(applePrefab, spawnPos, Quaternion.identity);
    }

    void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
    }
}
