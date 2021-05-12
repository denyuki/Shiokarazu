﻿using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        //ステージ上のギアの数を取得
        this.gears = GameObject.FindGameObjectsWithTag(Common.Gear);
        this.gearNum = this.gears.Length;

        for (int i = 0; i < gears.Length; ++i)
        {
                gearNumList.Add(this.gears[i]);
        }
    }

    // Update is called once per frame
    void Update()
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
        }
    }
}

