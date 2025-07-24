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
        ChangeBackGroundMusic(musicClip);//musicClip���� �����Ѵ�.
    }

    public void ChangeBackGroundMusic(AudioClip clip)
    {
        musicAudioSource.Stop();//���� Ʋ���ִ°� ���߰�
        musicAudioSource.clip = clip;//�ٲٰ�
        musicAudioSource.Play();//����Ѵ�.
    }

    public static void PlayClip(AudioClip clip)
    {
        SoundSource obj = Instantiate(instance.soundSourcePrefab);//���� �ҽ���� �������� �����ϰ�
        SoundSource soundSource = obj.GetComponent<SoundSource>();//���⿡ ���� SoundSource�� ������ ��
        soundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance);//����Ѵ�.
    }
}
