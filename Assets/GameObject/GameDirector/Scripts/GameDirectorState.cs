using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirectorState : MonoBehaviour
{

    [SerializeField] Generator generator;

    GearDirector gearDirector;

    public  GameObject clearText;

    public string nextScene;

    [SerializeField] GameObject SoundObject;
    SoundManager soundManager;
    bool canPlaySound = true;

    // Start is called before the first frame update
    void Start()
    {
        this.gearDirector = GetComponent<GearDirector>();
        this.soundManager = this.SoundObject.GetComponent<SoundManager>();
        this.canPlaySound = true;
    }

    // Update is called once per frame
    void Update()
    {
        //クリアしているか
        if (generator.GearConnect())
        {
            Debug.LogError("クリア");

            this.clearText.SetActive(true);

            if (this.canPlaySound)
            {
                this.soundManager.PlayClearSound();
                this.canPlaySound = false;
            }

            //電流をオンにする
            this.gearDirector.AllCurrentOn();

            

            Invoke("ChangeScene", 2.0f);
        }

    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(this.nextScene);
    }

    public void ChangeScene(string st)
    {
        SceneManager.LoadScene(st);
    }

}
