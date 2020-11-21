using UnityEngine;
using UnityEngine.UI;

public class SoundButtonController : MonoBehaviour
{
    [SerializeField]
    private Sprite onSprite;

    [SerializeField]
    private Sprite offSprite;

    [SerializeField]
    private Button soundButton;

    private void Start()
    {
        var onClickEvent = new Button.ButtonClickedEvent();
        onClickEvent.AddListener(ToggleSound);
        soundButton.onClick = onClickEvent;

        ChangeSprite(PlayerPrefs.GetString("sound"));
    }

    public void ToggleSound()
    { 
        AudioListener.pause = !AudioListener.pause;
        var soundState = AudioListener.pause ? "off" : "on";
        PlayerPrefs.SetString("sound", soundState);
        ChangeSprite(soundState);
    }

    private void ChangeSprite(string soundState)
    {
        if (soundState.Equals("on"))
        {
            soundButton.image.sprite = onSprite;
        }
        else
        {
            soundButton.image.sprite = offSprite;
        }
    }
}