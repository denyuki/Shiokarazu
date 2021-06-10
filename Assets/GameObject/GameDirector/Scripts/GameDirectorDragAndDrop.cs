using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirectorDragAndDrop : MonoBehaviour
{
    [SerializeField] Camera camera;

    GameObject dragAndDropObject;

    //力１のギア
    [SerializeField] GameObject gearPrefabOne;

    //力２のギア
    [SerializeField] GameObject gearPrefabTwo;

    //力３のギア
    [SerializeField] GameObject gearPrefabThree;

    [SerializeField] GameObject ItemPrefab;

    Vector3 startPosition;

    //ギア１の残り個数のテキスト
    [SerializeField] GameObject gearOneText;
    Text gearOneChangeText;
    public int gearOneNum = 1;

    //ギア２の残り個数のテキスト
    [SerializeField] GameObject gearTwoText;
    Text gearTwoChangeText;
    public int gearTwoNum = 1;

    //ギア３の残り個数のテキスト
    [SerializeField] GameObject gearThreeText;
    Text gearThreeChangeText;
    public int gearThreeNum = 1;

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


    Oil oil;
    Belt belt;

    //ここまで巣原が記述

    ////////////////////////////////////////////////////////////

    enum DragAndDrop
    {
        OBJECT_GET,
        OBJECT_DRAG,
        OBJECT_DROP,
        OBJECT_NULL,
    }
    DragAndDrop dragAndDrop = DragAndDrop.OBJECT_GET;

    // Start is called before the first frame update
    void Start()
    {

        oil = GetComponent<Oil>();
        belt = GetComponent<Belt>();
        ////////////////////////////////////////////////////////////
        
        //ここから巣原が記述
        this.gearDirector = GetComponent<GearDirector>();

        this.moveNumText = this.UIManager.GetComponent<UIManager>();

        this.gearOneChangeText = this.gearOneText.GetComponent<Text>();
        this.gearTwoChangeText = this.gearTwoText.GetComponent<Text>();
        this.gearThreeChangeText = this.gearThreeText.GetComponent<Text>();

        this.gearOneChangeText.text = "残り" + this.gearOneNum + "個";
        this.gearTwoChangeText.text = "残り" + this.gearTwoNum + "個";
        this.gearThreeChangeText.text = "残り" + this.gearThreeNum + "個";
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

                GearState gearState = dragAndDropObject.GetComponent<GearState>();
                gearState.IsDrag(true);
            }
            //ギア工場だったらギアを生成
            /*
            else if(hit2d && hit2d.collider.gameObject.tag == Common.GearFactory)
            {

                Debug.Log("factory");
                Vector3 mousePos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                                               camera.ScreenToWorldPoint(Input.mousePosition).y,
                                                               0);

                dragAndDropObject = Instantiate(gearPrefabOne, mousePos, Quaternion.identity);
                dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = true;

                ////////////////////////////////////////////////////////////
                
                //ここから巣原が記述

                //ギアを管理するオブジェクトに追加する
                gearDirector.gearNumList.Add(dragAndDropObject);

                //ここまで巣原が記述

                ////////////////////////////////////////////////////////////

                dragAndDrop = DragAndDrop.OBJECT_DRAG;
            }
            */
            //大きさ１の歯車を生成
            else if (hit2d && hit2d.collider.gameObject.tag == Common.GearOneFactory)
            {
                if(this.gearOneNum >= 1)
                {
                    this.gearOneNum -= 1;

                    this.gearOneChangeText.text = "残り" + this.gearOneNum + "個";

                    Debug.Log("factoryOne");
                    Vector3 mousePos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                                                   camera.ScreenToWorldPoint(Input.mousePosition).y,
                                                                   0);

                    dragAndDropObject = Instantiate(gearPrefabOne, mousePos, Quaternion.identity);
                    dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = true;

                    ////////////////////////////////////////////////////////////

                    //ここから巣原が記述

                    //ギアを管理するオブジェクトに追加する
                    gearDirector.gearNumList.Add(dragAndDropObject);

                    //ここまで巣原が記述

                    ////////////////////////////////////////////////////////////

                    dragAndDrop = DragAndDrop.OBJECT_DRAG;
                    //dragAndDrop = DragAndDrop.OBJECT_DROP;
                    //Drop(false);
                    
                    //dragAndDrop = DragAndDrop.OBJECT_DRAG;
                }

                
            }
            else if (hit2d && hit2d.collider.gameObject.tag == Common.GearTwoFactory)
            {
                if(this.gearTwoNum >= 1)
                {
                    this.gearTwoNum -= 1;

                    this.gearTwoChangeText.text = "残り" + this.gearTwoNum + "個";

                    Debug.Log("factoryTwo");
                    Vector3 mousePos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                                                   camera.ScreenToWorldPoint(Input.mousePosition).y,
                                                                   0);

                    dragAndDropObject = Instantiate(gearPrefabTwo, mousePos, Quaternion.identity);
                    dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = true;

                    ////////////////////////////////////////////////////////////

                    //ここから巣原が記述

                    //ギアを管理するオブジェクトに追加する
                    gearDirector.gearNumList.Add(dragAndDropObject);

                    //ここまで巣原が記述

                    ////////////////////////////////////////////////////////////

                    dragAndDrop = DragAndDrop.OBJECT_DRAG;
                }

                
            }
            else if (hit2d && hit2d.collider.gameObject.tag == Common.GearThreeFactory)
            {
                if(this.gearThreeNum >= 1)
                {
                    this.gearThreeNum -= 1;

                    this.gearThreeChangeText.text = "残り" + this.gearThreeNum + "個";

                    Debug.Log("factory");
                    Vector3 mousePos = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x,
                                                                   camera.ScreenToWorldPoint(Input.mousePosition).y,
                                                                   0);

                    dragAndDropObject = Instantiate(gearPrefabThree, mousePos, Quaternion.identity);
                    dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = true;

                    ////////////////////////////////////////////////////////////

                    //ここから巣原が記述

                    //ギアを管理するオブジェクトに追加する
                    gearDirector.gearNumList.Add(dragAndDropObject);

                    //ここまで巣原が記述

                    ////////////////////////////////////////////////////////////

                    dragAndDrop = DragAndDrop.OBJECT_DRAG;
                }

                
            }
            //アイテム工場だったらアイテムを生成

            else if (hit2d && hit2d.collider.gameObject.tag == Common.OilFactory)
            {
                if (!oil.oil)
                {
                    oil.CursorChange();
                    oil.oil = true;

                }
                else if (oil.oil)
                {
                    oil.CursorChangeNormal();
                    oil.oil = false;
                }
            }
            else if (hit2d && hit2d.collider.gameObject.tag == Common.BeltFactory)
            {

                if (!belt.belt)
                {
                    belt.CursorChange();
                    belt.belt = true;

                }
                else if (belt.belt)
                {
                    belt.CursorChangeNormal();
                    belt.BeltFalse();
                    belt.belt = false;
                }
            }
            ////////////////////////////////////////////////////////////

            //ここから巣原が記述

            else if (hit2d && hit2d.collider.gameObject.tag == Common.OpenSwitch)
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
            PolygonCollider2D poligonCollider = dragAndDropObject.GetComponent<PolygonCollider2D>();

            poligonCollider.isTrigger = true;
        }
        else
        {
            dragAndDrop = DragAndDrop.OBJECT_DROP;  
        }
    }


    //オブジェクを置く
    void Drop(bool a = true)
    {
        //Debug.LogError("よばれてるよ！！！！！！！！！！！！！！！");

        GearState gearState = dragAndDropObject.GetComponent<GearState>();
        gearState.IsDrag(false);
        gearState.toFirstDrag = false;

        dragAndDropObject.GetComponent<GearTouch>().DragAndDrop = false;
        dragAndDrop = DragAndDrop.OBJECT_GET;

        ////////////////////////////////////////////////////////////

        //ここから巣原が記述

        CircleCollider2D circle = dragAndDropObject.GetComponent<CircleCollider2D>();
        circle.enabled = true; 

        dragAndDropObject.GetComponent<GearTouch>().CanRotateGear();

        this.moveNumText.MoveNumPlus();

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit2d && hit2d.collider.gameObject.tag == Common.GearFactory && this.returnGear && a == true)
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
        ///

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

    //ギアの残り個数のテキストを変更する関数
    //実装出来たら後で置き換えましょう！
    /*
    void IncreaseGearText(GameObject text,int* num)
    {
        Text NumText = text.GetComponent<Text>();

        this.

        NumText.text = "残り" + 
    }
    */
}
