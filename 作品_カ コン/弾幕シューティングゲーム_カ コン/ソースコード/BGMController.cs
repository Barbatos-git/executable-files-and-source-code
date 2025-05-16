using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    public AudioSource audioSource;
    public Button bgmButton;
    public Sprite bgmOnSprite;
    public Sprite bgmOffSprite;
    private bool isBGMOn = true;
    // Start is called before the first frame update
    void Start()
    {
        // ボタンにクリックイベントを登録
        bgmButton.onClick.AddListener(ToggleBGM);

        UpdateButtonImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // BGMのON/OFFを切り替える
    void ToggleBGM()
    {
        isBGMOn = !isBGMOn;
        // BGMを再生または一時停止
        if (isBGMOn)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
        // ボタン画像を変更
        UpdateButtonImage();
    }

    // 現在のBGM状態に応じてボタン画像を更新
    void UpdateButtonImage()
    {
        if (isBGMOn)
        {
            bgmButton.image.sprite = bgmOnSprite;
        }
        else
        {
            bgmButton.image.sprite = bgmOffSprite;
        }
    }
}
