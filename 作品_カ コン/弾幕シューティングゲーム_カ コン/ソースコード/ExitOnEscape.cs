using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitOnEscape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EndGame();
    }

    // ゲーム終了処理
    private void EndGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // 実行ファイルを終了
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
