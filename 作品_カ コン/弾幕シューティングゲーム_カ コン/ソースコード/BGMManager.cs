using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> bgmClips;

    private List<AudioClip> remainingClips;
    private float fadeDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        remainingClips = new List<AudioClip>(bgmClips);
        PlayRandomBGM();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ランダムなBGMを選んで再生する
    private void PlayRandomBGM()
    {
        // 再生待ちリストが空なら再度初期化
        if (remainingClips.Count == 0)
        {
            remainingClips = new List<AudioClip>(bgmClips);
        }
        // ランダムに一曲選んで再生
        int randomIndex = UnityEngine.Random.Range(0, remainingClips.Count);
        AudioClip nextClip = remainingClips[randomIndex];
        remainingClips.RemoveAt(randomIndex);
        // フェードアウト後、新しいBGMを再生
        StartCoroutine(FadeOutAndPlayNext(nextClip));
    }

    // 現在のBGMをフェードアウトし、新しい曲を再生しながらフェードイン
    private IEnumerator FadeOutAndPlayNext(AudioClip newClip)
    {
        float startVolume = audioSource.volume;
        // フェードアウト処理
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        // 曲を切り替えて再生
        audioSource.clip = newClip;
        audioSource.Play();
        // フェードイン処理
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        // 曲が終わる直前に次の曲を再生
        Invoke(nameof(PlayRandomBGM), newClip.length - fadeDuration);
    }
}
