using UnityEngine;
using UnityEngine.UI;

public class CoinsController : MonoBehaviour
{
    [SerializeField]
    private Text coinsText = default;

    private void Start()
    {
        int coinsCount = PlayerPrefs.GetInt("coins");
        coinsText.text = coinsCount.ToString();
    }

    public void SetNewCoinsCount(int coinsCount)
    {
        coinsText.text = coinsCount.ToString();
    }
}