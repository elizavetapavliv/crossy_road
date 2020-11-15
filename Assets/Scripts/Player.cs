using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField]
    private TerrainPool terrainPool = default;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text coinsText;

    [SerializeField]
    private float jumpHeight = default;

    [SerializeField]
    private float jumpDuration = default;

    [SerializeField]
    private float diedScale = default;

    private bool isHopping;

    private int score;

    private int backSteps;

    private int sidesSteps;

    public bool isDead;

    public int zOffset;

    private int coinsCount;

    private void Start()
    {
        coinsCount = 0;
        score = 0;
        backSteps = 0;
        zOffset = 0;
    }
    private void Update()
    {
        if (!isDead)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && !isHopping)
            {
                transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), 0);

                MovePlayer(new Vector3(1, 0, 0));

                //ANOTHER SCRIPT
                terrainPool.GenerateTerrain(transform.position);

                score++;
                scoreText.text = score.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isHopping)
            {
                zOffset++;
                transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0);
                MovePlayer(new Vector3(0, 0, 1));
                sidesSteps++;
                CheckSteps(sidesSteps, 6);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !isHopping)
            {
                zOffset--;
                transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0);
                MovePlayer(new Vector3(0, 0, -1));
                sidesSteps++;
                CheckSteps(sidesSteps, 6);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && !isHopping)
            {
                transform.DORotateQuaternion(Quaternion.Euler(0, -90, 0), 0);
                backSteps++;
                CheckSteps(backSteps, 3);
                MovePlayer(new Vector3(-1, 0, 0));
            }
        }
    }

    private void MovePlayer(Vector3 translation)
    {
        transform.GetChild(0).DOLocalJump(transform.GetChild(0).localPosition, jumpHeight, 1, jumpDuration)
            .OnComplete(() => isHopping = false);
        transform.DOMove(transform.position + translation, 0);
        //animator.SetTrigger("hop");
        isHopping = true;

        //  transform.position += translation;
    }
    //private void FinishHopping()
    //{
    //    isHopping = false;
    //}

    private void CheckSteps(int steps, int maxValue)
    {
        if (steps == maxValue)
        {
            Die();
        }
    }

    public void Die()
    {
        if (gameObject != null)
        {
            transform.DOScale(new Vector3(transform.localScale.x, transform.localScale.y, diedScale), 0);
        }
        isDead = true;
        //Destroy(gameObject);
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
            coinsText.text = coinsCount.ToString();
        }
    }
}