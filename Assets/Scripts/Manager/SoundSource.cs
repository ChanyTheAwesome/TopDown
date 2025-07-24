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
        _audioSource.Play();//이것저것 다 정해서 재생!

        Invoke("Disable", clip.length + 2);//그리고 Disable을 재생시간보다 2초뒤에 호출한다.
    }

    public void Disable()
    {
        _audioSource?.Stop();//혹시 아직도 켜져있으면 멈추고
        Destroy(this.gameObject);//없앤다.
    }
}
