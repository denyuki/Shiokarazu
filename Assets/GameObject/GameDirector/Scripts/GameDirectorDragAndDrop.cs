<<<<<<< HEAD
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

    enum MousState {
        NORMAL,
        GEAR,
        OIL,
    }
    MousState mousState = MousState.NORMAL;

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
        //ゲームの状態
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

        //マウスの状態
        switch (mousState)
        {
            case MousState.NORMAL:
                break;
            case MousState.OIL:
                GetComponent<Oil>().OilUpdate();
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

                GetComponent<Oil>().CursorChangeNormal();　//カーソル変更

                //マウスの座標取得
                Vector3 mousePos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                               camera.ScreenToWorldPoint(Input.mousePosition).y,
                                               0);

                //プレハブ生成
                dragAndDropObject = Instantiate(gearPrefab, mousePos, Quaternion.identity);
                dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = true;

                ////////////////////////////////////////////////////////////
                
                //ここから巣原が記述

                //ギアを管理するオブジェクトに追加する
                gearDirector.gearNumList.Add(dragAndDropObject);

                //ここまで巣原が記述

                ////////////////////////////////////////////////////////////


                //状態変更
                mousState = MousState.NORMAL;
                dragAndDrop = DragAndDrop.OBJECT_DRAG;
            }
            //アイテム工場だったらアイテムを使えるようになる
            else if(hit2d && hit2d.collider.gameObject.tag == Common.ItemFactory)
            {
                GetComponent<Oil>().CursorChange();  //カーソル変更
                mousState = MousState.OIL;
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
            dragAndDrop = DragAndDrop.OBJECT_DROP;
        }
    }


    //オブジェクを置く
    void Drop()
    {
        dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = false;
        dragAndDrop = DragAndDrop.OBJECT_GET;
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //ギアを道具箱に戻す用の変数
    bool returnGear = false;

    //手数を更新する用の変数
    [SerializeField]
    GameObject UIManager;

    UIManager moveNumText;

    [SerializeField]
    LayerMask itemLayerMask;

    [SerializeField]
    LayerMask gearLayerMask;

    bool itemLayerMaskOn = false;

    public GameObject debugObject;

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

        this.moveNumText = this.UIManager.GetComponent<UIManager>();
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
            
            //アイテムボックスの切り替え用のレイヤーマスク
            LayerMask noHitLayerMask = itemLayerMask;
            if (this.itemLayerMaskOn)
            {
                noHitLayerMask = this.gearLayerMask;
                Debug.Log("1");
            }
            else if (!this.itemLayerMaskOn)
            {
                noHitLayerMask = this.itemLayerMask;
                Debug.Log("2");
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, ~(noHitLayerMask));
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

            //デバック用
            //this.debugObject = hit2d.collider.gameObject;

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
                dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = true;

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
            ////////////////////////////////////////////////////////////

            //ここから巣原が記述

            else if(hit2d && hit2d.collider.gameObject.tag == Common.OpenSwitch)
            {
                this.moveNumText.ChangeBoxButtonState();
            }

            //ここまで巣原が記述

            ////////////////////////////////////////////////////////////
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
            dragAndDrop = DragAndDrop.OBJECT_DROP;

            
        }
    }


    //オブジェクを置く
    void Drop()
    {
        dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = false;
        dragAndDrop = DragAndDrop.OBJECT_GET;

        ////////////////////////////////////////////////////////////

        //ここから巣原が記述

        this.moveNumText.MoveNumPlus();

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit2d && hit2d.collider.gameObject.tag == Common.GearFactory && this.returnGear)
        {
            Debug.Log("des");

            gearDirector.gearNumList.Remove(dragAndDropObject);

            Destroy(dragAndDropObject);
            dragAndDropObject = null;
        }

        if (!this.returnGear)
        {
            this.returnGear = true;
        }

        //ここまで巣原が記述

        ////////////////////////////////////////////////////////////
    }

    //アイテムボックスを有効にする関数
    public void ItemBoxButtonOn()
    {
        this.itemLayerMaskOn = true;
    }
    
    //ギアボックスを有効にする関数
    public void GearBoxButtonOn()
    {
        this.itemLayerMaskOn = false;
    }
}
>>>>>>> main
