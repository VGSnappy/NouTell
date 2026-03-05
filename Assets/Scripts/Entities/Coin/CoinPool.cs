//using System.Collections.Generic;
//using UnityEngine;

//public class CoinPool : MonoBehaviour
//{
//    [SerializeField] GameObject[] objectPrefabs;
//    [SerializeField] Transform parentTarget;

//    [Header("Pool Settings")]
//    [SerializeField] int maxPoolSize = 150;
//    [SerializeField] int maxActiveObjects = 100;


//    Queue<GameObject> pool = new Queue<GameObject>();
//    Queue<GameObject> activeObjects = new Queue<GameObject>();


//    public void InitializePool(int requiredActiveObjects)
//    {
//        int targetPoolSize = Mathf.Max(requiredActiveObjects, maxActiveObjects);

//        while (pool.Count + activeObjects.Count < targetPoolSize)
//            InstantiateNewObject();
//    }

//    public GameObject Get(Vector3 spawnPosition)
//    {
//        if (pool.Count == 0)
//            InstantiateNewObject();

//        GameObject obj = pool.Dequeue();
//        obj.transform.position = spawnPosition;
//        obj.SetActive(true);

//        activeObjects.Enqueue(obj);

//        if (activeObjects.Count > maxActiveObjects)
//        {
//            GameObject oldest = activeObjects.Dequeue();
//            Return(oldest);
//        }

//        return obj;
//    }

//    public void Return(GameObject obj)
//    {
//        obj.GetComponentInChildren<ParticleSystem>().Stop();
//        obj.SetActive(false);

//        if (pool.Count < maxPoolSize)
//            pool.Enqueue(obj);
//        else
//        {
//            GameObject oldest = pool.Dequeue();
//            Destroy(oldest);
//            pool.Enqueue(obj);
//        }
//    }

//    private void InstantiateNewObject()
//    {
//        GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
//        GameObject obj = Instantiate(prefab, parentTarget);


//        ParticleSystem particles = obj.GetComponentInChildren<ParticleSystem>();

//        var main = particles.main;
//        main.simulationSpace = ParticleSystemSimulationSpace.World;
//        var color = obj.GetComponent<Renderer>().material.color;
//        main.startColor = color;

//        particles.Stop();



//        obj.SetActive(false);
//        pool.Enqueue(obj);
//    }
//}
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int poolSize = 40;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.GetComponent<CoinBehavior>().coinPool = gameObject.GetComponent<CoinPool>();
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