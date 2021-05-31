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

    Image timerImage;
    float MaxLimit = 0;
    public float timeLimit = 100f;

    [SerializeField]
    GameObject startText;

    [SerializeField]
    GameObject stageGearText;
    Text stageGearPower;

    [SerializeField] GameObject directorState;
    GameDirectorState director;

    [SerializeField] string gameOverName;

    [SerializeField] GameObject GameOverText;

    // Start is called before the first frame update
    void Start()
    {
        this.cameraScrollbar = this.scrollbar.GetComponent<Scrollbar>();
        this.timerImage = this.Timer.GetComponent<Image>();

        this.leftEndPosition = this.stageLeftEnd.transform.position.x + this.leftEndPosition;
        this.rightEndPosition = this.stageRightEnd.transform.position.x + this.rightEndPosition;

        //スタートのテキストをオンにする
        this.startText.SetActive(true);
        Invoke("StartTextOff", 2f);

        this.MaxLimit = this.timeLimit;

        this.stageGearPower = stageGearText.GetComponent<Text>();
        this.stageGearText.SetActive(false);

        this.director = this.directorState.GetComponent<GameDirectorState>();
    }

    // Update is called once per frame
    void Update()
    {
        //最大値以上には回復しません
        if(this.timeLimit >= this.MaxLimit)
        {
            this.timeLimit = this.MaxLimit;
        }

        //ゲージを少しずつ減らしていく
        if(this.timeLimit > 0)
        {
            this.timeLimit -= Time.deltaTime;
            this.timerImage.fillAmount = this.timeLimit / this.MaxLimit;
        }
        else
        {
            this.GameOverText.SetActive(true);

            Invoke("Change", 2.0f);
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

        if(hit2d.collider != null){
            if (hit2d.collider.gameObject.tag == Common.StageGear)
            {
                this.stageGearText.SetActive(true);
                this.stageGearPower.text = " " + hit2d.collider.gameObject.GetComponent<GearState>().ReturnGearReceivePower();
            }
            else
            {
                this.stageGearText.SetActive(false);
            }
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

    void Change()
    {
        this.director.ChangeScene(this.gameOverName);
    }
}
