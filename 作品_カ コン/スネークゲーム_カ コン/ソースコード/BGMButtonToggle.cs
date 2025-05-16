using UnityEngine;
using UnityEngine.UI;

public class BGMButtonToggle : MonoBehaviour
{
    public Sprite bgmOnIcon;
    public Sprite bgmOffIcon;
    private Image iconImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        iconImage=transform.Find("Icon").GetComponent<Image>();
        UpdateIcon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ボタンがクリックされたときに呼ばれる処理
    public void OnClickToggle()
    {
        // BGMの状態を反転
        bool isBGMOn = !AudioManager.Instance.IsBGMEnabled();
        // AudioManagerに新しい状態を設定
        AudioManager.Instance.SetBGMEnabled(isBGMOn);
        // アイコンを更新
        UpdateIcon();
    }

    // 現在のBGM状態に基づいてアイコンを変更
    private void UpdateIcon()
    {
        if (iconImage != null)
        {
            iconImage.sprite = AudioManager.Instance.IsBGMEnabled() ? bgmOnIcon : bgmOffIcon;
        }
    }
}
