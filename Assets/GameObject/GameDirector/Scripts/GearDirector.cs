using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GearDirector : MonoBehaviour
{
    //ステージに置かれているギアの数を保存する配列、変数
    public GameObject[] gears;
    int gearNum;

    //ステージのギアに力を分配するためのリスト
    public List<GameObject> gearNumList = new List<GameObject>();
    public List<GearState> PowerReceiveList = new List<GearState>();

    //すでに管理に追加したかどうかを判断するリスト
    public List<GameObject> alreadyAddObject = new List<GameObject>();
    bool checkAdd = true;

    //シーン移動に使う変数
    [SerializeField]
    string nextScene;

    //一番ゴールに近いギアの電流の位置を変更するための変数
    int maxGearDistance = 0;
    GearState maxGear;

    //スタートのギアの電流の位置を変更するための変数
    int minGearDistance = 5;
    GearState minGear;

    // Start is called before the first frame update
    void Start()
    {
        //ステージ上のギアの数を取得
        this.gears = GameObject.FindGameObjectsWithTag(Common.Gear);
        this.gears = GameObject.FindGameObjectsWithTag(Common.StageGear);
        this.gearNum = this.gears.Length;

        for (int i = 0; i < gears.Length; ++i)
        {
                gearNumList.Add(this.gears[i]);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //力を分配するための処理
        for(int i = 0;i < this.gearNumList.Count; ++i)
        {
            this.checkAdd = true;
            for(int j = 0; j < this.alreadyAddObject.Count; ++j)
            {
                if(gearNumList[i] == alreadyAddObject[j])
                {
                    this.checkAdd = false;
                }
            }

            if (this.checkAdd)
            {
                this.PowerReceiveList.Add(this.gearNumList[i].GetComponent<GearState>());
                this.alreadyAddObject.Add(this.gearNumList[i]);
            }
            
        }
        
        
        for (int i = 0; i < this.PowerReceiveList.Count; ++i)
        {
            this.PowerReceiveList[i].SearchAndReceiveGearPower();
        }
        
    }

    //シーン遷移用の関数
    public void GoToNextScenen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(this.nextScene);
    }

    //ステージ上の全ての歯車の電流をオンにする
    public void AllCurrentOn()
    {
        for(int i = 0; i < this.PowerReceiveList.Count; ++i)
        {
            Debug.Log(this.PowerReceiveList[i].name);
            this.PowerReceiveList[i].currentPosition();

            if (this.PowerReceiveList[i].getGearDistance > this.maxGearDistance)
            {
                this.maxGearDistance = this.PowerReceiveList[i].getGearDistance;
                this.maxGear = this.PowerReceiveList[i];
            }

            if(this.PowerReceiveList[i].getGearDistance < this.minGearDistance)
            {
                this.minGearDistance = this.PowerReceiveList[i].getGearDistance;
                this.minGear = this.PowerReceiveList[i];
            }
        }

        this.maxGear.CurrentEndPosition();
        this.minGear.CurrentStartPosition();

    }
}

