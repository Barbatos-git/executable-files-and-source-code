using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class DeadEffectSurfaceCheck : MonoBehaviour
{
    public Collider2D mapBounds;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DelayedSurfaceCheck());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // エフェクトの出現位置に応じて地面 or 水上アニメーションを切り替える
    private IEnumerator DelayedSurfaceCheck()
    {
        // 1フレーム待機して、位置が安定するのを待つ
        yield return new WaitForEndOfFrame(); 

        // 位置補正。例：Y軸に0.16f（必要に応じて調整）
        Vector2 checkPoint = (Vector2)transform.position + Vector2.up * 0.5f;

        bool isOnGround = mapBounds != null && mapBounds.OverlapPoint(checkPoint);

        if (!isOnGround)
        {
            // 水上のエフェクトアニメーションを設定
            animator.SetBool("isOnWater", true); 
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 checkPoint = transform.position + Vector3.up * 0.5f;
        // 死亡エフェクトの判定ポイントを表示
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(checkPoint, 0.05f);

        // 地面のコライダー範囲を表示
        if (mapBounds != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            Bounds bounds = mapBounds.bounds;
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
    }
#endif
}
