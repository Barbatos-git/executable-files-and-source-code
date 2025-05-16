using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // エフェクトが終了したら自分自身を削除するための関数
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
