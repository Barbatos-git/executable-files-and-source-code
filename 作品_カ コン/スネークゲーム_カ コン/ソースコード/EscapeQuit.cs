using UnityEngine;

public class EscapeQuit : MonoBehaviour
{
    private static EscapeQuit instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // シングルトン化（複数存在しないように）
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // シーンを跨いでも残す
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // ESCキーが押されたとき
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // エディタ内ならプレイモード終了
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // 実行ファイルならアプリ終了
            Application.Quit();
#endif
        }
    }
}
