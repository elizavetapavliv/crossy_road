using UnityEngine;

[CreateAssetMenu(fileName = "Player Info", menuName = "Player Info")]
public class PlayerInfo : ScriptableObject
{
    public Vector3 position;
    public bool isDied;
}