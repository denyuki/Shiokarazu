using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirectorState : MonoBehaviour
{

    [SerializeField] Generator generator;


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //クリアしているか
        if (generator.GearConnect())
        {
            Debug.Log("クリア");
        }

    }

}
