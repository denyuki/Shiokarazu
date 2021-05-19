using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirectorState : MonoBehaviour
{

    [SerializeField] Generator generator;

    GearDirector gearDirector;

    // Start is called before the first frame update
    void Start()
    {
        this.gearDirector = GetComponent<GearDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        //クリアしているか
        if (generator.GearConnect())
        {
            Debug.Log("クリア");

            //電流をオンにする
            this.gearDirector.AllCurrentOn();
        }

    }

}
