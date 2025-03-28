using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    private TextMeshProUGUI _coinsText;

    void Start()
    {
        _coinsText = GetComponent<TextMeshProUGUI>();     
    }

    void Update()
    {
        _coinsText.text = "Coins: " + GameManager._instance._coins;
    }
}
