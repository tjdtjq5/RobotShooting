using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Range(0,1)] public float t_volume;
    [Range(0, 1)] public float bgm_volume;
    [Range(0, 1)] public float fx_volume;

    AudioSource c_bgmAudio;

    public AudioSource btnAudio;
    public AudioSource enemyDeadAudio;

    private void Awake()
    {
        t_volume = PlayerPrefs.HasKey("t_volume") ? PlayerPrefs.GetFloat("t_volume") : 1;
        bgm_volume = PlayerPrefs.HasKey("bgm_volume") ? PlayerPrefs.GetFloat("bgm_volume") : 1;
        fx_volume = PlayerPrefs.HasKey("fx_volume") ? PlayerPrefs.GetFloat("fx_volume") : 1;
    }

    [Button("TotalVolumeSetting")]
    public void TotalVolumeSetting(float _t_volume)
    {
        t_volume = _t_volume;
        PlayerPrefs.SetFloat("t_volume", _t_volume);
        if (c_bgmAudio != null)
        {
            BGMSoundPlay(c_bgmAudio);
        }
    }
    [Button("BGMVolumeSetting")]
    public void BGMVolumeSetting(float _bgm_volume)
    {
        bgm_volume = _bgm_volume;
        PlayerPrefs.SetFloat("bgm_volume", _bgm_volume);
        if (c_bgmAudio != null)
        {
            BGMSoundPlay(c_bgmAudio);
        }
    }
    [Button("FxVolumeSetting")]
    public void FxVolumeSetting(float _fx_volume)
    {
        fx_volume = _fx_volume;
        PlayerPrefs.SetFloat("fx_volume", _fx_volume);
    }

    public void BGMSoundPlay(AudioSource _audioSource)
    {
        if (c_bgmAudio != null)
        {
            c_bgmAudio.Stop();
        }
        _audioSource.volume = t_volume * bgm_volume;
        _audioSource.Play();
        c_bgmAudio = _audioSource;
    }
    public void FXSoundPlay(AudioSource _audioSource)
    {
        _audioSource.volume = t_volume * fx_volume;
        _audioSource.Play();
    }

    public void BtnPlay()
    {
        btnAudio.volume = t_volume * fx_volume;
        btnAudio.Play();
    }
    public void EnemyDeadPlay()
    {
        enemyDeadAudio.volume = t_volume * fx_volume;
        enemyDeadAudio.Play();
    }
}
