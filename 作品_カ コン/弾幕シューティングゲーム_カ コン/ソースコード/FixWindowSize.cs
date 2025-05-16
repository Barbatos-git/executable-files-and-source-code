using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FixWindowSize : MonoBehaviour
{
    // Windows API を使ってウィンドウ操作するための関数
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    private static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    // ウィンドウ位置設定フラグ（Zオーダー変更無し、非アクティブ化無し）
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_NOACTIVATE = 0x0010;
    // Windowsのウィンドウ座標構造体
    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left, top, right, bottom;
    }
    // Start is called before the first frame update
    void Start()
    {
        // 開始時にウィンドウサイズを固定する
        FixWindow(414, 896);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 指定サイズにウィンドウを調整
    private void FixWindow(int targetWidth, int targetHeight)
    {
        IntPtr hWnd = GetActiveWindow();  // 現在のウィンドウを取得
        if (hWnd == IntPtr.Zero) return;

        RECT rect;
        GetWindowRect(hWnd, out rect);  // 現在のウィンドウサイズを取得

        int windowWidth = rect.right - rect.left;
        int windowHeight = rect.bottom - rect.top;
        // 境界分を補正して目標サイズに調整
        int extraWidth = windowWidth - targetWidth;
        int extraHeight = windowHeight - targetHeight;
        // ウィンドウサイズと位置を設定
        SetWindowPos(hWnd, IntPtr.Zero, rect.left, rect.top, targetWidth - extraWidth, targetHeight - extraHeight, SWP_NOZORDER | SWP_NOACTIVATE);
    }
}
