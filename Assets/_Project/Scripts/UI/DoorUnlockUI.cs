using TMPro;
using UnityEngine;
using System.Collections;

public class DoorUnlockUI : MonoBehaviour
{
    [SerializeField] private PlayerCoinCollector _player; // riferimento al player
    [SerializeField] private int _requiredCoins = 100;    // minimo per aprire
    [SerializeField] private TextMeshProUGUI _unlockText; // testo "Porta sbloccata!"
    [SerializeField] private AudioSource _unlockSound;    // suono
    [SerializeField] private float _displayTime = 2f;     // tempo in secondi prima di scomparire

    private bool _triggered = false;

    void Start()
    {
        if (_player != null)
            _player.OnCoinsChanged.AddListener(CheckCoins);

        if (_unlockText != null)
            _unlockText.gameObject.SetActive(false); // parte nascosto
    }

    void CheckCoins(int coins)
    {
        if (!_triggered && coins >= _requiredCoins)
        {
            _triggered = true;

            if (_unlockText != null)
            {
                _unlockText.gameObject.SetActive(true); // mostra il testo
                StartCoroutine(HideTextAfterSeconds(_displayTime));
            }

            if (_unlockSound != null)
                _unlockSound.Play(); // suona il suono
        }
    }

    private IEnumerator HideTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (_unlockText != null)
            _unlockText.gameObject.SetActive(false); // nasconde il testo
    }

    public void HideImmediately()  // Nasconde subito il testo
    {
        if (_unlockText != null)
            _unlockText.gameObject.SetActive(false);
    }
}
