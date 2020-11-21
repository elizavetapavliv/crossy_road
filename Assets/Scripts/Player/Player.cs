using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerPosition playerPosition;

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
    private Text scoreText;

    [SerializeField]
    private AudioClip coinAudio;

    [SerializeField]
    private AudioClip jumpAudio;

    [SerializeField]
    private AudioClip gameOverAudio;

    private AudioSource audioSource;

    public bool isHopping;

    private int score;
    private int coinsCount;

    private int backSteps;
    private int sidesSteps;

    private bool isDied;

    private Vector3 initialChildPosition;

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
    }
    private void Update()
    {
        if (!isDied)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && !isHopping)
            {
                currentDirection = new Vector3(1, 0, 0);
                transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), 0);
                MovePlayer(currentDirection, moveDuration);

                terrainGenerator.GenerateTerrain(transform.position);

                score++;
                scoreText.text = score.ToString();
                backSteps = 0;
                sidesSteps = 0;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isHopping)
            {
                currentDirection = new Vector3(0, 0, 1);
                zOffset++;
                transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0);
                MovePlayer(currentDirection, moveDuration);
                sidesSteps++;
                CheckSteps(sidesSteps, 6);

                backSteps = 0;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !isHopping)
            {
                currentDirection = new Vector3(0, 0, -1);
                zOffset--;
                transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0);
                MovePlayer(currentDirection, moveDuration);
                sidesSteps++;
                CheckSteps(sidesSteps, 6);
                backSteps = 0;
                MovePlayer(currentDirection, moveDuration);

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && !isHopping)
            {
                currentDirection = new Vector3(-1, 0, 0);
                transform.DORotateQuaternion(Quaternion.Euler(0, -90, 0), 0);
                backSteps++;
                CheckSteps(backSteps, 3);
                MovePlayer(currentDirection, moveDuration);
                sidesSteps = 0;
            }

            //if (!isDied && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && !isHopping)
            //{
            //    var position = Input.GetTouch(0).deltaPosition;
            //    if (position.y > 0)
            //    {
            //        currentDirection = new Vector3(1, 0, 0);
            //        transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), 0);
            //        MovePlayer(currentDirection);

            //        terrainGenerator.GenerateTerrain(transform.position);

            //        score++;
            //        scoreText.text = score.ToString();
            //        backSteps = 0;
            //        sidesSteps = 0;
            //    }
            //    else if (position.x < 0)
            //    {
            //        currentDirection = new Vector3(0, 0, 1);
            //        zOffset++;
            //        transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0);
            //        MovePlayer(currentDirection);
            //        sidesSteps++;
            //        CheckSteps(sidesSteps, 6);

            //        backSteps = 0;
            //    }
            //    else if (position.x > 0)
            //    {
            //        currentDirection = new Vector3(0, 0, -1);
            //        zOffset--;
            //        transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0);
            //        MovePlayer(currentDirection);
            //        sidesSteps++;
            //        CheckSteps(sidesSteps, 6);
            //        backSteps = 0;
            //        MovePlayer(currentDirection);

            //    }
            //    else
            //    {
            //        currentDirection = new Vector3(-1, 0, 0);
            //        transform.DORotateQuaternion(Quaternion.Euler(0, -90, 0), 0);
            //        backSteps++;
            //        CheckSteps(backSteps, 3);
            //        MovePlayer(currentDirection);
            //        sidesSteps = 0;
            //    }
            //}
        }
    }

    public void MovePlayer(Vector3 translation, float duration)
    {
        var tw = transform.GetChild(0).DOLocalJump(initialChildPosition, jumpHeight, 1, jumpDuration)
            .OnComplete(() => isHopping = false);
        
        audioSource.PlayOneShot(jumpAudio);

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
            audioSource.PlayOneShot(coinAudio);
        }
    }
}