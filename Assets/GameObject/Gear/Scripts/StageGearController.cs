using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGearController : MonoBehaviour
{
    //ギアが受けている力の大きさを保存する変数
    //このギアまでの全てのギアの力の合計値が入る
    float gearReceivePower;

    //ギアの力の大きさ    
    public int gearPower = 0;

    //始めのギアはgearDistanceを更新しないようにするための変数
    [SerializeField]
    bool startGear = false;

    //触れているギアの情報を保持しておく変数
    public List<GearState> gearList = new List<GearState>();
    int gearDistance = 0;

    //力を渡すギアを保存しておく変数
    public List<GearState> receivePowerList = new List<GearState>();

    //力を渡す専用の変数
    [SerializeField]
    float receivePower = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.receivePower = this.gearReceivePower;
    }

    //歯車にかかっている力の大きさを受け取る関数
    //自分より前のギア全ての合計値になる
    public void GearReceivePower(float power)
    {
        //かかっている力を登録する
        this.gearReceivePower = power;
    }

    //このギアにかかっている力を返す関数
    public float ReturnGearReceivePower()
    {
        return this.gearReceivePower;
    }

    //次のギアに力を与える関数
    public void SearchAndReceiveGearPower()
    {
        int maxDistance = -1;

        for (int i = 0; i < this.gearList.Count; ++i)
        {
            if (this.gearList[i].getGearDistance > maxDistance)
            {
                this.receivePowerList.Clear();

                this.receivePowerList.Add(this.gearList[i]);
                maxDistance = this.gearList[i].getGearDistance;
            }
            else if (this.gearList[i].getGearDistance == maxDistance)
            {
                this.receivePowerList.Add(this.gearList[i]);
            }
        }

        /*
        for(int i = 0; i < this.receivePowerList.Count; ++i)
        {
            this.totalPower += this.receivePowerList[i].gearPower;
        }

        for(int i = 0;i < this.receivePowerList.Count; ++i)
        {
            this.percentOfReceivePower.Add(MathReceivePower(this.totalPower, this.receivePowerList[i].gearPower));
        }
        */

        for (int i = 0; i < this.receivePowerList.Count; ++i)
        {
            //this.receivePowerList[i].GearReceivePower(this.percentOfReceivePower[i]);
            if (this.gearDistance < this.receivePowerList[i].getGearDistance)
            {
                this.receivePowerList[i].GearReceivePower(this.receivePower + this.gearPower);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //つながっている歯車の情報を持つ
        if (collision.gameObject.tag == Common.Gear)
        {
            bool addList = true;
            for (int i = 0; i < gearList.Count; ++i)
            {
                //Debug.Log(gearList[i].gameObject.name + " " + collision.gameObject.name);
                if (gearList[i].gameObject == collision.gameObject)
                {
                    addList = false;
                    break;
                }
            }

            if (addList)
            {
                gearList.Add(collision.gameObject.GetComponent<GearState>());

                if (!this.startGear)
                {
                    int min = 100;

                    for (int j = 0; j < gearList.Count; ++j)
                    {
                        if (gearList[j].getGearDistance < min)
                        {
                            min = gearList[j].getGearDistance;
                        }
                    }

                    this.gearDistance = min + 1;
                }

                Debug.Log(this.gearDistance);
            }
        }

    }
}
