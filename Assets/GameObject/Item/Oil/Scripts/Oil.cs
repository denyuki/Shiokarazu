using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    float decreaseTime = 0.5f;
    int hp = 100;

    public bool push = false;

    AudioSource audioSource;


    [SerializeField] GameObject uIManager;

    [SerializeField] Texture2D texture;

    Vector2 pastMousePos;
    Vector2 nowMousePos;

    Vector2 pastVector;
    Vector2 nowVector;

    [SerializeField] Camera camera;


    //Oilサウンド
    [SerializeField] AudioClip puhs;
    [SerializeField] AudioClip shake;


    enum OilState
    {
        NOEMAL,
        PUSH,
    }
    OilState oilState = OilState.NOEMAL;

    float time = 0.1f;

    bool sound = false;

    // Start is called before the first frame update
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OilUpdate()
    {

        switch (oilState)
        {
            case OilState.NOEMAL:
                Nomal();
                push = false;
                //Sound();
                break;
            case OilState.PUSH:
                Push();
                push = true;
                break;

        }
    }

    void Nomal()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

            //ギアだったらそのまま移動へ
            if (hit2d.collider != null && hit2d.collider.gameObject.tag == Common.StartGenerate)
            {
                //かける音を鳴らす
                audioSource.clip = puhs;
                audioSource.loop = true;
                audioSource.Play();

                oilState = OilState.PUSH;

            }
        }

    }

    void Push()
    {
        decreaseTime -= Time.deltaTime;

        if (decreaseTime < 0)
        {
            hp--;
            decreaseTime = 0.5f;

            //Debug.Log(hp);

            uIManager.GetComponent<UIManager>().timeLimit += Time.deltaTime * 100;
        }


        if (Input.GetMouseButtonUp(0))
        {
            //かける音を止める
            audioSource.Stop();
            oilState = OilState.NOEMAL;
        }
    }


    void Sound()
    {
        /*
        time -= Time.deltaTime;
        if (time < 0)
        {
            time = 0.1f;

            nowMousePos = new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,
                               camera.ScreenToWorldPoint(Input.mousePosition).y);

            float vertical = nowMousePos.y - pastMousePos.y;
            float side = nowMousePos.x - pastMousePos.x;

            nowVector = new Vector2(side, vertical);


            
            if((nowVector.x < 0 && pastVector.x > 0) || (nowVector.x > 0 && pastVector.x < 0) ||
                (nowVector.y < 0 && pastVector.y > 0) || (nowVector.y > 0 && pastVector.y < 0))
            {
                if (Mathf.Sqrt(side * side + vertical * vertical) > 2f)
                {
                    //振る音を鳴らす
                    audioSource.clip = shake;
                    audioSource.loop = false;
                    audioSource.Play();
                    sound = false;
                }
            }
            

             /*
            if (Mathf.Sqrt(side * side + vertical * vertical) > 3f)
            {
                sound = true;
            }else if (sound)
            {

                //振る音を鳴らす
                audioSource.clip = shake;
                audioSource.loop = false;
                audioSource.Play();
                sound = false;
            }


            pastMousePos = nowMousePos;
            pastVector = nowVector;
        }
      */

        
    }

    public void CursorChange() {

        pastMousePos = new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,
                           camera.ScreenToWorldPoint(Input.mousePosition).y);

        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

    public void CursorChangeNormal()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    }

}
