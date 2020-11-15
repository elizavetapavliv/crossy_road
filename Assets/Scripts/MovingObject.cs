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

    private Player passanger;

    public Vector3 initialPosition;

    public int direction;

    public bool isUsed = false;

    private void Start()
    {
        initialPosition = transform.position;
        isUsed = true;
        StartMoving();
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.z - initialPosition.z) >= maxDistance)
        {
            gameObject.SetActive(false);
            transform.DOMoveZ(initialPosition.z, 0);
        }
       
        if (isLog && passanger != null)
        {
             passanger.transform.DOMoveZ(transform.position.z + passanger.zOffset, 0);
        }
    }
    public void StartMoving()
    {
        var time = Random.Range(minTime, maxTime);
        transform.DOMoveZ(initialPosition.z + direction * maxDistance, time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isLog)
        {
            var player = collision.collider.GetComponent<Player>();
            passanger = player;
            player.zOffset = 0;
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
