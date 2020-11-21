using DG.Tweening;
using UnityEngine;

public class FollowLevelScene : MonoBehaviour 
{
    [SerializeField]
    private Vector3 offset = default;

    [SerializeField]
    private PlayerInfo playerInfo = default;
    private void Start()
    { 
        transform.DOMove(new Vector3(playerInfo.position.x, 0, playerInfo.position.z) + offset, 0);
    }
}