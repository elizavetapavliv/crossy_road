using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.GetComponent<Player>();
        if (player)
        {
            if(gameObject.name.Contains("Water"))
            {
                Destroy(collider.gameObject);
            }
            player.Die();
        }
    }
}
