using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    //クリアに必要な力
    [SerializeField] int clearPower = 0;
    //現在の力
    int nowPower = 0;

    //触れているギアの情報
    public GearState gearState = null;
    public GearTouch gearTouch = null;

    public List<GearState> gearstate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //ギアとつながっているか
    public bool GearConnect()
    {

        if (gearState != null && !gearTouch.DragAndDrop)
        {
            Debug.LogWarning(gearState + "gias");

            return ConnectChase(gearState);
        }else
        {
            return false;
        }
    }

    //ギアが繋がっていてクリアしているか（再起関数）
    bool ConnectChase(GearState gear)
    {
        //Debug.Break();

        if (!gear.addEnd)
        {
            return false;
        }

        Debug.LogWarning("1");

        int geraDistance = gear.getGearDistance;

        int distance = 0;

        Debug.Log(gear.gameObject.name + "空っぽ" + gear.gearList.Count);

        if (!(gear.gearList.Count == 0))
        {
            distance = gear.gearList[0].getGearDistance;
        }

        
        int count = 0;

        Debug.LogWarning(gear.gearList.Count + "GearListCount");

        //ジェネレーターに近いギアを探す
        for (int i = 1; i < gear.gearList.Count; i++)
        {
            if(distance > gear.gearList[i].getGearDistance)
            {
                distance = gear.gearList[i].getGearDistance;
                count = i;
            }
        }
        Debug.LogWarning(count + "かうんと");

        nowPower += gear.gearList[count].gearPower;
        Debug.LogWarning("nowPower"+ nowPower + " ClearPower" + clearPower + " GearDistance" + geraDistance);

        //ジェネレーターとつながっているか
        if (geraDistance == 0)
        {
            if (clearPower == nowPower)
            {

                //クリア
                return true;
            }
            else
            {
                //クリアしていない
                nowPower = 0;
                return false;  
            }
        }

       
        return ConnectChase(gear.gearList[count]);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == Common.Gear)
        {
            gearState = collision.gameObject.GetComponent<GearState>();
            gearTouch = collision.gameObject.GetComponent<GearTouch>();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Common.Gear)
        {
            gearState = collision.gameObject.GetComponent<GearState>();
            gearTouch = collision.gameObject.GetComponent<GearTouch>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Common.Gear)
        {
            gearState = null;
            gearTouch = null;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Common.Gear)
        {
            gearState = collision.gameObject.GetComponent<GearState>();
            gearTouch = collision.gameObject.GetComponent<GearTouch>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
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
