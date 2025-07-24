using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField][Range(0.0f, 1.0f)] private float soundEffectVolume;
    [SerializeField][Range(0.0f, 1.0f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0.0f, 1.0f)] private float musicVolume;

    private AudioSource musicAudioSource;
    public AudioClip musicClip;

    public SoundSource soundSourcePrefab;

    private void Awake()
    {
        instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;
    }

    private void Start()
    {
        ChangeBackGroundMusic(musicClip);//musicClip으로 변경한다.
    }

    public void ChangeBackGroundMusic(AudioClip clip)
    {
        musicAudioSource.Stop();//지금 틀고있는걸 멈추고
        musicAudioSource.clip = clip;//바꾸고
        musicAudioSource.Play();//재생한다.
    }

    public static void PlayClip(AudioClip clip)
    {
        SoundSource obj = Instantiate(instance.soundSourcePrefab);//사운드 소스라는 프리팹을 생성하고
        SoundSource soundSource = obj.GetComponent<SoundSource>();//여기에 붙은 SoundSource를 가져온 후
        soundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance);//재생한다.
    }
}
