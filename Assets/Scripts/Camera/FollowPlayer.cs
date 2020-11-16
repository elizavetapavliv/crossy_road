using DG.Tweening;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player = default;

    [SerializeField]
    private Vector3 offset = default;

    private void Start()
    {
        transform.DOMoveY(player.transform.position.y + offset.y, 0);
    }

    private void Update()
    { 
        if (player != null)
        {
            transform.DOMoveX(player.transform.position.x + offset.x, 0);
            transform.DOMoveZ(player.transform.position.z + offset.z, 0);
        }
    }
}