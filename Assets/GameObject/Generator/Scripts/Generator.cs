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


    //ギアとつながっているか
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

    //ギアが繋がっていてクリアしているか（再起関数）
    bool ConnectChase(GearState gear)
    {

        int geraDistance = gear.getGearDistance;

        int distance = gear.gearList[0].getGearDistance;
        int count = 0;


        //ジェネレーターに近いギアを探す
        for (int i = 1; i < gear.gearList.Count; i++)
        {
            if(distance > gear.gearList[i].getGearDistance)
            {
                distance = gear.gearList[i].getGearDistance;
                count = i;
            }
        }

        nowPower += gear.gearList[count].gearPower;
        Debug.Log("nowPower"+ nowPower);

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
