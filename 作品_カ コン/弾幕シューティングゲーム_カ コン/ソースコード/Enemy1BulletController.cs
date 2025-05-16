using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed = 2f;
    private InvincibleAndFlashController playerInvincibleController;
    // Start is called before the first frame update
    void Start()
    {
        playerInvincibleController = FindObjectOfType<InvincibleAndFlashController>();
        StartCoroutine(FireBarrage());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 弾幕の発射を数秒遅らせるコルーチン
    IEnumerator FireBarrage()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(RingBullt());
    }

    // 円状に弾を発射し続けるコルーチン
    IEnumerator RingBullt()
    {
        int bulletCount = 8;
        float radius = 0.5f;
        while (true)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                // 発射角を均等に設定
                float angle = i * (360f / bulletCount);
                // 方向と位置を計算
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector3 spawnPosition = transform.position + (Vector3)(direction * radius);
                // 弾を生成して速度を設定
                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
                // プレイヤー衝突時イベントの登録
                BarrageController barrage = bullet.GetComponent<BarrageController>();
                if (barrage != null && playerInvincibleController != null)
                {
                    barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                }
            }
            yield return new WaitForSeconds(4f);
        }
    }
}
