using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotController : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject explosionPrefab;
    public float explosionCooldown = 1f;
    private static float lastExplosionTime = 0f;
    public float damage = 10f;
    private GameObject bossHpBar;
    private BossHpBarController bossHp;
    private EnemyExplosionAndHpController enemyHp;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        bossHpBar = GameObject.Find("BossHpBar");
        if (bossHpBar != null)
        { bossHp = bossHpBar.GetComponent<BossHpBarController>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
    }

    // 弾が画面外にあるかどうかのチェック
    private bool IsOutOfScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }

    // 衝突判定
    void OnTriggerEnter2D(Collider2D collision)
    {
        // ボスに命中
        if (collision.gameObject.CompareTag("Boss"))
        {
            if (Time.time - lastExplosionTime >= explosionCooldown)
            {
                SpawnExplosion();
                lastExplosionTime = Time.time;
            }
            if (bossHp != null)
            {
                bossHp.TakeDamage(damage);
                //Debug.Log(damage);
            }
            Destroy(gameObject);
        }
        // 敵に命中
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyHp = collision.GetComponent<EnemyExplosionAndHpController>();
            if (Time.time - lastExplosionTime >= explosionCooldown)
            {
                SpawnExplosion();
                lastExplosionTime = Time.time;
            }
            if (enemyHp != null)
            {
                enemyHp.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    // 爆発エフェクトを生成
    void SpawnExplosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
