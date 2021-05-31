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

    // Start is called before the first frame update
    void Start()
    {
        this.gearDirector = GetComponent<GearDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        //クリアしているか
        if (generator.GearConnect())
        {
            Debug.LogError("クリア");

            this.clearText.SetActive(true);

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
