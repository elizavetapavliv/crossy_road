using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField]
    private GameObject coinPrefab = default;

    public int poolSize = default;

    private List<Coin> pool = default;

    public static CoinPool instance;
    private void Start()
    {
        instance = this;
        pool = new List<Coin>(poolSize);
        for(int i = 0; i < poolSize; i++)
        {
            pool.Add(CreateCoin());
        }
    }

    public Coin GetPooledObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }

    private Coin CreateCoin()
    {
        var coin = Instantiate(coinPrefab);
        coin.SetActive(false);
        return coin.GetComponent<Coin>();
    }
}