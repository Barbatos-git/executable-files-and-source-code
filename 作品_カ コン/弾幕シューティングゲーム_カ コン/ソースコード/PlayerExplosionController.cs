using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float fadeDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private bool isFading = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // プレイヤー死亡イベントに登録
        PlayerHpController playerHp = FindObjectOfType<PlayerHpController>();
        if (playerHp != null)
        {
            playerHp.onPlayerDeath.AddListener(OnPlayerDeath);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 死亡時に呼び出される処理
    private void OnPlayerDeath()
    {
        // 爆発を生成
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // フェード処理開始
        if (!isFading)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    // 徐々に透明にして削除するコルーチン
    private IEnumerator FadeOutAndDestroy()
    {
        isFading = true;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        // 完全に透明にしてから削除
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        Destroy(gameObject);
    }
}
