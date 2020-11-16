using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private TerrainPool terrainPool = default;

    [SerializeField]
    private float jumpHeight = default;

    [SerializeField]
    private float jumpDuration = default;

    [SerializeField]
    private float moveDuration = default;

    [SerializeField]
    private float diedScale = default;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private AudioClip coinAudio;

    [SerializeField]
    private AudioClip jumpAudio;

    [SerializeField]
    private AudioClip gameOverAudio;

    private AudioSource audioSource;

    private bool isHopping;

    private int score;
    private int coinsCount;

    private int backSteps;
    private int sidesSteps;

    private bool isDied;

    public int zOffset;

    private void Start()
    {
        score = 0;
        backSteps = 0;
        zOffset = 0;
        audioSource = GetComponent<AudioSource>();
        coinsCount = PlayerPrefs.GetInt("coins");
        isDied = false;
    }
    private void Update()
    {
        if (!isDied)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && !isHopping)
            {
                transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), 0);

                MovePlayer(new Vector3(1, 0, 0));

                terrainPool.GenerateTerrain(transform.position);

                score++;
                scoreText.text = score.ToString();
                backSteps = 0;
                sidesSteps = 0;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isHopping)
            {
                zOffset++;
                transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0);
                MovePlayer(new Vector3(0, 0, 1));
                sidesSteps++;
                CheckSteps(sidesSteps, 6);
                backSteps = 0;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !isHopping)
            {
                zOffset--;
                transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0);
                MovePlayer(new Vector3(0, 0, -1));
                sidesSteps++;
                CheckSteps(sidesSteps, 6);
                backSteps = 0;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && !isHopping)
            {
                transform.DORotateQuaternion(Quaternion.Euler(0, -90, 0), 0);
                backSteps++;
                CheckSteps(backSteps, 3);
                MovePlayer(new Vector3(-1, 0, 0));
                sidesSteps = 0;
            }
        }
    }

    private void MovePlayer(Vector3 translation)
    {
        transform.GetChild(0).DOLocalJump(transform.GetChild(0).localPosition, jumpHeight, 1, jumpDuration)
            .OnComplete(() => isHopping = false);
        audioSource.PlayOneShot(jumpAudio);
        transform.DOMove(transform.position + translation, moveDuration);
        isHopping = true;
    }
    private void CheckSteps(int steps, int maxValue)
    {
        if (steps == maxValue)
        {
            Die();
        }
    }

    public void Die()
    {
        isDied = true;
        transform.DOScale(new Vector3(transform.localScale.x, transform.localScale.y, diedScale), 0);
        transform.DOComplete();
        
        SaveScoreAndCoins();
        if (!AudioListener.pause)
        {
            StartCoroutine("PlayGameOver");
        }
        else
        {
            SceneManager.LoadScene("Result", LoadSceneMode.Additive);
        }
       
    }
    private IEnumerator PlayGameOver()
    {
        audioSource.PlayOneShot(gameOverAudio);
        yield return new WaitWhile(() => audioSource.isPlaying);
        PlayerPrefs.SetFloat("x", transform.position.x);
        PlayerPrefs.SetFloat("y", transform.position.y);
        PlayerPrefs.SetFloat("z", transform.position.z);
        SceneManager.LoadScene("Result", LoadSceneMode.Additive);
    }

    private void SaveScoreAndCoins()
    {
        var best = PlayerPrefs.GetInt("best");
        if (score > best)
        {
            PlayerPrefs.SetInt("best", score);
        }
        PlayerPrefs.SetInt("coins", coinsCount);
        PlayerPrefs.SetInt("score", score);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var gameObjectName = collision.collider.gameObject.name;
        if (gameObjectName.Contains("Grass") || gameObjectName.Contains("Road"))
        {
            var z = transform.position.z;
            var roundZ = Mathf.Round(z);
            if (z != roundZ)
            {
                transform.DOMoveZ(roundZ, 0);
            }
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Contains("Coin"))
        {
            coinsCount++;
            GetComponent<CoinsController>().SetNewCoinsCount(coinsCount);
            audioSource.PlayOneShot(coinAudio);
        }
    }
}