using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    Queue<GameObject> pool = new Queue<GameObject>();

    public void InitializePool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position,Quaternion.identity,GameObject.FindGameObjectWithTag("PCoin").transform);
            coin.GetComponent<ParticleSystem>().Stop();
            coin.SetActive(false);
            pool.Enqueue(coin);
        }
    }

    public GameObject Get(Vector3 position)
    {
        if (pool.Count == 0)
            return null;

        GameObject coin = pool.Dequeue();
        coin.transform.position = position;
        coin.SetActive(true);
        coin.GetComponent<ParticleSystem>().Play();

        CoinBehavior cb = coin.GetComponent<CoinBehavior>();
        cb.OnSpawn(this);

        return coin;
    }

    public void Return(GameObject coin)
    {
        coin.SetActive(false);
        pool.Enqueue(coin);
    }
}