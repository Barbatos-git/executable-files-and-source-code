using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletCount = 20;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 弾を一斉に円状に発射する関数
    public void Fire()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            // 弾の角度を等間隔で計算
            float angle = i * (360f / bulletCount);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // 方向ベクトルを角度から算出
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            bullet.GetComponent<Rigidbody>().linearVelocity = direction * speed;
        }
    }
}
