using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private CoinPool coinPool;
    [SerializeField] private Transform player;

    [Header("Spawn Settings")]
    [SerializeField]  int coinsCount = 40;
    [SerializeField]  float spawnRadius = 25f;
    [SerializeField]  float respawnDistance = 30f;
    [SerializeField]  float spawnDelay = 1f;

    private GameObject[] spawnedCoins;
    private float respawnDistanceSqr;

    void Start()
    {
        respawnDistanceSqr = respawnDistance * respawnDistance;

        // Підлаштовуємо пул під coinsCount + maxActiveObjects
        coinPool.InitializePool(coinsCount);

        spawnedCoins = new GameObject[coinsCount];


        // Запускаємо корутину спавну монет
        StartCoroutine(SpawnCoinsWithDelay());

    }

     IEnumerator SpawnCoinsWithDelay()
    {
        for (int i = 0; i < coinsCount; i++)
        {
            Vector3 pos = GetRandomPointAroundPlayer();
            spawnedCoins[i] = coinPool.Get(pos);

            yield return new WaitForSeconds(spawnDelay); // чекаємо 1 сек
        }
    }

    void Update()
    {
        for (int i = 0; i < spawnedCoins.Length; i++)
        {
            GameObject coin = spawnedCoins[i];
            if (!coin || !coin.activeSelf) continue;

            float sqrDistance = (player.position - coin.transform.position).sqrMagnitude;
            if (sqrDistance > respawnDistanceSqr)
            {
                Vector3 newPos = GetRandomPointAroundPlayer();
                coin.transform.position = newPos;
            }
        }
    }

    private Vector3 GetRandomPointAroundPlayer()
    {
        Vector2 circle = Random.insideUnitCircle * spawnRadius;

        Vector3 position = new Vector3(player.position.x + circle.x, player.position.y + 20f, player.position.z + circle.y);

        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 50f))
            position.y = hit.point.y;

        return position;
    }
}