using System.Collections;
using UnityEngine;

public class BumpPlayer : MonoBehaviour
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
                if (!AudioListener.pause)
                {
                    audioSource.PlayOneShot(waterAudio);
                }
                player.Die();
            }
            else if(!gameObject.name.Contains("Tree"))
            {
                if (!AudioListener.pause)
                {
                    StartCoroutine("PlayAudio", hitAudio);
                }
                player.Die();
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        player = collision.collider.GetComponent<Player>();
        if (gameObject.name.Contains("Tree"))
        {
            if (!AudioListener.pause)
            {
                StartCoroutine("PlayAudio", hitAudio);
            }
            StartCoroutine("MovePlayerBack"); 
        }
    }
    private IEnumerator MovePlayerBack()
    {
        yield return new WaitWhile(() => player.isHopping);
        player.MovePlayer(player.currentDirection * -1, 0f);
    }

    private IEnumerator PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
        yield return new WaitForSeconds(audio.length);
    }
}
