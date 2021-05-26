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
    public int gearDistance = 0;

    //力を渡すギアを保存しておく変数
    public List<GearState> receivePowerList = new List<GearState>();

    //渡す力の量を計算するようの変数
    List<int> percentOfReceivePower = new List<int>();
    public int totalPower = 0;

    //力を渡す専用の変数
    [SerializeField]
    float  receivePower = 0;

    //ベルトから力を受け取る用の変数
    public float beltPower = 0;

    bool isDrag = false;

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

        //一定距離以上離れていたら歯車の触れているリストから削除する計算
        for(int i = 0; i < gearList.Count; ++i)
        {
            float distance = 0;
            float distanceX = 0;
            float distanceY = 0;

            distanceX = Mathf.Abs(gameObject.transform.position.x - gearList[i].gameObject.transform.position.x);
            distanceY = Mathf.Abs(gameObject.transform.position.y - gearList[i].gameObject.transform.position.y);

            distance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

            //お互いの半径の合計より離れていたらリストから削除
            if (distance > this.gearPower + gearList[i].gearPower)
            {
                Debug.LogWarning(gameObject.name + " から削除されたよ " + gearList[i].gameObject.name);

                if(this.gearDistance == gearList[i].getGearDistance)
                {
                    this.gearDistance = 0;
                }

                gearList.Remove(gearList[i]);
                
            }
        }
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
                //ベルトから力を受け取っている場合はそっちを渡す
                if(this.beltPower > this.receivePower)
                {
                    this.receivePowerList[i].GearReceivePower(this.receivePower + this.beltPower);
                }
                else
                {
                    this.receivePowerList[i].GearReceivePower(this.receivePower + this.gearPower);
                }
                
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == Common.Gear)
        {
            if (collision.gameObject.GetComponent<GearState>().IsDrag())
            {
                return;
            }
        }

        //つながっている歯車の情報を持つ
        if (collision.gameObject.tag == Common.Gear ||collision.gameObject.tag == Common.StageGear)
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
                            if(!(gearList[j].gameObject.tag == Common.StageGear && gearList[j].getGearDistance == 0))
                            {
                                min = gearList[j].getGearDistance;
                            }
                            
                        }
                    }
                    Debug.LogError(collision.gameObject.name);
                    this.gearDistance = min + 1;
                }

                Debug.Log(this.gearDistance);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Common.Gear)
        {
            if (collision.gameObject.GetComponent<GearState>().IsDrag())
            {
                return;
            }
        }

        GearState gearState;

        if (!(collision.gameObject.tag == Common.Gear || collision.gameObject.tag == Common.StageGear))
        {
            return;
        }

        gearState = collision.gameObject.GetComponent<GearState>();

        this.receivePower = gearState.ReturnGearReceivePower();
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Common.Gear)
        {
            if (collision.gameObject.GetComponent<GearState>().IsDrag())
            {
                return;
            }
        }

        //ドラッグされている最中は処理を行わない
        if (this.isDrag)
        {
            return;
        }

        //つながっている歯車の情報を持つ
        if(collision.gameObject.tag == Common.Gear || collision.gameObject.tag == Common.StageGear)
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
                            if (!(gearList[j].gameObject.tag == Common.StageGear && gearList[j].getGearDistance == 0))
                            {
                                min = gearList[j].getGearDistance;
                            }
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
        if (collision.gameObject.tag == Common.Gear)
        {
            if (collision.gameObject.GetComponent<GearState>().IsDrag())
            {
                return;
            }
        }

        //ドラッグされている最中は処理を行わない
        if (this.isDrag)
        {
            return;
        }

        GearState gearState;

        if(!(collision.gameObject.tag == Common.Gear || collision.gameObject.tag == Common.StageGear))
        {
            return;
        }

        gearState = collision.gameObject.GetComponent<GearState>();
        
        this.receivePower = gearState.ReturnGearReceivePower();
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

        transform.DetachChildren();
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

    public void IsDrag(bool value)
    {
        this.isDrag = value;
    }

    public bool IsDrag()
    {
        return this.isDrag;
    }
}
