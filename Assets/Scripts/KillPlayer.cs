using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip hitAudio;

    [SerializeField]
    private AudioClip waterAudio;

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.GetComponent<Player>();
        if (player)
        {
            if(gameObject.name.Contains("Water"))
            {
                audioSource.PlayOneShot(waterAudio);
                Destroy(collider.gameObject);
            }
            else
            {
                audioSource.PlayOneShot(hitAudio);
            }
            player.Die();
        }
    }
}
