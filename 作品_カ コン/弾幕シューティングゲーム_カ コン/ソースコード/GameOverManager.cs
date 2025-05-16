using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Image fadeImage;
    public Image gameOverImage;
    public float fadeDuration = 2f;
    public float waitTimeBeforeTransition = 2f;
    public string nextSceneName = "";
    private float fadeAlpha = 0f;
    private float gameOverAlpha = 0f;
    private bool isGameOverTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHpController playerHp = FindObjectOfType<PlayerHpController>();
        if (playerHp != null)
        {
            playerHp.onPlayerDeath.AddListener(TriggerGameOver);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOverTriggered)
        {
            // 黒幕を徐々にフェードイン
            if (fadeAlpha < 225f/255f)
            {
                fadeAlpha += Time.deltaTime / fadeDuration;
                fadeImage.color = new Color(0f, 0f, 0f, Mathf.Clamp01(fadeAlpha));
            }
            // 黒幕がある程度濃くなったら、「Game Over」画像をフェードイン
            if (fadeAlpha >= 225f/255f && gameOverAlpha < 1f)
            {
                gameOverAlpha += Time.deltaTime / fadeDuration;
                gameOverImage.color = new Color(1f, 1f, 1f, Mathf.Clamp01(gameOverAlpha));
            }
            // 両方表示されたら遷移
            if (gameOverAlpha >= 1f)
            {
                Invoke("TransitionToNextScene", waitTimeBeforeTransition);
                isGameOverTriggered = false;
            }
        }
    }

    // ゲームオーバー演出を開始する
    public void TriggerGameOver()
    {
        if (!isGameOverTriggered)
        {
            isGameOverTriggered = true;
            fadeImage.gameObject.SetActive(true);
            gameOverImage.gameObject.SetActive(true);
        }
    }

    // 次のシーンに遷移
    void TransitionToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
