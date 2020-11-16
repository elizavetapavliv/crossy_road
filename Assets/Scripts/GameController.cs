using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private void Start()
    {
        
    }

    public void OnClick()
    {
        SceneManager.LoadScene("Level");
    }
}
