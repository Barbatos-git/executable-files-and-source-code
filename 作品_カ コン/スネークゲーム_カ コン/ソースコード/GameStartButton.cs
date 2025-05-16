using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    public string gameSceneName = "GameScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        // 性別が選択されていることを確認
        if (!PlayerPrefs.HasKey("SelectedGender"))
        {
            return;
        }

        // ゲームシーンを読み込む
        SceneManager.LoadScene(gameSceneName);
    }
}
