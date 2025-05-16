using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortingByY : MonoBehaviour
{
    private SpriteRenderer sr;
    public int baseOrder = 1000;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // 画面下に行くほど sortingOrder が大きくなる（差を強調するため *100）
        sr.sortingOrder = baseOrder + Mathf.RoundToInt(-transform.position.y * 100);
    }
}
