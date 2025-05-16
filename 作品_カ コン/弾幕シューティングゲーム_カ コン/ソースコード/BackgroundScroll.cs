using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 下方向に背景をスクロールする（時間に依存して滑らかに移動）
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        // 背景が特定位置（-40f）を超えたら、上に戻してループさせる
        if (transform.position.y < -40f)
        {
            transform.position = new Vector3(transform.position.x, 40f, transform.position.z);
        }
    }
}
