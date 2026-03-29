using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lifebar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Image _fillableLifebar;

    // Aggiorna la UI quando cambia l'HP
    public void UpdateGraphics(int currentHP, int maxHP)
    {
        if (_hpText != null)
            _hpText.text = currentHP + "/" + maxHP;

        if (_fillableLifebar != null)
            _fillableLifebar.fillAmount = (float)currentHP / maxHP;
    }
}