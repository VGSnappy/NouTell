using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private List<CoinBehavior> activeCoins = new List<CoinBehavior>();

    public void Register(CoinBehavior coin)
    {
        if (!activeCoins.Contains(coin))
            activeCoins.Add(coin);
    }

    public void Unregister(CoinBehavior coin)
    {
        activeCoins.Remove(coin);
    }

    void Update()
    {
        for (int i = 0; i < activeCoins.Count; i++)
        {
            //activeCoins[i].ManualUpdate();
        }
    }
}