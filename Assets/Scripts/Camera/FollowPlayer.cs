using DG.Tweening;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo playerInfo = default;

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
        if (!playerInfo.isDied)
        {
            transform.DOMoveX(player.transform.position.x + offset.x, 0);
            transform.DOMoveZ(player.transform.position.z + offset.z, 0);
        }
        else
        {
            transform.DOKill();
        }
    }
}