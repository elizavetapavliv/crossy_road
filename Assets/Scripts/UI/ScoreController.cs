using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("score").ToString();
    }
}
