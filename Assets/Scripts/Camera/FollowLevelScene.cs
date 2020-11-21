using DG.Tweening;
using UnityEngine;

public class FollowLevelScene : MonoBehaviour 
{
    [SerializeField]
    private Vector3 offset = default;

    [SerializeField]
    private PlayerPosition playerPosition = default;
    private void Start()
    { 
        transform.DOMove(new Vector3(playerPosition.position.x, 0, playerPosition.position.z) + offset, 0);
    }
}