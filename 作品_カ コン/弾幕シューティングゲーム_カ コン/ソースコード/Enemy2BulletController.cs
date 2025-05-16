using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed = 2f;
    public float spreadFactor = 1.5f;
    public Transform barragePointUnder;
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

    // 発射までの遅延処理
    IEnumerator FireBarrage()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(FanBullet());
    }

    // 扇状に弾を発射するコルーチン
    IEnumerator FanBullet()
    {
        int bulletCount = 6;
        float angleRange = 90f;
        float adjustedAngleRange = angleRange * spreadFactor;
        while (true)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                // 角度を等間隔に計算
                float angle = -adjustedAngleRange / 2 + adjustedAngleRange * i / (bulletCount - 1);
                GameObject bullet = Instantiate(bulletPrefab, barragePointUnder.position, Quaternion.identity);
                // 弾を生成し、角度に応じた方向へ発射
                Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad));
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
                // 衝突イベントをプレイヤーにバインド
                BarrageController barrage = bullet.GetComponent<BarrageController>();
                if (barrage != null && playerInvincibleController != null)
                {
                    barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
