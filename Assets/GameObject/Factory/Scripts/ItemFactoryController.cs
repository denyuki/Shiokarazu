using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactoryController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

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

    //itemFactoryを前側にする関数
    public void ItemFactoryOn()
    {
        this.spriteRenderer.sortingOrder = 5;
        transform.position = new Vector3(transform.position.x, transform.position.y, this.frontNum);
    }

    //itemFactoryを後ろ側にする関数
    public void ItemFactoryOff()
    {
        this.spriteRenderer.sortingOrder = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y, this.backNum);
    }
}
