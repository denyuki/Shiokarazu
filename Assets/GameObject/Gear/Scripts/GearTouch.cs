using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTouch : MonoBehaviour
{
    //半径取得用
    Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Vector3 nearGearPosition = collision.transform.position;
        Vector3 gearPosition = transform.position;

        float gearVectorX = Mathf.Abs(gearPosition.x - nearGearPosition.x);
        float gearVectorY = Mathf.Abs(gearPosition.y - nearGearPosition.y);
        float gearVectorZ = Mathf.Abs(gearPosition.z - nearGearPosition.z);

        Vector3 gearVector = new Vector3(gearVectorX, gearVectorY, gearVectorZ);

        //ギアの位置を移動する
        transform.position = collision.transform.position + gearVector; 
    }
}
