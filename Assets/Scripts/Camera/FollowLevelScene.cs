using DG.Tweening;
using UnityEngine;

public class FollowLevelScene : MonoBehaviour 
{
    [SerializeField]
    private Vector3 offset = default;
    private void Start()
    {
        transform.DOMove(new Vector3(PlayerPrefs.GetFloat("x"), 
            0, PlayerPrefs.GetFloat("z")) + offset, 0);
    }
}