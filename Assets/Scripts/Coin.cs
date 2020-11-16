using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private float jumpHeight = default;

    [SerializeField]
    private float jumpDuration = default;

    [SerializeField]
    private float minWaitingTime = default;

    [SerializeField]
    private float maxWaitingTime = default;

    private void Start()
    {
        StartCoroutine("CoinJump");
    }

    private IEnumerator CoinJump()
    {
        while (true)
        {
            var timeToWait = Random.Range(minWaitingTime, maxWaitingTime);
            yield return new WaitForSeconds(timeToWait);
            transform.DOJump(transform.position, jumpHeight, 2, jumpDuration);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Contains("Player") || collider.gameObject.name.Contains("Tree")
            || collider.gameObject.name.Contains("Coin"))
        {
            gameObject.SetActive(false);
        }
    }
}

