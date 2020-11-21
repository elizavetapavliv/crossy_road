using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo playerInfo = default;

    [SerializeField]
    private int minCount = default;

    [SerializeField]
    private int maxCount = default;

    private List<Coin> coins;

    private void Start()
    {
        coins = new List<Coin>();
        GenerateCoins();
    }
    private void Update()
    {
        if (playerInfo.isDied)
        {
            StopCoinsJumping();
        }
    }
    private void GenerateCoins()
    {
        var collider = GetComponent<Collider>();
        maxCount = Mathf.Min(maxCount, CoinPool.instance.poolSize);
        int count = Random.Range(minCount, maxCount);

        for (int i = 0; i < count; i++)
        {
            var coin = CoinPool.instance.GetPooledObject();

            if (coin)
            {
                coin.gameObject.SetActive(true);
                coin.transform.DOMove(new Vector3(transform.position.x,
                   coin.transform.position.y, Random.Range(collider.bounds.min.z, collider.bounds.max.z)), 0);
                coin.transform.DOComplete();
                coins.Add(coin);
            }
        }
    }

    public void RegenerateCoins()
    {
        var collider = GetComponent<Collider>();
        foreach (var coin in coins)
        {
            coin.StopJumping();
            coin.transform.DOMove(new Vector3(transform.position.x,
                  coin.transform.position.y, Random.Range(collider.bounds.min.z, collider.bounds.max.z)), 0);
            coin.transform.DOComplete();
        }
        coins.Clear();
        GenerateCoins();
    }

    private void StopCoinsJumping()
    {
        foreach (var coin in coins)
        {
            coin.StopJumping();
        }
        coins.Clear();
    }
}
