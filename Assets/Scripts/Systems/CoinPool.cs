using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab; // Prefab монети
    [SerializeField] private int poolSize = 40;     // Розмір пулу

    private Queue<GameObject> pool = new Queue<GameObject>();

    public void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab, GameObject.FindGameObjectWithTag("PCoin").transform);
            coin.SetActive(false);
            pool.Enqueue(coin);
        }
    }
    public GameObject Get(Vector3 position)
    {
        if (pool.Count == 0)
        {
            Debug.LogWarning("CoinPool пустий!");
            return null;
        }

        GameObject coin = pool.Dequeue();
        coin.transform.position = position;
        coin.SetActive(true);
        return coin;
    }

    public void Return(GameObject coin)
    {
        coin.SetActive(false);
        pool.Enqueue(coin);
    }
}