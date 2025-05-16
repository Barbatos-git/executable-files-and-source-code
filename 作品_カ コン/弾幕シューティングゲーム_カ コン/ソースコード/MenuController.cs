using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenuUI;
    private bool isMenuOpen = false;
    public string nextSceneName = "";
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        settingsMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // メニューを開く
    public void OpenMenu()
    {
        if (isMenuOpen)
        {
            return;
        }
        isMenuOpen = true;
        settingsMenuUI.SetActive(true);  // UIを表示
        Time.timeScale = 0f;             // ゲームを一時停止
        audioSource.Pause();             // 音も停止
    }

    // メニューを閉じてゲーム再開
    public void ResumeGame()
    {
        if (!isMenuOpen)
        {
            return;
        }
        isMenuOpen = false;
        settingsMenuUI.SetActive(false);  // UIを非表示
        Time.timeScale = 1f;              // ゲームを一時停止
        audioSource.Play();               // 音も停止
    }

    // メニューからシーンを戻す（例：タイトルへ）
    public void LoadPreviousScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}
