using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonController : MonoBehaviour
{
    public void OnClickPlay()
    {
        SceneManager.LoadScene("Level");

    }
}