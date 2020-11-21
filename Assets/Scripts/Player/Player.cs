using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    private const int MAX_SIDE_STEPS = 6;
    private const int MAX_BACK_STEPS = 3;

    [SerializeField]
    private PlayerPosition playerPosition = default;

    [SerializeField]
    private TerrainGenerator terrainGenerator = default;

    [SerializeField]
    private float jumpHeight = default;

    [SerializeField]
    private float jumpDuration = default;

    [SerializeField]
    private float moveDuration = default;

    [SerializeField]
    private float diedScale = default;

    [SerializeField]
    private Text scoreText = default;

    [SerializeField]
    private AudioClip coinAudio = default;

    [SerializeField]
    private AudioClip jumpAudio = default;

    [SerializeField]
    private AudioClip gameOverAudio = default;

    private AudioSource audioSource = default;

    public bool isHopping;

    private int score;
    private int coinsCount;

    private int backSteps;
    private int sidesSteps;

    private bool isDied;

    private Vector3 initialChildPosition;

    private Vector2 startTouch;

    public Vector3 currentDirection;

    public int zOffset;

    private void Start()
    {
        score = 0;
        backSteps = 0;
        zOffset = 0;
        audioSource = GetComponent<AudioSource>();
        coinsCount = PlayerPrefs.GetInt("coins");
        isDied = false;
        initialChildPosition = transform.GetChild(0).localPosition;
        playerPosition.isDied = false;
    }
    private void Update()
    {
        if (!isDied && Input.touchCount > 0 && !isHopping)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouch = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                var delta = touch.position - startTouch;
                if(Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
                {
                    if(delta.y > 0)
                    {
                        currentDirection = new Vector3(1, 0, 0);
                        transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), 0);
                        MovePlayer(currentDirection, moveDuration);
                        score++;
                        scoreText.text = score.ToString();
                        backSteps = 0;
                        sidesSteps = 0;

                        terrainGenerator.GenerateTerrain(transform.position);
                    }
                    else
                    {
                        currentDirection = new Vector3(-1, 0, 0);
                        transform.DORotateQuaternion(Quaternion.Euler(0, -90, 0), 0);
                        backSteps++;
                        CheckSteps(backSteps, MAX_BACK_STEPS);
                        MovePlayer(currentDirection, moveDuration);
                        sidesSteps = 0;
                    }
                }
                else
                {
                    if (delta.x < 0)
                    {
                        currentDirection = new Vector3(0, 0, 1);
                        zOffset++;
                        transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0);
                        MovePlayer(currentDirection, moveDuration);
                        sidesSteps++;
                        CheckSteps(sidesSteps, MAX_SIDE_STEPS);
                        backSteps = 0;
                    }
                    else
                    {
                        currentDirection = new Vector3(0, 0, -1);
                        zOffset--;
                        transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0);
                        MovePlayer(currentDirection, moveDuration);
                        sidesSteps++;
                        CheckSteps(sidesSteps, MAX_SIDE_STEPS);
                        backSteps = 0;
                    }
                }
            }
        }
    }

    public void MovePlayer(Vector3 translation, float duration)
    {
        transform.GetChild(0).DOLocalJump(initialChildPosition, jumpHeight, 1, jumpDuration)
            .OnComplete(() => isHopping = false);

        if (!AudioListener.pause)
        {
            audioSource.PlayOneShot(jumpAudio);
        }

        transform.DOMove(transform.position + translation, duration);
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
        transform.GetChild(0).DOKill();
        transform.DOComplete();

        SaveScoreAndCoins();
        if (!AudioListener.pause)
        {
            StartCoroutine("PlayGameOver");
        }
        else
        {
            LoadResultScene();
        }
       
    }
    private IEnumerator PlayGameOver()
    {
        audioSource.PlayOneShot(gameOverAudio);
        yield return new WaitWhile(() => audioSource.isPlaying);
        LoadResultScene();
    }

    private void LoadResultScene()
    {
        playerPosition.isDied = true;
        playerPosition.position = transform.position;
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
            if (!AudioListener.pause)
            {
                audioSource.PlayOneShot(coinAudio);
            }
        }
    }
}