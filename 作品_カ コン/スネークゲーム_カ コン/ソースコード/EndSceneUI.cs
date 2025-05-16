using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndSceneUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text meetText;
    public float countDuration = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int finalMeet = PlayerPrefs.GetInt("FinalMeet", 0);

        StartCoroutine(CountUpText(scoreText, finalScore));
        StartCoroutine(CountUpText(meetText, finalMeet));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 数字を段階的に増やして表示するコルーチン
    IEnumerator CountUpText(TMP_Text targetText, int finalValue)
    {
        float elapsed = 0f;
        int currentValue = 0;

        while (elapsed < countDuration)
        {
            elapsed += Time.deltaTime;
            // 経過時間に応じて線形補間
            float t = Mathf.Clamp01(elapsed / countDuration);
            currentValue = Mathf.RoundToInt(Mathf.Lerp(0, finalValue, t));
            // テキスト更新
            targetText.text = currentValue.ToString();
            yield return null;
        }

        // 最終的な値が正確であることを確認する
        targetText.text = finalValue.ToString(); 
    }

    // 「End」ボタンが押されたときにタイトル画面に戻る
    public void OnEndButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
