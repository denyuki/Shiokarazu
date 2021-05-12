using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearFactoryController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    //z軸の移動用の変数
    float frontNum = 0f;
    float backNum = 1f;

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //gearFactoryを前側にする関数
    public void GearFactoryOn()
    {
        this.spriteRenderer.sortingOrder = 5;
        transform.position = new Vector3(transform.position.x, transform.position.y, this.frontNum);
    }

    //gearFactoryを後ろ側にする関数
    public void GearFactoryOff()
    {
        this.spriteRenderer.sortingOrder = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y, this.backNum);
    }
}
