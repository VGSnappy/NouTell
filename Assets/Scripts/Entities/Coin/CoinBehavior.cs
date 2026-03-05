//using System.Collections;
//using UnityEngine;

//public class CoinBehavior : MonoBehaviour
//{
//    [SerializeField] CoinPool coinPool;

//    [SerializeField] float orbitSpeed = 200f;
//    [SerializeField] float shrinkSpeed = 2f;
//    [SerializeField] float moveSpeed = 3f;

//    ParticleSystem particleSysInCoin;

//    Transform player;
//    bool isAttracting = false;

//    ParticleSystem.Particle[] particles;

//    void Start()
//    {
//        coinPool = GameObject.Find("CoinManager").GetComponentInChildren<CoinPool>();

//        particleSysInCoin = GetComponentInChildren<ParticleSystem>();

//        if (particleSysInCoin != null)
//            particles = new ParticleSystem.Particle[particleSysInCoin.main.maxParticles];
//    }
//    void Update()
//    {
//        if (!isAttracting || player == null || particleSysInCoin == null) return;

//        int count = particleSysInCoin.GetParticles(particles); // <- тут вже масив не null

//        for (int i = 0; i < count; i++)
//        {
//            Vector3 dir = (player.position - particles[i].position).normalized;
//            Vector3 toPlayer = dir * 6f;
//            Vector3 axis = Vector3.Cross(dir, Vector3.up).normalized;
//            Vector3 swirl = axis * 3f;

//            particles[i].velocity = toPlayer + swirl;
//        }

//        particleSysInCoin.SetParticles(particles, count);
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (isAttracting) return;

//        if (other.CompareTag("Player"))
//        {
//            isAttracting = true;
//            player = other.transform;

//            if (particleSysInCoin != null)
//            {
//                particleSysInCoin.Play();
//                particles = new ParticleSystem.Particle[particleSysInCoin.main.maxParticles];
//            }


//            if (particleSysInCoin != null)
//                StartCoroutine(ShrinkAndOrbit());
//        }
//    }

//    IEnumerator ShrinkAndOrbit()
//    {
//        Vector3 startScale = transform.localScale;
//        float t = 0;

//        while (t < 2f)
//        {
//            t += Time.deltaTime * shrinkSpeed;

//            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

//            if (player != null)
//            {
//                transform.RotateAround(player.position, Vector3.up, orbitSpeed * Time.deltaTime);

//                transform.position = Vector3.MoveTowards(
//                    transform.position,
//                    player.position,
//                    moveSpeed * Time.deltaTime
//                );
//            }

//        yield return null;
//        }
//        coinPool.Return(gameObject);

//    }
//}


using System.Collections;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    [SerializeField] public CoinPool coinPool;

    [SerializeField] private float orbitSpeed = 200f;
    [SerializeField] private float shrinkSpeed = 2f;
    [SerializeField] private float moveSpeed = 3f;

    private ParticleSystem particleSys;
    private ParticleSystem.Particle[] particles;
    private Transform player;
    private bool isAttracting = false;
    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();

        if (particleSys == null)
        {
            Debug.LogWarning($"ParticleSystem не знайдено на {gameObject.name}");
        }
        else
        {
            particles = new ParticleSystem.Particle[particleSys.main.maxParticles];
        }


        // Перевірка пулу
        if (coinPool == null)
        {
            Debug.LogError("CoinPool не прив'язаний!");
        }
    
      coinPool.InitializePool();
    }

    void Update()
    {
        if (!isAttracting || player == null || particleSys == null) return;

        int count = particleSys.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 dir = (player.position - particles[i].position).normalized;
            Vector3 toPlayer = dir * 6f;
            Vector3 axis = Vector3.Cross(dir, Vector3.up).normalized;
            Vector3 swirl = axis * 3f;

            particles[i].velocity = toPlayer + swirl;
        }

        particleSys.SetParticles(particles, count);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAttracting) return;

        if (other.CompareTag("Player"))
        {
            isAttracting = true;
            player = other.transform;

            if (particleSys != null)
                particleSys.Play();

            StartCoroutine(ShrinkAndOrbit());
        }
    }

    IEnumerator ShrinkAndOrbit()
    {
        Vector3 startScale = transform.localScale;
        float t = 0;

        while (t < 2f)
        {
            t += Time.deltaTime * shrinkSpeed;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            if (player != null)
            {
                transform.RotateAround(player.position, Vector3.up, orbitSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }

            yield return null;
        }

        coinPool.Return(gameObject);
    }
}