using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public string nextSceneName = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // スペースキーが押されたらシーンを切り替える
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextScene();
        }
    }

    // 次のシーンを読み込む
    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
