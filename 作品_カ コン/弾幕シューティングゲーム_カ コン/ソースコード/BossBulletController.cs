using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    public GameObject[] bulletPrefab;
    public float speed = 2f;
    public float speedPro = 5f;
    public Transform barragePointUnder;
    public Transform barragePointCenter;
    public Transform barragePoint;
    public Transform[] straightPoint;
    public Transform[] TrackingPoint;
    //public float barrageRate = 0.5f;
    public float spreadFactor = 1.5f;
    public float fireDuration = 20f;
    public float intervalBetweenBullets = 10f;
    private InvincibleAndFlashController playerInvincibleController;
    private GameObject parentBullet;
    private GameObject player;

    private List<int> barrageSequence = new List<int>();
    private bool isFiring = false;
    private List<GameObject> activeBullets = new List<GameObject>();

    //private BossHpBarController hpBarController;
    //private bool hasDied = false;
    public bool isMovable = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInvincibleController = FindObjectOfType<InvincibleAndFlashController>();
        //hpBarController = FindObjectOfType<BossHpBarController>();
        //if (hpBarController != null)
        //{
        //    hpBarController.onHpZero.AddListener(OnBossDeath);
        //}

        StartCoroutine(DelayStartBarrage());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 弾幕開始までの遅延
    IEnumerator DelayStartBarrage()
    {
        yield return new WaitForSeconds(5f);
        StartNewBarrageCycle();
    }

    // 弾幕サイクルの開始（ランダムにパターン選択）
    void StartNewBarrageCycle()
    {
        barrageSequence.Clear();
        for (int i = 0; i < bulletPrefab.Length; i++)
        {
            barrageSequence.Add(i);
        }
        StartCoroutine(StartRandomBarrage());
    }

    // ランダムな弾幕パターンを順に実行
    IEnumerator StartRandomBarrage()
    {
        while (barrageSequence.Count > 0)
        {
            int randomIndex = Random.Range(0, barrageSequence.Count);
            int barrageType = barrageSequence[randomIndex];
            barrageSequence.RemoveAt(randomIndex);
            StartCoroutine(FireBarrage(barrageType));
            yield return new WaitForSeconds(fireDuration + intervalBetweenBullets);
        }

        StartNewBarrageCycle();
    }

    // 指定された弾幕タイプを実行
    IEnumerator FireBarrage(int barrageType)
    {
        DestroyActiveBullets();
        isFiring = true;
        switch (barrageType)
        { 
            case 0:
                StartCoroutine(FanBullet());
                break;
            case 1:
                StartCoroutine(StraightBullet());
                break;
            case 2:
                StartCoroutine(SpiralBullet());
                break;
            case 3:
                StartCoroutine(TrackingBullet());
                break;
            case 4:
                StartCoroutine(ZigZagBullet());
                break;
            case 5:
                StartCoroutine(SpreadWaveBullet());
                break;
            case 6:
                StartCoroutine(RingBullt());
                break;
            case 7:
                StartCoroutine(SplittingBullet());
                break;
        }
        yield return new WaitForSeconds(fireDuration);
        isFiring = false;
    }

    // 現在の弾を全て破棄
    void DestroyActiveBullets()
    {
        foreach (GameObject bullet in activeBullets)
        {
            Destroy(bullet);
        }
        activeBullets.Clear();
    }

    // 扇状に弾を発射
    IEnumerator FanBullet()
    {
        int bulletCount = 8;
        float angleRange = 120f;
        float adjustedAngleRange = angleRange * spreadFactor;
        while (isFiring && isMovable)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = -adjustedAngleRange / 2 + adjustedAngleRange * i / (bulletCount - 1);
                GameObject bullet = Instantiate(bulletPrefab[0], barragePointUnder.position, Quaternion.identity);
                Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad));
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
                activeBullets.Add(bullet);
                // プレイヤーへの衝突イベント登録
                BarrageController barrage = bullet.GetComponent<BarrageController>();
                if (barrage != null && playerInvincibleController != null)
                {
                    barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // 下方向に直進弾を発射
    IEnumerator StraightBullet()
    {
        while (isFiring && isMovable)
        {
            foreach (Transform point in straightPoint)
            {
                GameObject bullet = Instantiate(bulletPrefab[1], point.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * speedPro;
                activeBullets.Add(bullet);

                BarrageController barrage = bullet.GetComponent<BarrageController>();
                if (barrage != null && playerInvincibleController != null)
                {
                    barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                }
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    // 渦巻き状に弾を発射
    IEnumerator SpiralBullet()
    {
        float angle = 0f;
        int spiralCount = 3;
        float spiralOffset = 360f / spiralCount;
        while (isFiring && isMovable)
        {
            for (int i = 0; i < spiralCount; i++)
            {
                float currentAngle = angle + i * spiralOffset;
                float radians = currentAngle * Mathf.Rad2Deg;
                Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
                GameObject bullet = Instantiate(bulletPrefab[2], barragePoint.position, Quaternion.Euler(0, 0, angle));
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
                activeBullets.Add(bullet);

                BarrageController barrage = bullet.GetComponent<BarrageController>();
                if (barrage != null && playerInvincibleController != null)
                {
                    barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                }
            }
            angle += 10f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // プレイヤーを追尾する弾
    IEnumerator TrackingBullet()
    {
        while (isFiring && isMovable)
        {
            if (player == null)
            {
                yield return null;
            }
            else
            {
                foreach (Transform point in TrackingPoint) 
                {
                    Vector2 direction = (player.transform.position - point.position).normalized;
                    GameObject bullet = Instantiate(bulletPrefab[3], point.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
                    activeBullets.Add(bullet);

                    BarrageController barrage = bullet.GetComponent<BarrageController>();
                    if (barrage != null && playerInvincibleController != null)
                    {
                        barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                    }
                }
                yield return new WaitForSeconds(1.8f);
            }
        }
    }

    // ジグザグに動く弾
    IEnumerator ZigZagBullet()
    {
        float horizontalDistance = 5f;
        float verticalStep = -5f;
        int bulletCount = 3;
        float spacing = 3f;
        bool moveRight = true;
        while (isFiring && isMovable)
        {
            for (int i = -bulletCount/2; i <= bulletCount/2; i++)
            {
                Vector3 offset = new Vector3(i * spacing, 0, 0);
                GameObject bullet = Instantiate(bulletPrefab[4], barragePointUnder.position + offset, Quaternion.identity);
                StartCoroutine(MoveInZigZag(bullet, horizontalDistance, verticalStep, speed, moveRight));
                activeBullets.Add(bullet);

                BarrageController barrage = bullet.GetComponent<BarrageController>();
                if (barrage != null && playerInvincibleController != null)
                {
                    barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                }
            }

            moveRight = !moveRight;
            yield return new WaitForSeconds(1.5f);
        }
    }

    // 弾をジグザグに動かす補助コルーチン
    IEnumerator MoveInZigZag(GameObject bullet, float horizontalDistance, float verticalStep, float speed, bool initialDirection)
    {
        Vector3 startPosition = bullet.transform.position;
        Vector3 targetPosition = startPosition;
        bool moveRight = initialDirection;
        while (bullet != null)
        {
            targetPosition.x += moveRight ? horizontalDistance : -horizontalDistance;
            while (bullet != null && Vector3.Distance(bullet.transform.position, targetPosition) > 0.1f)
            {
                bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            targetPosition.y += verticalStep;
            while (bullet != null && Vector3.Distance(bullet.transform.position, targetPosition) > 0.1f)
            {
                bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            moveRight = !moveRight;
        }
    }

    // 波状に広がる弾を発射
    IEnumerator SpreadWaveBullet()
    {
        while (isFiring && isMovable)
        {
            float angeRange = 120f;
            int bulletCount = 5;
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = -angeRange / 2 + angeRange * i / (bulletCount - 1);
                GameObject bullet = Instantiate(bulletPrefab[5], barragePointUnder.position, Quaternion.identity);
                Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad));
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
                activeBullets.Add(bullet);

                BarrageController barrage = bullet.GetComponent<BarrageController>();
                if (barrage != null && playerInvincibleController != null)
                {
                    barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // 円状に弾を発射（リング状）
    IEnumerator RingBullt()
    {
        int bulletCount = 15;
        float radius = 0.5f;
        while (isFiring && isMovable)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * (360f / bulletCount);
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector3 spawnPosition = barragePointCenter.position + (Vector3)(direction * radius);
                GameObject bullet = Instantiate(bulletPrefab[6], spawnPosition, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
                activeBullets.Add(bullet);

                BarrageController barrage = bullet.GetComponent<BarrageController>();
                if (barrage != null && playerInvincibleController != null)
                {
                    barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // 一定距離進んだ後に分裂する弾
    IEnumerator SplittingBullet()
    {
        while (isFiring && isMovable)
        {
            // 既に親弾が存在する間は待機
            while (parentBullet != null)
            {
                yield return null;
            }
            // 親弾の生成と発射
            parentBullet = Instantiate(bulletPrefab[7], barragePointUnder.position, Quaternion.identity);
            parentBullet.transform.localScale *= 2f;

            Vector2 direction = Vector2.down;
            Rigidbody2D rb = parentBullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * speedPro;
            StartCoroutine(HandleBulletSplit(parentBullet));

            BarrageController parentBarrage = parentBullet.GetComponent<BarrageController>();
            if (parentBarrage != null && playerInvincibleController != null)
            {
                parentBarrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    // 一定距離移動後に分裂処理を行う
    IEnumerator HandleBulletSplit(GameObject parentBullet)
    {
        float splitDistance = 7f;
        Vector3 startPosition = barragePointUnder.position;
        while (parentBullet != null && Vector3.Distance(startPosition, parentBullet.transform.position) < splitDistance)
        {
            yield return null;
        }
        if (parentBullet == null)
        {
            yield break;
        }
        SplitIntoBullets(parentBullet.transform.position);
        Destroy(parentBullet);
    }

    // 弾を複数方向へ分裂
    private void SplitIntoBullets(Vector3 position)
    {
        int bulletCount = 10;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * 360f / bulletCount;
            Vector2 splitDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject bullet = Instantiate(bulletPrefab[7], parentBullet.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = splitDirection * speed;
            activeBullets.Add(bullet);

            BarrageController barrage = bullet.GetComponent<BarrageController>();
            if (barrage != null && playerInvincibleController != null)
            {
                barrage.isCollision.AddListener(playerInvincibleController.CollisionWithBarrage);
            }
        }
    }

    //private void OnBossDeath()
    //{
    //    if (hasDied) { return; }
    //    hasDied = true;
    //}
}
