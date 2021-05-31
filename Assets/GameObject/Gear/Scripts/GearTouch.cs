using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTouch : MonoBehaviour
{
    //半径取得用
    SpriteRenderer spriteRenderer;

    public bool DragAndDrop = false;

    PolygonCollider2D polygonCollider2;

    bool canRotate = false;

    Spinning spinning;

    int distance = 0;

    [SerializeField] bool fixedGear = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        this.polygonCollider2 = GetComponent<PolygonCollider2D>();

        this.spinning = GetComponent<Spinning>();
    }

    // Update is called once per frame
    void Update()
    {
        if (distance % 2 == 0)
        {
            spinning.speedRot = new Vector3(0, 0, -30f);
        }
        else if (distance % 2 == 1)
        {
            spinning.speedRot = new Vector3(0, 0, 30f);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!DragAndDrop)
        {
            if(!this.fixedGear)
            {
                Vector3 nearGearPosition = collision.transform.position;
                Vector3 gearPosition = transform.position;

                Vector3 gearVector = gearPosition - nearGearPosition;

                SpriteRenderer spriteRendererOther = collision.gameObject.GetComponent<SpriteRenderer>();

                float sum = (this.spriteRenderer.bounds.size.x + spriteRendererOther.bounds.size.x) / 3;

                //ギアの位置を移動する
                transform.position = collision.transform.position + gearVector.normalized * sum;

                DragAndDrop = true;

                Debug.Log("呼ばれました" + gameObject.name);
            }
            
        }

        //StageGearは無条件で回転し始める
        if(this.gameObject.tag == Common.StageGear)
        {
            //isTriggerをオフにすることで回るようにする
            this.polygonCollider2.isTrigger = false;

            this.distance = gameObject.GetComponent<GearState>().getGearDistance;
        }

        if (this.canRotate)
        {
            //isTriggerをオフにすることで回るようにする
            this.polygonCollider2.isTrigger = false;

            this.distance = gameObject.GetComponent<GearState>().getGearDistance;        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!DragAndDrop)
        {

            if (!this.fixedGear)
            {
                Vector3 nearGearPosition = collision.transform.position;
                Vector3 gearPosition = transform.position;

                Vector3 gearVector = gearPosition - nearGearPosition;

                SpriteRenderer spriteRendererOther = collision.gameObject.GetComponent<SpriteRenderer>();

                float sum = (this.spriteRenderer.bounds.size.x + spriteRendererOther.bounds.size.x) / 3;

                //ギアの位置を移動する
                transform.position = collision.transform.position + gearVector.normalized * sum;

                DragAndDrop = true;

                Debug.Log("呼ばれました" + gameObject.name);
            }
        }

        if (this.canRotate)
        {
            //isTriggerをオフにすることで回るようにする
            this.polygonCollider2.isTrigger = false;

            this.distance = gameObject.GetComponent<GearState>().getGearDistance;


        }
    }

    //isTriggerをオフにして歯車が回るようにする
    public void CanRotateGear()
    {
        this.canRotate = true;
    }

    public void CanNotRotateGear()
    {
        this.canRotate = false;
    }
}
