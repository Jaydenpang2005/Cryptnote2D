using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrowScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 mousePos;
    private Camera mainCam;
    public float moveSpeed = 10;
    private float force = 8f;
    public Vector3 moveDir = Vector3.right;

    public float lifespan = 3;

    private float fliedTime = 0;

    private Rigidbody2D rb;



    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {

        //transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;

    }

    private void Timer(){

        if(fliedTime < lifespan){
            fliedTime -= Time.deltaTime;
        }else{
            Destroy(gameObject);
        }

    }
}
