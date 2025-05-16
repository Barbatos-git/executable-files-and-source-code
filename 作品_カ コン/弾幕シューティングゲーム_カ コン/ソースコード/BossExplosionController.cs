using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Jobs;

public class BossExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform[] explosionPoints;
    [SerializeField] private float explosionInterval = 0.3f;
    [SerializeField] private BossHpBarController hpBarController;
    private BossController bossController;
    private bool hasDied = false;
    [SerializeField] private float fadeDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private bool isFading = false;
    private GameObject bossReCenter;
    public int damage = 1;
    public UnityEvent isCollision;
    // Start is called before the first frame update
    void Start()
    {
        bossReCenter = GameObject.Find("BossReCenter");
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossController = bossReCenter.GetComponent<BossController>();
        if (hpBarController != null)
        {
            hpBarController.onHpZero.RemoveListener(OnBossDeath);
            hpBarController.onHpZero.AddListener(OnBossDeath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ボスのHPが0になったときに呼ばれる
    private void OnBossDeath()
    {
        if (hasDied) { return; }
        hasDied = true;
        // 移動を停止
        if (bossController != null)
        {
            bossController.StopMovement();
        }
        // 爆発アニメーション開始
        StartCoroutine(TriggerExplosionsSequentially());
        // フェードアウト処理
        if (!isFading)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    // 爆発エフェクトを順番に発生させる
    private IEnumerator TriggerExplosionsSequentially()
    {
        Transform[] shuffledPoints = ShuffleArray(explosionPoints);
        foreach (var point in shuffledPoints)
        {
            Instantiate(explosionPrefab, point.position, Quaternion.identity);
            yield return new WaitForSeconds(explosionInterval);
        }
    }

    // 爆発ポイントの順序をランダムにシャッフル
    private Transform[] ShuffleArray(Transform[] array)
    {
        Transform[] shuffledArray = (Transform[])array.Clone();
        for (int i = shuffledArray.Length-1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = shuffledArray[i];
            shuffledArray[i] = shuffledArray[randomIndex];
            shuffledArray[randomIndex] = temp;
        }
        return shuffledArray;
    }

    // フェードアウトしながらオブジェクトを破壊
    private IEnumerator FadeOutAndDestroy()
    {
        yield return new WaitForSeconds(2f);
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
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        Destroy(gameObject);
        // 親オブジェクトも削除
        Transform parent = transform.parent;
        if (parent != null)
        {
            Destroy(parent.gameObject);
        }
    }

    // プレイヤーとの接触処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasDied)
        {
            PlayerHpController playerHp = collision.gameObject.GetComponent<PlayerHpController>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(damage);
                isCollision?.Invoke();
            }
        }
    }
}
