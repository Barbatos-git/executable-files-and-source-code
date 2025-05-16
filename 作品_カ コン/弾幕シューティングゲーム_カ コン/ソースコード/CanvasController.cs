using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();
        float aspectRatio = (float)Screen.width / Screen.height;
        // 横長画面なら幅基準（matchWidthOrHeight = 0）、縦長画面なら高さ基準（= 1）
        scaler.matchWidthOrHeight = aspectRatio > 1 ? 0 : 1;
        // 安全領域（ノッチやUI被り回避）の取得
        Rect safeArea = Screen.safeArea;
        // RectTransformを取得して、安全領域にフィットさせる
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = safeArea.position;
        rectTransform.offsetMax = safeArea.position + safeArea.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
