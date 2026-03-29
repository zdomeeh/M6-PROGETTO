using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;      
    [SerializeField] private AudioClip _coinClip;          
    [SerializeField] private AudioClip _jumpClip;          
    [SerializeField] private AudioClip _damageClip;   

    // audio quando raccoglie la moneta
    public void PlayCoin()
    {
        if (_audioSource != null && _coinClip != null)
            _audioSource.PlayOneShot(_coinClip);
    }

    // audio quando salta
    public void PlayJump()
    {
        if (_audioSource != null && _jumpClip != null)
            _audioSource.PlayOneShot(_jumpClip);
    }

    // audio quando prende danno
    public void PlayDamage()
    {
        if (_audioSource != null && _damageClip != null)
            _audioSource.PlayOneShot(_damageClip);
    }
}