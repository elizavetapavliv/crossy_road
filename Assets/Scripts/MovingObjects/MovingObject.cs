using DG.Tweening;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    private float minTime = default;

    [SerializeField]
    private float maxTime = default;

    [SerializeField]
    private bool isLog = default;

    [SerializeField]
    private float maxDistance = default;

    [SerializeField]
    private AudioClip movingAudio;

    private AudioSource audioSource;

    private Player passanger;

    public Vector3 initialPosition;

    public int direction;

    public bool isUsed = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position;
        isUsed = true;
        StartMoving();
    }

    private void Update()
    {
        var distance = Mathf.Abs(transform.position.z - initialPosition.z);

        if (distance >= maxDistance)
        {
            StopMoving();
            if(isLog && passanger != null)
            {
                passanger.Die();
            }
        }

        if (isLog)
        {
            if (passanger != null)
            {
                passanger.transform.DOMoveZ(transform.position.z + passanger.zOffset, 0);
            }
        }
        else
        {
            audioSource.PlayOneShot(movingAudio);
        }
    }
    public void StartMoving()
    {
        var time = Random.Range(minTime, maxTime);
        transform.DOMoveZ(initialPosition.z + direction * maxDistance, time);
    }

    public void StopMoving()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isLog)
        {
            var playerComponent = collision.collider.GetComponent<Player>();
            passanger = playerComponent;
            playerComponent.zOffset = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isLog && passanger != null)
        {
            passanger = null;
        }
    }

}