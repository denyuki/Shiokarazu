using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text moveNumText;
    
    int moveNum = 0;

    [SerializeField]
    Button GearBoxButton;

    [SerializeField]
    Button ItemBoxButton;

    //切り替えボタンの表示を切り替える変数
    bool exchangeSwitch = true;

    //ステージの表示位置移動用の変数
    [SerializeField]
    Camera camera;

    [SerializeField]
    GameObject stageLeftEnd;

    [SerializeField]
    GameObject stageRightEnd;

    [SerializeField]
    GameObject scrollbar;

    Scrollbar cameraScrollbar;

    //カメラが移動できる端を保存しておく変数
    float leftEndPosition = 9f;
    float rightEndPosition = -9f;

    //制限時間表示用の変数
    [SerializeField]
    GameObject Timer;

    Text timerText;
    float timeLimit = 100f;

    [SerializeField]
    GameObject startText;

    // Start is called before the first frame update
    void Start()
    {
        this.cameraScrollbar = this.scrollbar.GetComponent<Scrollbar>();
        this.timerText = this.Timer.GetComponent<Text>();

        this.leftEndPosition = this.stageLeftEnd.transform.position.x + this.leftEndPosition;
        this.rightEndPosition = this.stageRightEnd.transform.position.x + this.rightEndPosition;

        //スタートのテキストをオンにする
        this.startText.SetActive(true);
        Invoke("StartTextOff", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(this.timeLimit > 0)
        {
            this.timeLimit -= Time.deltaTime;
            this.timerText.text = "" + this.timeLimit;
        }
    }

    public void MoveNumPlus()
    {
        ++this.moveNum;

        this.moveNumText.text = "手数：" + this.moveNum;
    }

    //アイテムボックスとギアボックスの切り替えボタンのオンオフをする関数
    public void ChangeBoxButtonState()
    {

        if (!this.exchangeSwitch)
        {
            this.ItemBoxButton.gameObject.SetActive(true);
            this.GearBoxButton.gameObject.SetActive(true);

            this.exchangeSwitch = true;
        }else if (this.exchangeSwitch)
        {
            this.ItemBoxButton.gameObject.SetActive(false);
            this.GearBoxButton.gameObject.SetActive(false);

            this.exchangeSwitch = false;
        }
        
    }

    public void CameraTransformcontrolByScrollbar()
    {
        Vector3 cameraPosition = new Vector3(this.leftEndPosition+ this.rightEndPosition * this.cameraScrollbar.value, this.camera.transform.position.y, this.camera.transform.position.z);

        this.camera.transform.position = cameraPosition;
    }

    void StartTextOff()
    {
        this.startText.SetActive(false);
    }
}
