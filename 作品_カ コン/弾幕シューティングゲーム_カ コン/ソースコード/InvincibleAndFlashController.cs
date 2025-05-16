using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvincibleAndFlashController : MonoBehaviour
{
    public float invincibleTime = 0.5f;
    public float whiteFlashTime = 0.3f;
    public float flashTime = 0.1f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    public UnityEvent onPlayerInvincible;
    public UnityEvent resetInvincible;
    private Color originalColor;
    private BossExplosionController bossExplosionController;
    // Start is called before the first frame update
    void Start()
    {
        bossExplosionController = FindObjectOfType<BossExplosionController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        // ボスとの衝突イベントを登録
        if (bossExplosionController != null)
        {
            bossExplosionController.isCollision.AddListener(CollisionWithBoss);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 外部からダメージ処理を呼び出す際の入口
    public void TakeDamage()
    {
        if (isInvincible)
        {
            return;
        }
        StartCoroutine(InvincibleAndFlash());
    }

    // 無敵状態と点滅を同時に管理するコルーチン
    private IEnumerator InvincibleAndFlash()
    {
        isInvincible = true;
        StartCoroutine(FlashWhite());  // 点滅
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
        resetInvincible?.Invoke();  // UI側に通知
    }

    // 赤く点滅させる処理
    private IEnumerator FlashWhite()
    {
        float elapsedTime = 0f;
        while (elapsedTime < whiteFlashTime)
        {
            // 赤色に変更
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashTime);
            // 元の色に戻す
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashTime);
            elapsedTime += flashTime;
        }
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = originalColor;
    }

    // ボスとの衝突時の処理
    private void CollisionWithBoss()
    {
        TakeDamage();
        onPlayerInvincible?.Invoke();
    }

    // 弾（Barrage）との衝突時の処理
    public void CollisionWithBarrage()
    {
        TakeDamage();
        onPlayerInvincible?.Invoke();
    }

    // 敵との衝突時の処理
    public void CollisionWithEnemy()
    {
        TakeDamage();
        onPlayerInvincible?.Invoke();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Boss") && !isInvincible)
    //    {
    //        TakeDamage();
    //        onPlayerInvincible?.Invoke();
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Barrage") && !isInvincible)
    //    {
    //        TakeDamage();
    //        onPlayerInvincible?.Invoke();
    //    }
    //}
}
