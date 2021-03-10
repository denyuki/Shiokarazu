using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirectorState : MonoBehaviour
{

    [SerializeField] Generator generator;

    List<GameObject> myList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        myList.Add(gameObject);
        Debug.Log(myList[0].name);
    }

    // Update is called once per frame
    void Update()
    {

        //if (generator.GearConnect())
       // {
            //Debug.Log("クリア");
       // }

    }

}
