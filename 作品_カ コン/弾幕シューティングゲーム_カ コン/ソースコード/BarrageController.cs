using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarrageController : MonoBehaviour
{
    private Camera mainCamera;
    public int damage = 1;
    private BossHpBarController hpBarController;
    private bool hasDied = false;
    public UnityEvent isCollision;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        hpBarController = FindObjectOfType<BossHpBarController>();
        if (hpBarController != null)
        {
            //hpBarController.onHpZero.RemoveListener(OnBossDeath);
            // ボスのHPがゼロになった時のイベントに登録
            hpBarController.onHpZero.AddListener(OnBossDeath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector2.down * speed * Time.deltaTime);
        // 画面外に出たらオブジェクトを削除する
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
    }

    // 弾が画面外にあるかどうかを判定
    private bool IsOutOfScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }

    // プレイヤーとの当たり判定処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasDied)
        {
            // プレイヤーにダメージを与える
            PlayerHpController playerHp = collision.gameObject.GetComponent<PlayerHpController>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(damage);
                isCollision?.Invoke();
            }
            // 弾を削除する
            Destroy(gameObject);
        }
    }

    // ボスが死亡した時に呼び出される
    private void OnBossDeath()
    {
        if (hasDied) { return; }
        hasDied = true;
    }
}
