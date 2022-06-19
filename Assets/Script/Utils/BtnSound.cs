using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BtnSound : MonoBehaviour
{
    public AudioSource audioSource;
    public void OnClick()
    {
        SoundManager.Instance.FXSoundPlay(audioSource);
    }
}
