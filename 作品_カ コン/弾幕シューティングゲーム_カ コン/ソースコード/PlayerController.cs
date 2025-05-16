using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    private float minX, maxX, minY, maxY;
    private CapsuleCollider2D cpc;
    public VirtualJoystick joystick;
    private GameObject ctrlGround;
    private BoxCollider2D ctrlgd;
    public GameObject shotPrefab;
    public Transform shotPoint;
    public float shotSpeed = 5f;
    private float shotRate = 0.1f;
    private float nextShotTime = 0f;
    private bool isAlive = true;
    //private bool isInvincible = false;
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        cpc = GetComponent<CapsuleCollider2D>();
        ctrlGround = GameObject.Find("Ctrlground");
        ctrlgd = ctrlGround.GetComponent<BoxCollider2D>();
        Camera mainCamera = Camera.main;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWideth = cameraHeight * mainCamera.aspect;
        // カメラサイズに応じて移動制限範囲を設定
        minX = mainCamera.transform.position.x - cameraWideth / 2 + cpc.size.x * 5f;
        maxX = mainCamera.transform.position.x + cameraWideth / 2 - cpc.size.x * 5f;
        minY = mainCamera.transform.position.y - cameraHeight / 2 + cpc.size.y * 5f + ctrlgd.size.y * 3.5f;
        maxY = mainCamera.transform.position.y + cameraHeight / 2 - cpc.size.y * 5f;
        // 死亡イベントを登録
        PlayerHpController playerHp = FindObjectOfType<PlayerHpController>();
        if (playerHp != null)
        {
            playerHp.onPlayerDeath.AddListener(OnPlayerDeath);
        }

        //InvincibleAndFlashController Invincible = FindObjectOfType<InvincibleAndFlashController>();
        //if (Invincible != null)
        //{
        //    Invincible.onPlayerInvincible.RemoveAllListeners();
        //    Invincible.onPlayerInvincible.AddListener(OnPlayerInvincible);
        //    Invincible.resetInvincible.AddListener(ResetPlayerInvincible);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        // キーボードとジョイスティックの両方に対応
        Vector2 keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 joystickInput = joystick.InputDirection;
        movement = joystickInput.magnitude > 0 ? joystickInput : keyboardInput;
        //movement.x = Input.GetAxis("Horizontal");
        //movement.y = Input.GetAxis("Vertical");
        // 移動中＆発射間隔経過 → 弾発射
        if (movement != Vector2.zero && Time.time >= nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + shotRate;
        }
    }

    // 弾を発射する処理
    void Shoot()
    {
        GameObject shot = Instantiate(shotPrefab, shotPoint.position, shotPoint.rotation);
        Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = shotPoint.up * shotSpeed;
        }

    }

    // 物理挙動の移動処理
    void FixedUpdate()
    {
        if (!isAlive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 targetPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        // 画面内に収める
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        rb.MovePosition(targetPosition);
        //Vector2 direction = joystick.InputDirection;
        //rb.velocity = direction * moveSpeed;
    }

    // 死亡時の動作
    private void OnPlayerDeath()
    {
        isAlive = false;
    }

    //private void OnPlayerInvincible()
    //{
    //    isInvincible = true;
    //    if (rb != null)
    //    {
    //        rb.simulated = false;
    //    }
    //}

    //private void ResetPlayerInvincible()
    //{
    //    isInvincible = false;
    //    if (rb != null)
    //    {
    //        rb.simulated = true;
    //    }
    //}
}
