using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAppear : MonoBehaviour
{
    public Transform boss;
    public float appearSpeed = 3f;
    public Vector3 targetPosition;
    public GameObject bossHpBar;
    private BossController bossController;
    public UnityEvent bossHpActive;
    // Start is called before the first frame update
    void Start()
    {
        // 初期位置を画面外（上）に設定
        Vector3 startPosition = new Vector3(0, 30f, 0);
        boss.position = startPosition;
        // ボスの移動を一時停止
        bossController = FindObjectOfType<BossController>();
        if (bossController != null)
        {
            bossController.isMovable = false;
        }

        //StartCoroutine(MoveBossToPosition());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ボスが上からゆっくりとターゲット位置へ移動する演出
    IEnumerator MoveBossToPosition()
    {
        // 少し待機してから開始
        yield return new WaitForSeconds(1f);
        // 指定座標まで移動
        while (Vector3.Distance(boss.position, targetPosition) > 0.1f)
        {
            boss.position = Vector3.MoveTowards(boss.position, targetPosition, appearSpeed * Time.deltaTime);
            yield return null;
        }
        // 位置をぴったり合わせる
        boss.position = targetPosition;
        // HPバーを表示
        bossHpBar.SetActive(true);
        bossHpActive?.Invoke();
        // ボスの移動を再開
        if (bossController != null)
        {
            bossController.isMovable = true;
        }
    }

    // 外部から呼び出して、ボスの登場アニメーションを開始する
    public void MoveBoss()
    {
        StartCoroutine(MoveBossToPosition());
    }
}
