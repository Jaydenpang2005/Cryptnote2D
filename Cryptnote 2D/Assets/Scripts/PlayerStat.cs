using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int health;

    void Start()
    {
        health = 50;
    }

    public void LoseHealth(int hp)
    {
        health -= hp;
        Debug.Log(health);
    }
}
