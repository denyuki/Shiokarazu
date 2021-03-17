using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDirector : MonoBehaviour
{
    //ステージに置かれているギアの数を保存する配列
    GameObject[] gears;

    //ステージのギアに力を分配するためのリスト
    List<GameObject> gearNumList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ上のギアの数を取得
        this.gears = GameObject.FindGameObjectsWithTag(Common.Gear);


    }
}

