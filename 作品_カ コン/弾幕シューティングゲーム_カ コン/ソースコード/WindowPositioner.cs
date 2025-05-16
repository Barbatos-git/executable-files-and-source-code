using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class WindowPositioner : MonoBehaviour
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    const uint SWP_NOZORDER = 0x0004;
    const uint SWP_NOACTIVATE = 0x0010;
    // Start is called before the first frame update
    void Start()
    {
        CenterWindow(414, 896);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ウィンドウを画面中央に移動させる処理
    void CenterWindow(int width, int height)
    {
        IntPtr hWnd = GetActiveWindow();
        if (hWnd == IntPtr.Zero) return;
        // 現在の画面解像度を取得
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        // 画面中央の座標を計算
        int x = (screenWidth - width) / 2;
        int y = (screenHeight - height) / 2;
        // ウィンドウを中央に移動
        SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
    }
}
