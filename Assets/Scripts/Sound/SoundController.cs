using UnityEngine;

public class SoundController : MonoBehaviour
{
    private void Start()
    {
        var soundState = PlayerPrefs.GetString("sound");
        AudioListener.pause = soundState.Equals("off") ? true : false;
    }
}