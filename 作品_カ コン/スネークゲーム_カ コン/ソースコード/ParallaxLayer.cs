using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float speed = 0.1f;
    private float width;
    float reWidth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        reWidth = width - 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        // 左に移動する
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 左端を越えたら右端に移動する
        if (transform.position.x < -reWidth)
        {
            transform.position = new Vector3(reWidth, transform.position.y, transform.position.z);
        }
    }
}
