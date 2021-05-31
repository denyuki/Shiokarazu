using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactoryController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    float frontNum = 0f;
    float backNum = 1f;

    [SerializeField] GameObject oilSwitch;
    [SerializeField] GameObject oilText;

    [SerializeField] GameObject beltSwitch;
    [SerializeField] GameObject beltText;

    int itemOnSortingOrder = 6;
    int itemOffSortingOrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.oilText.SetActive(false);
        this.beltText.SetActive(false);
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

        this.oilSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.itemOnSortingOrder;
        this.beltSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.itemOnSortingOrder;

        this.oilText.SetActive(true);
        this.beltText.SetActive(true);
    }

    //itemFactoryを後ろ側にする関数
    public void ItemFactoryOff()
    {
        this.spriteRenderer.sortingOrder = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y, this.backNum);

        this.oilSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.itemOffSortingOrder;
        this.beltSwitch.gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.itemOffSortingOrder;

        this.oilText.SetActive(false);
        this.beltText.SetActive(false);
    }
}
