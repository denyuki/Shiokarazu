using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    float decreaseTime = 0.5f;
    int hp = 100;
    [SerializeField] Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OilUpdate()
    {
        CursorChange();

        if (Input.GetMouseButton(0))
        {
            decreaseTime -= Time.deltaTime;

            if (decreaseTime < 0)
            {
                hp--;
                decreaseTime = 0.5f;

                Debug.Log(hp);
            }
        }
    }


    void Sound(){}

    public void CursorChange() {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

    public void CursorChangeNormal()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    }

}
