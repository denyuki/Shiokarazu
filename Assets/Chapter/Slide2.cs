using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide2 : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject sliderGameObject;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(int number)
    {
        sliderGameObject.active = true;
        animator.SetBool("Slider", true);
        Debug.Log(number);
    }
}
