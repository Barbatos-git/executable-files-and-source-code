using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHpController : MonoBehaviour
{
    public int maxHp = 10;
    public int currentHp;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public UnityEvent onPlayerDeath;
    private bool isInvincible = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        // 無敵状態イベントを受け取る設定
        InvincibleAndFlashController Invincible = FindObjectOfType<InvincibleAndFlashController>();
        if (Invincible != null)
        {
            Invincible.onPlayerInvincible.AddListener(OnPlayerInvincible);
            Invincible.resetInvincible.AddListener(ResetPlayerInvincible);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // HPに応じてハートUIを更新
    void UpdateHpBar()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartStatus = currentHp - (i * 2);
            if (heartStatus >= 2)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (heartStatus == 1)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    // ダメージを受けたときの処理
    public void TakeDamage(int damage) 
    {
        if (isInvincible)
        {
            return;
        }
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            onPlayerDeath?.Invoke();
        }
    }

    // 無敵状態フラグON
    private void OnPlayerInvincible()
    {
        isInvincible = true;
    }

    // 無敵状態フラグOFF
    private void ResetPlayerInvincible()
    {
        isInvincible = false;
    }
}
