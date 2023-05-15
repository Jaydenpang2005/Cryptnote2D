using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStatController : MonoBehaviour
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
