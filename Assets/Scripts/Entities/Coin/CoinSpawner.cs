using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] CoinPool coinPool;
    [SerializeField] Transform player;

    [Header("Spawn Settings")]
    [SerializeField] int coinsCount = 40;
    [SerializeField] float spawnRadius = 25f;
    [SerializeField] float respawnDistance = 40f;

    GameObject[] spawnedCoins;

    void Start()
    {
        coinPool.InitializePool(coinsCount);

        spawnedCoins = new GameObject[coinsCount];

        for (int i = 0; i < coinsCount; i++)
        {
            spawnedCoins[i] = coinPool.Get(GetRandomPoint());
        }
    }

    void Update()
    {
        for (int i = 0; i < spawnedCoins.Length; i++)
        {
            GameObject coin = spawnedCoins[i];

            if (!coin || !coin.activeInHierarchy)
            {
                spawnedCoins[i] = coinPool.Get(GetRandomPoint());
                continue;
            }

            float dist = (player.position - coin.transform.position).sqrMagnitude;

            if (dist > respawnDistance * respawnDistance)
            {
                coin.transform.position = GetRandomPoint();
            }
        }
    }

    Vector3 GetRandomPoint()
    {
        Vector2 circle = Random.insideUnitCircle * spawnRadius;

        Vector3 pos = new Vector3(
            player.position.x + circle.x,
            player.position.y + 20f,
            player.position.z + circle.y
        );

        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 50f))
            pos.y = hit.point.y;

        return pos;
    }
}