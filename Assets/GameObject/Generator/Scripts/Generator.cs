using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    GameObject gear = null;

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
        if (gear != null)
        {
            return ConnectChase(gear);
        }else
        {
            return false;
        }
    }


    bool ConnectChase(GameObject gear)
    {
        if (gear.GetComponent<GearState>().beforeGear != null)
        {
            if(gear.tag == Common.GearFactory)
            {
                return true;
            }

            return ConnectChase(gear.GetComponent<GearState>().beforeGear);
        }
        else
        {
            return false;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == Common.Gear)
        {
            gear = collision.gameObject;
        }
    }
}
