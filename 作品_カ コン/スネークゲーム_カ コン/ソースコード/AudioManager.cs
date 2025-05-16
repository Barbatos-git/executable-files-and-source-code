using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip bgmClip;
    public AudioClip deadClip;
    private const string BGM_PREF_KEY = "BGM_ENABLED";

    public AudioClip clickSound;
    public AudioSource clickSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        // まだインスタンスが存在しない場合、自分を設定
        if (Instance == null) 
        {
            Instance = this;
            // シーンをまたいでも破棄されないようにする
            DontDestroyOnLoad(gameObject);
            // BGM設定を初期化
            InitBGMSetting();
            // クリック音再生用AudioSourceを追加
            clickSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            // 既に存在する場合は自分を削除
            Destroy(gameObject);
        }
    }

    // BGM設定の初期化処理
    void InitBGMSetting()
    {
        // BGMが有効かどうかをPlayerPrefsから取得（デフォルトは有効）
        bool isEnabled = PlayerPrefs.GetInt(BGM_PREF_KEY, 1) == 1;
        // BGMクリップとループ設定
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        // 有効な場合は再生
        if (isEnabled)
        {
            bgmSource.Play();
        }
    }

    // BGMのオン／オフを切り替える関数
    public void SetBGMEnabled(bool enabled)
    {
        if (enabled)
        {
            if (!bgmSource.isPlaying)
            {
                bgmSource.Play();
            }
        }
        else
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }
        // 設定を保存
        PlayerPrefs.SetInt(BGM_PREF_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    // BGMが有効かどうかを取得する関数
    public bool IsBGMEnabled()
    {
        return PlayerPrefs.GetInt(BGM_PREF_KEY, 1) == 1;
    }

    // 死亡時のBGMと効果音の切り替え再生
    public void PlayDeadMusic()
    {
        // 現在のBGMを停止
        bgmSource.Stop();
        // 死亡効果音が設定されていれば再生
        if (deadClip != null)
        {
            sfxSource.PlayOneShot(deadClip);
        }
    }

    // クリック音を再生する関数（ボタンに設定する用途など）
    public void ClickSoundPlay()
    {
        if (clickSound != null)
        {
            clickSource.PlayOneShot(clickSound);
        }
    }
}
