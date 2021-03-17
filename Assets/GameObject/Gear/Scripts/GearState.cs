using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    int gearPower;

    //ギアの耐久度
    [SerializeField]
    float gearDurability = 100;

    //ギアにダメージを与えるときに使うタイマー用の変数
    float gearDamegeTimer = 0;

    //ギアが受けている力の大きさを保存する変数
    float gearReceivePower;

    //つながっているギアの情報を保持しておく変数
    public GameObject beforeGear;

    //始めのギアはgearDistanceを更新しないようにするための変数
    [SerializeField]
    bool startGear = false;

    //触れているギアの情報を保持しておく変数
    List<GearState> gearList = new List<GearState>();
    int gearDistance = 0;

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
    }

    //歯車の状態がsingleの時呼ばれる関数
    public void Single()
    {

    }

    //歯車の状態がadaptの時呼ばれる関数
    public void Adapt()
    {
        GearDamege();
    }

    //歯車にかかっている力の大きさを受け取る関数
    public void GearReceivePower(int power)
    {
        //かかっている力を登録する
        this.gearReceivePower = power;

        if(power > this.gearPower && this.gearDamegeTimer <= 0)
        {
            this.gearDamegeTimer = 2;
        }else if(power <= this.gearPower)
        {
            this.gearDamegeTimer = 0;
        }
    }

    //このギアにかかっている力を返す関数
    public float ReturnGearReceivePower()
    {
        return this.gearReceivePower;
    }

    //ギアの耐久度を減らす関数
    //引数はどれだけオーバーパワーがかかっているか
    void GearDamege()
    {
        //タイマーを減らしていき、０になったらダメージを与える
        if(this.gearDamegeTimer > 0)
        {
            this.gearDamegeTimer -= Time.deltaTime;
        }
        else if(this.gearDamegeTimer <= 0)
        {
            this.gearDurability -= this.gearReceivePower - this.gearPower;
            this.gearDamegeTimer = 2;
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

                    //ダメージタイマーを０にする
                    this.gearDamegeTimer = 0;
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
}
