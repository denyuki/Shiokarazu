using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    GearState gear = null;

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
        if (gear != null && gear.gearList.Count > 0)
        {

            return ConnectChase(gear);
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


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Common.Gear)
        {
            gear = collision.gameObject.GetComponent<GearState>();
        }
    }
}
