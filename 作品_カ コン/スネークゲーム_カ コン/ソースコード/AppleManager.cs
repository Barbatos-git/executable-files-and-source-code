using UnityEngine;

public class AppleManager : MonoBehaviour
{
    //当たり判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Head"))
        {
            SnakeGameManager.Instance.OnAppleEaten();
            Destroy(gameObject);
        }
    }
}
