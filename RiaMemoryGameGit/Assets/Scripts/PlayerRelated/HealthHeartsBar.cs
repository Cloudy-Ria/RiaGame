using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartsBar : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    private int health, maxHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();
    
    public void updateHealth(int _health, int _maxHealth)
    {
        health = _health;
        maxHealth = _maxHealth;
        DrawHearts();
    }
    private void DrawHearts()
    {
        ClearHearts();
        for (int i = 0; i < maxHealth; i++)
        {
            CreateEmptyHeart();
        }
        for (int i = 0; i < hearts.Count; i++)
        {
            if (health > i)
            {
                hearts[i].SetHeartImage(HealthHeart.HeartStatus.Full);
            }
            else
            {
                hearts[i].SetHeartImage(HealthHeart.HeartStatus.Empty);
            }
        }
          
    }

    private void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HealthHeart.HeartStatus.Empty);
        hearts.Add(heartComponent);
    }
    private void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }
}
