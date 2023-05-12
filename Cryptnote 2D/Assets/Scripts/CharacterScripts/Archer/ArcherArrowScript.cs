using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrowScript : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeed = 10;
    public Vector3 moveDir = Vector3.right;

    public float lifespan = 3;

    private float fliedTime = 0;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;


    }

    private void Timer(){

        if(fliedTime < lifespan){
            fliedTime -= Time.deltaTime;
        }else{
            Destroy(gameObject);
        }

    }
}
