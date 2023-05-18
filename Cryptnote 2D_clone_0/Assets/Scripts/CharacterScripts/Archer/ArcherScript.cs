using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ArcherScript : NetworkBehaviour
{
    // Start is called before the first frame update

    public int arrowQuantity;       //quantity of arrow

    public SpriteRenderer spriteRenderer;   //object that control shapes and color etc

    public GameObject arrow;        //arrow object

    public float attackCoolDownMax; //time interval between two attacks

    private float attackCoolDown;   //arrow attack cool down


    void Start()
    {
        arrowQuantity = 10;         //Give 10 arrow in beginning
        attackCoolDown = 0f;        //set cool down to 0 in beginning
    }

    // Update is called once per frame
    void Update()
    {
        CoolDownTimer();            //timing attack cool down
        OnKeyDownHandler();

    }


//functions: ------------------------------------------------------------------------------------------


    private void OnKeyDownHandler(){
        if(!IsOwner) { return; }

        if(Input.GetKeyDown(KeyCode.E)) {         
            //when pressed E
            ShootServerRpc();
        };

    }

//RPCs

    [ServerRpc]
    void ShootServerRpc() { Shoot(); }

    void Shoot()
    {
        if(attackCoolDown <= 0){
            //Spawn an arrow in Archer position, Archer rotation

            var instantiatedArrow = Instantiate(arrow, transform.position, Quaternion.identity);
            instantiatedArrow.GetComponent<NetworkObject>().Spawn();

            //set cool down
            attackCoolDown = attackCoolDownMax;

        }else{

            //not yet cool down
            Debug.Log("dont be too hasty!");

        }
    }

    private void CoolDownTimer(){

        //cool down clock
        if(attackCoolDown > 0){
            attackCoolDown -= Time.deltaTime;
            
        }
    }
}
