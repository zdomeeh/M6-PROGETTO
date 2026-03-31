using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;

    public void UpdateCoinText(int coins) // Aggiorna il testo dell'UI con il numero corrente di monete
    {
        if (_coinText != null) // Controlla che il riferimento non sia nullo
            _coinText.text = "Coins: " + coins;
    }

    public void Test(int value) // Metodo di test per verificare che il valore venga ricevuto correttamente
    {
        Debug.Log("UI riceve valore: " + value);
    }
}
