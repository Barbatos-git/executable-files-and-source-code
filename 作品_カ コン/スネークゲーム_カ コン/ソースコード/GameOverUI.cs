using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public CanvasGroup canvasGroup;
    public Button restartButton;
    public Button endButton;
    public float fadeDuration = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        // 初期状態で非表示にする
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameOverPanel.SetActive(false);
        // ボタンにイベントを登録
        restartButton.onClick.AddListener(RestartGame);
        endButton.onClick.AddListener(EndGame);
    }

    // ゲームオーバー画面を表示
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        StartCoroutine(FadeInUI());
    }

    // UIをフェードインさせるコルーチン
    private IEnumerator FadeInUI()
    {
        canvasGroup.alpha = 0;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // 現在のシーンを再読込して再スタート
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMEnabled(true);
        }
    }

    // エンドシーンに移行
    public void EndGame()
    {
        SceneManager.LoadScene("EndScene");
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMEnabled(true);
        }
    }
}
