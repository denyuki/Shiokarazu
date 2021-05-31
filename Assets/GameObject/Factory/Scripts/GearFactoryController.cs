using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearFactoryController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    //z軸の移動用の変数
    float frontNum = 0f;
    float backNum = 1f;

    [SerializeField] GameObject GearOneSwitch;
    [SerializeField] GameObject GearOneText;

    [SerializeField] GameObject GearTwoSwitch;
    [SerializeField] GameObject GearTwoText;

    [SerializeField] GameObject GearThreeSwitch;
    [SerializeField] GameObject GearThreeText;

    int GearSwitchOnSorting = 6;
    int GearSwitchOffSorting = 0;

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

        this.GearOneSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.GearSwitchOnSorting;
        this.GearTwoSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.GearSwitchOnSorting;
        this.GearThreeSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.GearSwitchOnSorting;

        this.GearOneText.SetActive(true);
        this.GearTwoText.SetActive(true);
        this.GearThreeText.SetActive(true);
    }

    //gearFactoryを後ろ側にする関数
    public void GearFactoryOff()
    {
        this.spriteRenderer.sortingOrder = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y, this.backNum);

        this.GearOneSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.GearSwitchOffSorting;
        this.GearTwoSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.GearSwitchOffSorting;
        this.GearThreeSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.GearSwitchOffSorting;

        this.GearOneText.SetActive(false);
        this.GearTwoText.SetActive(false);
        this.GearThreeText.SetActive(false);
    }
}
