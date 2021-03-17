using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    GearState gearState = null;
    GearTouch gearTouch = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GearConnect()
    {
        if (gearState != null && !gearTouch.DragAndDrop)
        {

            return ConnectChase(gearState);
        }else
        {
            return false;
        }
    }


    bool ConnectChase(GearState gear)
    {

        int geraDistance = gear.getGearDistance;
        Debug.Log(geraDistance);

        if(geraDistance == 0)
        {

            return true;
        }


        int distance = gear.gearList[0].getGearDistance;
        int count = 0;

        for (int i = 1; i < gear.gearList.Count; i++)
        {
            if(distance > gear.gearList[i].getGearDistance)
            {
                distance = gear.gearList[i].getGearDistance;
                count = i;
            }
        }


        return ConnectChase(gear.gearList[count]);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Common.Gear)
        {
            gearState = collision.gameObject.GetComponent<GearState>();
            gearTouch = collision.gameObject.GetComponent<GearTouch>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Common.Gear)
        {
            gearState = null;
            gearTouch = null;
            
        }
    }
}
