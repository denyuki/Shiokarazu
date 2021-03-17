﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirectorDragAndDrop : MonoBehaviour
{
    [SerializeField] Camera camera;

    GameObject dragAndDropObject;

    [SerializeField] GameObject gearPrefab;

    [SerializeField] GameObject ItemPrefab;

    Vector3 startPosition;

    ////////////////////////////////////////////////////////////
    
    //ここから巣原が記述

    //GearDirector.gearNumListにギアを追加するための変数
    GearDirector gearDirector;

    //ここまで巣原が記述

    ////////////////////////////////////////////////////////////

    enum DragAndDrop
    {
        OBJECT_GET,
        OBJECT_DRAG,
        OBJECT_DROP,
    }
    DragAndDrop dragAndDrop = DragAndDrop.OBJECT_GET;

    // Start is called before the first frame update
    void Start()
    {
        ////////////////////////////////////////////////////////////
        
        //ここから巣原が記述
        this.gearDirector = GetComponent<GearDirector>();
        //ここまで巣原が記述

        ////////////////////////////////////////////////////////////
    }

    // Update is called once per frame
    void Update()
    {
        switch (dragAndDrop)
        {
            case DragAndDrop.OBJECT_GET:
                GetObject();
                break;

            case DragAndDrop.OBJECT_DRAG:
                Drag();
                break;

            case DragAndDrop.OBJECT_DROP:
                Drop();
                break;
        }
    }


    //オブジェク情報を取得
    void GetObject()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

            //ギアだったらそのまま移動へ
            if (hit2d && hit2d.collider.gameObject.tag == Common.Gear)
            {
                
                dragAndDropObject = hit2d.collider.gameObject;

                StartPosition();  //移動前のポジションを保存

                dragAndDrop = DragAndDrop.OBJECT_DRAG;
            }
            //ギア工場だったらギアを生成
            else if(hit2d && hit2d.collider.gameObject.tag == Common.GearFactory)
            {

                Debug.Log("factory");
                Vector3 mousePos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                                               camera.ScreenToWorldPoint(Input.mousePosition).y,
                                                               0);

                dragAndDropObject = Instantiate(gearPrefab, mousePos, Quaternion.identity);
                dragAndDropObject.GetComponent<CircleCollider2D>().enabled = false;

                ////////////////////////////////////////////////////////////
                
                //ここから巣原が記述

                //ギアを管理するオブジェクトに追加する
                gearDirector.gearNumList.Add(dragAndDropObject);

                //ここまで巣原が記述

                ////////////////////////////////////////////////////////////

                dragAndDrop = DragAndDrop.OBJECT_DRAG;
            }
            //アイテム工場だったらアイテムを生成
            else if(hit2d && hit2d.collider.gameObject.tag == Common.ItemFactory)
            {

                Vector3 mousePos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                               camera.ScreenToWorldPoint(Input.mousePosition).y,
                                               0);

                dragAndDropObject = Instantiate(ItemPrefab, mousePos, Quaternion.identity);

                dragAndDrop = DragAndDrop.OBJECT_DRAG;
            }
        }
    }


    //移動前の位置を記憶する
    void StartPosition()
    {
        startPosition = Input.mousePosition;
    }


    //オブジェクを移動
    void Drag()
    {

        //Debug.Log("drag");
        if (!Input.GetMouseButtonUp(0))
        {
            dragAndDropObject.transform.position = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, 
                                                               camera.ScreenToWorldPoint(Input.mousePosition).y,
                                                               0); 

        }
        else
        {
            dragAndDropObject.GetComponent<CircleCollider2D>().enabled = true;
            dragAndDrop = DragAndDrop.OBJECT_DROP;
        }
    }


    //オブジェクを置く
    void Drop()
    {

        dragAndDrop = DragAndDrop.OBJECT_GET;
    }


}
