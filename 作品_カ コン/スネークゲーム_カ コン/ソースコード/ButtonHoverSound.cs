using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // マウスがボタンに乗ったときに呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip)
        {
            // 現在のカメラ位置から音を再生
            AudioSource.PlayClipAtPoint(hoverClip, Camera.main.transform.position);
        }
    }

    // ボタンがクリックされたときに呼ばれる
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
            // AudioManagerで設定されたクリック音を再生
            AudioManager.Instance.ClickSoundPlay();
    }
}
