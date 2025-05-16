        using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI bossHpText;
    public TextMeshProUGUI gameTimeText;
    public TextMeshProUGUI playerHpText;
    public TextMeshProUGUI scoreText;

    private float displayScore = 0;
    private float targetScore;
    // Start is called before the first frame update
    void Start()
    {
        float bossHp = GameDataManager.Instance.bossHp;
        float gameTime = GameDataManager.Instance.gameTime;
        float playerHp = GameDataManager.Instance.playerHp;
        float maxPlayerHp = GameDataManager.Instance.maxPlayerHp;
        SetUp(bossHp, gameTime, playerHp, maxPlayerHp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 表示を初期化し、スコアを計算
    public void SetUp(float bossHp, float gameTime, float playerHp, float maxPlayerHp)
    {
        // ボスHP
        bossHpText.text = bossHp.ToString("F0");

        //gameTimeText.text = gameTime.ToString("F1") + "s";
        // 経過時間（分:秒形式）
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        gameTimeText.text = $"{minutes}:{seconds.ToString("D2")}s";
        // プレイヤーHP
        playerHpText.text = playerHp + "/" + maxPlayerHp;
        // スコア計算（時間とHPの重みづけ）
        float timeWeight = 0.6f;
        float playerHpWeight = 0.4f;
        float timeScore = Mathf.Clamp(bossHp - gameTime * 10, 0, bossHp);
        float hpScore = (playerHp / maxPlayerHp) * bossHp;
        // 最終スコア計算
        targetScore = bossHp + timeScore * timeWeight + hpScore * playerHpWeight;
        // スコア表示アニメーション開始
        StartCoroutine(AnimateScore());
    }

    // スコアを徐々に加算表示する演出
    IEnumerator AnimateScore()
    {
        while (displayScore < targetScore)
        {
            displayScore += Time.deltaTime * 20000;
            scoreText.text = Mathf.FloorToInt(displayScore).ToString();
            yield return null;
        }
        // 最終値で止める
        displayScore = targetScore;
        scoreText.text = Mathf.FloorToInt(displayScore).ToString();
    }
}
