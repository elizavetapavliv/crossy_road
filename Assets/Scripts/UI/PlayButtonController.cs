using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonController : MonoBehaviour
{
    [SerializeField]
    private Button playButton = default;

    private void Start()
    {
        var onClickEvent = new Button.ButtonClickedEvent();
        onClickEvent.AddListener(() => SceneManager.LoadScene("Level"));
        playButton.onClick = onClickEvent;
    }
}