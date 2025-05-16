using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 targetPosition;
    private float minX, maxX, minY, maxY;
    private CircleCollider2D bossCircleCollider;
    private Vector2 offset;
    private bool isAlive = true;
    public bool isMovable = false;
    // Start is called before the first frame update
    void Start()
    {
        bossCircleCollider = GetComponent<CircleCollider2D>();
        SetRandomTargetPosition();
        if (bossCircleCollider != null)
        {
            offset = bossCircleCollider.offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ボスが生存かつ移動可能なときのみ移動を行う
        if (isAlive && isMovable)
        {
            MoveToTarget();
        }
    }

    // ランダムな座標を移動先として設定
    void SetRandomTargetPosition() 
    {
        Camera mainCamera = Camera.main;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWideth = cameraHeight * mainCamera.aspect;
        // カメラの中で移動できる範囲を計算（画面内に収める）
        minX = mainCamera.transform.position.x - cameraWideth / 2 + bossCircleCollider.radius;
        maxX = mainCamera.transform.position.x + cameraWideth / 2 - bossCircleCollider.radius;
        minY = mainCamera.transform.position.y + cameraHeight / 6 + bossCircleCollider.radius - 1.5f;
        maxY = mainCamera.transform.position.y + cameraHeight / 2 - bossCircleCollider.radius - 1.5f;

        float targetX = Random.Range(minX, maxX);
        float targetY = Random.Range(minY, maxY);

        targetPosition = new Vector3(targetX, targetY, transform.position.z);
    }

    // ターゲットへ向かって移動する処理
    private void MoveToTarget()
    {
        // 目的地に到達したら次のランダム座標を設定
        if (Vector3.Distance(transform.position + (Vector3)offset, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }
        // オフセットを考慮して位置を補正しながら移動
        transform.position = Vector3.MoveTowards(transform.position + (Vector3)offset, targetPosition, moveSpeed * Time.deltaTime) - (Vector3)offset;
    }

    // ボスを停止（死亡時など）
    public void StopMovement()
    {
        isAlive = false;
    }
}
