using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource _audioSource;
    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance)
    {
        if(_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        CancelInvoke();
        _audioSource.clip = clip;
        _audioSource.volume = soundEffectVolume;
        _audioSource.pitch = 1.0f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);
        _audioSource.Play();//�̰����� �� ���ؼ� ���!

        Invoke("Disable", clip.length + 2);//�׸��� Disable�� ����ð����� 2�ʵڿ� ȣ���Ѵ�.
    }

    public void Disable()
    {
        _audioSource?.Stop();//Ȥ�� ������ ���������� ���߰�
        Destroy(this.gameObject);//���ش�.
    }
}
