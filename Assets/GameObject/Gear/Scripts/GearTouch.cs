using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTouch : MonoBehaviour
{
    //半径取得用
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Vector3 nearGearPosition = collision.transform.position;
        Vector3 gearPosition = transform.position;

        Vector3 gearVector = gearPosition - nearGearPosition;

        SpriteRenderer spriteRendererOther = collision.gameObject.GetComponent<SpriteRenderer>();

        float sum = (this.spriteRenderer.bounds.size.x + spriteRendererOther.bounds.size.x)/2;

        Debug.Log(this.spriteRenderer.bounds.size.x);

        //ギアの位置を移動する
        transform.position = collision.transform.position + gearVector.normalized * sum;
    }
}
