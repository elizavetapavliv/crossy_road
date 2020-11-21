using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private Text scoreText = default;
    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("score").ToString();
    }
}
