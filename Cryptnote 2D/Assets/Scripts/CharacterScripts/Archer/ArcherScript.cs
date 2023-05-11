using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherScript : MonoBehaviour
{
    // Start is called before the first frame update

    public int arrowQuantity; //quantity of arrow

    public GameObject arrow;


    void Start()
    {
        arrowQuantity = 10;   //Give 10 arrow in beginning
    }

    // Update is called once per frame
    void Update()
    {
        OnKeyDownHandler();
    }

    private void OnKeyDownHandler(){

        if(Input.GetKey(KeyCode.E)) {
            Instantiate(arrow, transform.position, transform.rotation);
        };

    }

    private void OnClickHandler(){

    }
}
