using UnityEngine;
using UnityEngine.UI;

public class BestController : MonoBehaviour
{
    [SerializeField]
    private Text bestText = default;

    void Start()
    {
        int bestScore = PlayerPrefs.GetInt("best");
        bestText.text = bestScore.ToString();
    }
}
