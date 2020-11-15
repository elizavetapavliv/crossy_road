using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField]
    private int minCount;

    [SerializeField]
    private int maxCount;

    private List<Coin> coins;

    private void Start()
    {
        coins = new List<Coin>();
        GenerateCoins();
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

                coin.transform.DOMoveX(transform.localPosition.x, 0);
                coin.transform.DOMoveZ(Random.Range(collider.bounds.min.z, collider.bounds.max.z), 0);
            }
        }
    }

    public void RegenerateCoins()
    {
        foreach (var coin in coins)
        {
            coin.gameObject.SetActive(false);
        }
        coins.Clear();
        GenerateCoins();
    }
}
