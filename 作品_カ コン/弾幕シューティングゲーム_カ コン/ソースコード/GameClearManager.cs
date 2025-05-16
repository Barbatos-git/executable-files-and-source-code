using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    public Image fadeImage;
    public Image gameClearImage;
    public float fadeDuration = 2f;
    public float waitTimeBeforeTransition = 2f;
    public string nextSceneName = "";
    private float gameClearAlpha = 0f;
    private bool isGameClearTriggered = false;
    private float gameTimes = 0f;
    // Start is called before the first frame update
    void Start()
    {
        BossAppear bossAppear = FindObjectOfType<BossAppear>();
        if (bossAppear != null)
        {
            bossAppear.bossHpActive.AddListener(BossHpActive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTimes += Time.deltaTime;
        if (isGameClearTriggered)
        {
            // アルファ値をフェードイン
            gameClearAlpha += Time.deltaTime / fadeDuration;
            gameClearImage.color = new Color(1f, 1f, 1f, Mathf.Clamp01(gameClearAlpha));
            // 完全に表示されたら遷移予約
            if (gameClearAlpha >= 1f)
            {
                Invoke("TransitionToNextScene", waitTimeBeforeTransition);
                isGameClearTriggered = false;
            }
        }
    }

    // ボスHPバーが表示されたときに呼ばれる
    private void BossHpActive()
    {
        BossHpBarController bossHp = FindObjectOfType<BossHpBarController>();
        if (bossHp != null)
        {
            bossHp.onHpZero.AddListener(TriggerGameClear);
        }
    }

    // ボスが倒されたときに呼び出される
    public void TriggerGameClear()
    {
        if (!isGameClearTriggered)
        {
            isGameClearTriggered = true;
            fadeImage.gameObject.SetActive(true);
            gameClearImage.gameObject.SetActive(true);
        }
    }

    // 次のシーンへ遷移し、必要なゲームデータを保存
    void TransitionToNextScene()
    {
        BossHpBarController bossHpBarController = FindObjectOfType<BossHpBarController>();
        GameDataManager.Instance.bossHp = bossHpBarController.maxHp;
        GameDataManager.Instance.gameTime = gameTimes - fadeDuration - waitTimeBeforeTransition;
        PlayerHpController playerHpController = FindObjectOfType<PlayerHpController>();
        GameDataManager.Instance.playerHp = playerHpController.currentHp;
        GameDataManager.Instance.maxPlayerHp = playerHpController.maxHp;

        SceneManager.LoadScene(nextSceneName);
    }
}
