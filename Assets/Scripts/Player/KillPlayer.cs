using System.Collections;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip hitAudio;

    [SerializeField]
    private AudioClip waterAudio;

    private AudioSource audioSource;

    private Player player;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        player = collider.GetComponent<Player>();
        if (player)
        {
            if(gameObject.name.Contains("Water"))
            {
                audioSource.PlayOneShot(waterAudio);
                player.Die();
            }
            else
            {
                StartCoroutine("PlayAudio", hitAudio);
            }

        }
    }
    private IEnumerator PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
        yield return new WaitForSeconds(audio.length);
        player.Die();
    }
}
