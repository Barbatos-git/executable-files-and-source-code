using System.Collections.Generic;
using UnityEngine;

public class SnakeFollowManager : MonoBehaviour
{
    public GameObject bodyPrefab;
    public float gap = 0.5f;
    public float minGap = 0.01f;

    private List<Transform> followers = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();
    private float distanceSinceLastRecord = 0f;

    private SnakeHeadController head;

    void Start()
    {
        head = GetComponent<SnakeHeadController>();
        positionHistory.Insert(0, transform.position);
    }

    void FixedUpdate()
    {
        // 移動距離を計算して履歴追加
        Vector2 velocity = head.GetMoveDirection() * head.moveSpeed;
        distanceSinceLastRecord += (velocity * Time.fixedDeltaTime).magnitude;

        if (distanceSinceLastRecord >= minGap)
        {
            positionHistory.Insert(0, transform.position);
            distanceSinceLastRecord = 0f;
        }

        UpdateFollowers();  // 体の追従処理
        CleanUpHistory();  // 古い履歴を削除
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
        return positionHistory[positionHistory.Count - 1];  // 履歴末尾
    }

    void CleanUpHistory()
    {
        int maxHist = Mathf.RoundToInt((followers.Count + 2) * gap / minGap);
        if (positionHistory.Count > maxHist)
            positionHistory.RemoveRange(maxHist, positionHistory.Count - maxHist);
    }

    public void AddFollower()
    {
        Vector3 spawnPos = followers.Count == 0 ? transform.position : followers[^1].position;
        GameObject newFollower = Instantiate(bodyPrefab, spawnPos, Quaternion.identity);

        if (!newFollower.GetComponent<SnakeBodyPart>())
            newFollower.AddComponent<SnakeBodyPart>();

        followers.Add(newFollower.transform);
    }

    public int GetFollowerCount() => followers.Count;

    public List<Transform> GetFollowers()
    {
        return followers;
    }
}
