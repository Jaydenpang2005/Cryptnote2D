using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    private PlayerStat php;
    public int damage;
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            php = collider.gameObject.GetComponent<PlayerStat>();
            php.LoseHealth(damage);
        }
    }
}
