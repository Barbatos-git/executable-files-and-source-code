using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScoreUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public string nextSceneName = "";
    // Start is called before the first frame update
    void Start()
    {
        // 初期状態は完全に透明
        canvasGroup.alpha = 0;
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        // マウス左クリック or スペースキーでシーン遷移
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            TransitionToNextScene();
        }
    }

    // フェードイン処理（透明→表示）
    IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    // シーンを指定の名前で切り替える
    void TransitionToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
