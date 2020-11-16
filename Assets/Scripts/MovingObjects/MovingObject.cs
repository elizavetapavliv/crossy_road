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

    private Vector3 initialPosition;

    public int direction;

    public bool isUsed;

    public float offset;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isUsed = false;
    }

    private void Update()
    {
        var distance = Mathf.Abs(transform.position.z - initialPosition.z);

        if (distance >= maxDistance - Mathf.Abs(offset))
        {
            StopMoving();
            if(isLog && passanger != null)
            {
                passanger.Die();
                passanger = null;
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
        initialPosition = transform.position;
        float time = minTime;
        if (!isUsed)
        {
            initialPosition.z += offset;
            transform.position = initialPosition;
            transform.DOMoveZ(initialPosition.z + direction * maxDistance, time);
            isUsed = true;
        }
        else
        {
            offset = 0f;
            time = Random.Range(minTime, maxTime);
            transform.DOMoveZ(initialPosition.z + direction * maxDistance, time);
        }
    }

    public void StopMoving()
    {
        transform.DOComplete();
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