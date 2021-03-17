using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDirector : MonoBehaviour
{
    //ステージに置かれているギアの数を保存する配列、変数
    public GameObject[] gears;
    int gearNum;

    //ステージのギアに力を分配するためのリスト
    public List<GameObject> gearNumList = new List<GameObject>();

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
        
    }
}

