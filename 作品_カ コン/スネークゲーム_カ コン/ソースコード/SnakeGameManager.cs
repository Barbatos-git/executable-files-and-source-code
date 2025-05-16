using UnityEngine;
using System.Collections;

public class SnakeGameManager : MonoBehaviour
{
    public static SnakeGameManager Instance { get; private set; }

    public GameObject maleCharacter;
    public GameObject femaleCharacter;
    public SnakeHeadController headController;
    public SnakeFollowManager followManager;
    public AppleManager appleManager;

    public float speedIncrease = 0.5f;
    public float maxSpeed = 20f;

    public GameObject applePrefab;
    public GameObject mapBounds;
    private BoxCollider2D boxCollider;
    public Vector2 spawnRangeMin;
    public Vector2 spawnRangeMax;
    private CircleCollider2D circleCollider;

    public GameObject deathEffectPrefab;
    public GameOverUI gameOverUI;

    public int currentScore = 0;
    public int meetCount = 0;
    public int scorePerMeet = 10;

    void Start()
    {
        // 性別に応じて該当キャラをアクティブ化
        string selectedGender = PlayerPrefs.GetString("SelectedGender", "Male");

        maleCharacter.SetActive(false);
        femaleCharacter.SetActive(false);

        if (selectedGender == "Male")
        {
            maleCharacter.SetActive(true);
            headController = maleCharacter.GetComponent<SnakeHeadController>();
            followManager = maleCharacter.GetComponent<SnakeFollowManager>();
        }
        else
        {
            femaleCharacter.SetActive(true);
            headController = femaleCharacter.GetComponent<SnakeHeadController>();
            followManager = femaleCharacter.GetComponent<SnakeFollowManager>();
        }

        boxCollider = mapBounds.GetComponent<BoxCollider2D>();
        circleCollider = applePrefab.GetComponent<CircleCollider2D>();
        // リンゴの生成範囲を算出
        spawnRangeMin = new Vector2(
            -boxCollider.size.x / 2 + circleCollider.radius / 2, 
            -boxCollider.size.y / 2 + circleCollider.radius / 2
            );
        spawnRangeMax = new Vector2(
            boxCollider.size.x / 2 - circleCollider.radius / 2, 
            boxCollider.size.y / 2 - circleCollider.radius / 2
            );

        SpawnApple();  // 初回リンゴ生成
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;        
    }

    public void OnAppleEaten()
    {
        meetCount++;
        currentScore += scorePerMeet;

        followManager.AddFollower();
        IncreaseSpeed();
        SpawnApple();
    }

    public void SpawnApple()
    {
        Vector2 pos = new Vector2(
            Random.Range(spawnRangeMin.x, spawnRangeMax.x),
            Random.Range(spawnRangeMin.y, spawnRangeMax.y)
        );
        Instantiate(applePrefab, pos, Quaternion.identity);
    }

    public void IncreaseSpeed()
    {
        headController.moveSpeed = Mathf.Min(headController.moveSpeed + speedIncrease, maxSpeed);
    }

    public void OnPlayerDied()
    {
        // 最終スコアとリンゴ数を保存
        PlayerPrefs.SetInt("FinalScore", currentScore);
        PlayerPrefs.SetInt("FinalMeet", meetCount);

        StartCoroutine(HeadDeath());
    }

    private IEnumerator HeadDeath()
    {
        // 1. ヘッドの動きを停止
        headController.StopMovement();

        // 2. エフェクト：ヘッド
        var headEffect = Instantiate(deathEffectPrefab, headController.transform.position, Quaternion.identity);
        AssignTilemapsToEffect(headEffect);

        // 3. エフェクト：ボディ
        foreach (var follower in followManager.GetFollowers())
        {
            var bodyEffect = Instantiate(deathEffectPrefab, follower.position, Quaternion.identity);
            AssignTilemapsToEffect(bodyEffect);
            follower.gameObject.SetActive(false);
        }

        // 4. ヘッドを非表示にする
        headController.gameObject.SetActive(false);

        // 5. GameOver BGM を再生
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDeadMusic();
        }

        // 6. 少し遅れて GameOver UI を表示
        yield return new WaitForSeconds(1.5f);
        gameOverUI.ShowGameOver();
    }

    void AssignTilemapsToEffect(GameObject effect)
    {
        var surfaceChecker = effect.GetComponent<DeadEffectSurfaceCheck>();
        if (surfaceChecker != null)
        {
            surfaceChecker.mapBounds = boxCollider;
        }
    }
}
