using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public float bossHp;
    public float gameTime;
    public int playerHp;
    public int maxPlayerHp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // シーン切り替え時に破棄されないようにする
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // 重複があれば破棄
        }
    }
}
