using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearState : MonoBehaviour
{
    //ギアの状態
    public enum State
    {
        single,
        adapt
    }
    State state = State.single;

    //ギアの力の大きさ    
    public int gearPower;

    //ギアが受けている力の大きさを保存する変数
    //このギアまでの全てのギアの力の合計値が入る
    float gearReceivePower;

    //つながっているギアの情報を保持しておく変数
    public GameObject beforeGear;

    //始めのギアはgearDistanceを更新しないようにするための変数
    [SerializeField]
    bool startGear = false;

    //触れているギアの情報を保持しておく変数
    public List<GearState> gearList = new List<GearState>();
    int gearDistance = 0;

    //力を渡すギアを保存しておく変数
    public List<GearState> receivePowerList = new List<GearState>();

    //渡す力の量を計算するようの変数
    List<int> percentOfReceivePower = new List<int>();
    public int totalPower = 0;

    //力を渡す専用の変数
    [SerializeField]
    float  receivePower = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (this.state)
        {
            case State.single: Single(); break;
            case State.adapt:  Adapt();  break;
        }

        this.receivePower = this.gearReceivePower;
    }

    //歯車の状態がsingleの時呼ばれる関数
    public void Single()
    {

    }

    //歯車の状態がadaptの時呼ばれる関数
    public void Adapt()
    {
        
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

    //今は使ってない
    //渡す力の量を計算する関数
    //floatのほうがいいかも
    /*
    int MathReceivePower(int gearNum,int percent)
    {
        return (this.gearPower / gearNum) * percent;
    }
    */

    //次のギアに力を与える関数
    public void SearchAndReceiveGearPower()
    {
        int maxDistance = -1;

        for (int i = 0; i < this.gearList.Count; ++i)
        {
            if(this.gearList[i].gearDistance > maxDistance)
            {
                this.receivePowerList.Clear();

                this.receivePowerList.Add(this.gearList[i]);
                maxDistance = this.gearList[i].gearDistance;
            }else if(this.gearList[i].gearDistance == maxDistance)
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

        for(int i = 0;i < this.receivePowerList.Count; ++i)
        {
            //this.receivePowerList[i].GearReceivePower(this.percentOfReceivePower[i]);
            if(this.gearDistance < this.receivePowerList[i].getGearDistance)
            {
                this.receivePowerList[i].GearReceivePower(this.receivePower + this.gearPower);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //つながっている歯車の情報を持つ
        if(collision.gameObject.tag == Common.Gear)
        {
            bool addList = true;
            for(int i = 0; i < gearList.Count; ++i)
            {
                //Debug.Log(gearList[i].gameObject.name + " " + collision.gameObject.name);
                if(gearList[i].gameObject == collision.gameObject)
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        GearState gearState;

        if(!(collision.gameObject.tag == Common.Gear))
        {
            return;
        }

        gearState = collision.gameObject.GetComponent<GearState>();
        
        this.receivePower = gearState.ReturnGearReceivePower();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Gear")
        {
            gearList.Remove(collision.gameObject.GetComponent<GearState>());
        }
    }

    public int getGearDistance
    {
        get
        {
            return this.gearDistance;
        }
    }

    public void ChangeState(State next)
    {
        //後始末
        switch (this.state)
        {
            case State.single:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                }
                break;
            case State.adapt:
                {
                    //stateを切り替えた際に後始末が必要ならばここに記述
                }
                break;
        }

        //stateの切り替え   
        this.state = next;

        //初期化処理
        switch (this.state)
        {
            case State.single:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
            case State.adapt:
                {
                    //stateを切り替えた際に初期化が必要ならばここに記述
                }
                break;
        }
    }

    //電流のポジションを変更する関数
    public void currentPosition()
    {
        GameObject rightEnd;
        GameObject leftEnd;
        GameObject currentOnSwitch;

        Transform rightEndTransform;
        Transform leftEndTransform;
        Transform currentOnSwitchTransform;

        currentOnSwitchTransform = transform.Find(Common.SimpleLightningBoltPrefab);
        leftEndTransform = currentOnSwitchTransform.Find(Common.LightningStart);
        rightEndTransform = currentOnSwitchTransform.Find(Common.LightningEnd);
        
        if(rightEndTransform == null)
        {
            Debug.LogError("右が見つかってないよ" + gameObject.name);
        }
        

        rightEnd = rightEndTransform.gameObject;
        leftEnd = leftEndTransform.gameObject;
        currentOnSwitch = currentOnSwitchTransform.gameObject;

        Vector3 rightCurrentPosition = new Vector3(0f,0f,0f);
        Vector3 leftCurrentPosition = new Vector3(0f,0f,0f);

        int min = 100;
        int max = 0;

        //つながっている歯車の位置を取得する(前後）
        for(int i = 0; i < gearList.Count; ++i)
        {
            if(this.gearList[i].getGearDistance < this.gearDistance)
            {
                if(this.gearList[i].getGearDistance < min)
                {
                    leftCurrentPosition = this.gearList[i].transform.position;
                    min = this.gearList[i].getGearDistance;
                }
            }else if(this.gearList[i].getGearDistance > this.gearDistance)
            {
                if(this.gearList[i].gearDistance > max)
                {
                    rightCurrentPosition = this.gearList[i].transform.position;
                    max = this.gearList[i].getGearDistance;
                }
            }
        }

        leftEnd.transform.position = leftCurrentPosition;
        rightEnd.transform.position = rightCurrentPosition;

        //電流をオンにする
        currentOnSwitch.SetActive(true);
    }

    public void CurrentStartPosition()
    {
        GameObject leftEnd;
        Transform leftEndTransform;
        Transform currentOnSwitchTransform;

        currentOnSwitchTransform = transform.Find(Common.SimpleLightningBoltPrefab);
        leftEndTransform = currentOnSwitchTransform.Find(Common.LightningStart);

        leftEnd = leftEndTransform.gameObject;

        leftEnd.transform.localPosition = new Vector3(-0.5f, 0f, 0f);
    }

    public void CurrentEndPosition()
    {
        GameObject rightEnd;
        Transform rightEndTransform;
        Transform currentOnSwitchTransform;

        currentOnSwitchTransform = transform.Find(Common.SimpleLightningBoltPrefab);
        rightEndTransform = currentOnSwitchTransform.Find(Common.LightningEnd);

        rightEnd = rightEndTransform.gameObject;

        rightEnd.transform.localPosition = new Vector3(0.5f, 0f, 0f);
    }
}
