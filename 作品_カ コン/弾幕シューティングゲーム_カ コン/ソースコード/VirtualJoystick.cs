using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, 
    IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public float joystickRadius = 100f;
    private Vector2 inputDirection = Vector2.zero;
    public Vector2 InputDirection => inputDirection;
    // Start is called before the first frame update
    void Start()
    {
        HideJoystick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // タップ開始時の処理
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 position;
        // タッチ位置をローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out position
            );
        // 背景の位置をタップ位置へ
        joystickBackground.anchoredPosition = position;
        ShowJoystick();       // 表示する
        OnDrag(eventData);    // 即時ドラッグ処理も行う
    }

    // タップ終了時の処理（リセット）
    public void OnPointerUp(PointerEventData eventData)
    {
        inputDirection = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
        HideJoystick();
    }

    // ドラッグ中の処理（方向入力）
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 positoin;
        // 背景基準で座標取得
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground, 
            eventData.position, 
            eventData.pressEventCamera, 
            out positoin);
        // 半径以内に制限
        positoin = Vector2.ClampMagnitude(positoin, joystickRadius);
        // ハンドル位置を更新
        joystickHandle.anchoredPosition = positoin;
        // 入力方向を更新（正規化）
        inputDirection = positoin / joystickRadius;
    }

    // ジョイスティック表示
    private void ShowJoystick()
    {
        joystickBackground.gameObject.SetActive(true);
        joystickHandle.gameObject.SetActive(true);
    }

    // ジョイスティック非表示
    private void HideJoystick()
    {
        joystickBackground.gameObject.SetActive(false);
        joystickHandle.gameObject.SetActive(false);
    }
}
