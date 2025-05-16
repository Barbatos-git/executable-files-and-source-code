using UnityEngine;

public class MapBoundsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ヘッドがこのコライダーから外に出たときに呼ばれる
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Head"))
        {
            // ゲームオーバー処理を呼び出し
            SnakeGameManager.Instance.OnPlayerDied();
        }
    }
}
