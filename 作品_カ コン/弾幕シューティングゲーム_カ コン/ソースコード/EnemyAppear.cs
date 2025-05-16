using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAppear : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform spawnAreaLeft;
    public Transform spawnAreaRight;
    public int initiaEnemyCount = 5;
    public int secondEnemyCount = 3;
    private List<GameObject> activeEnemies = new List<GameObject>();
    public Vector3 enemyPoint;
    private InvincibleAndFlashController playerInvincibleController;
    private GameObject enemy;
    private bool isBossAppear = false;
    private bool isSecond = false;
    private BossAppear bossAppear;
    // Start is called before the first frame update
    void Start()
    {
        bossAppear = FindObjectOfType<BossAppear>();
        playerInvincibleController = FindObjectOfType<InvincibleAndFlashController>();
        // 第1波の敵を出現させる
        StartCoroutine(SpawnEnemies(enemyPrefabs[0], initiaEnemyCount));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 敵を一定数生成するコルーチン
    IEnumerator SpawnEnemies(GameObject enemyPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 左右ランダムで出現エリアを決定
            bool spawnFromLeft = Random.value > 0.5f;
            Transform spawnArea = spawnFromLeft ? spawnAreaLeft : spawnAreaRight;
            // エリア内ランダム座標を計算
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnArea.position.x - spawnArea.localPosition.x / 2, spawnArea.position.x + spawnArea.localPosition.x / 2),
                spawnArea.position.y,
                spawnArea.position.z
                );
            // 敵を生成
            enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            // ターゲット位置の設定と死亡イベント登録
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetTargetPosition(enemyPoint);
                enemyController.onEnemyDeath += OnEnemyDeath;
            }
            // HPや爆発管理、プレイヤーとの衝突対応
            EnemyExplosionAndHpController enemyExplosionAndHp = enemy.GetComponent<EnemyExplosionAndHpController>();
            if (enemyExplosionAndHp != null && playerInvincibleController != null)
            {
                enemyExplosionAndHp.isCollision.AddListener(playerInvincibleController.CollisionWithEnemy);
                enemyExplosionAndHp.Initialize();
            }

            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(1f);
        }
    }

    // 敵が死んだときに呼ばれるイベント処理
    private void OnEnemyDeath(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
        // 第1波の全滅時に第2波出現
        if (activeEnemies.Count == 0 && !isSecond)
        {
            StartCoroutine(SpawnSceondWave());
        }
        // 第2波全滅かつボス出現状態ならボス戦開始
        if (isBossAppear && activeEnemies.Count == 0 && isSecond)
        {
            bossAppear.MoveBoss();
            BossBulletController bossBulletController = FindObjectOfType<BossBulletController>();
            bossBulletController.isMovable = true;
        }
    }

    // 第2波の敵を出現させる
    IEnumerator SpawnSceondWave()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(SpawnEnemies(enemyPrefabs[1], secondEnemyCount));
        isBossAppear = true;
        isSecond = true;
    }
}
