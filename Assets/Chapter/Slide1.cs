﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slide1 : MonoBehaviour
{

    [SerializeField] Animator sliderAnimator;
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
        if (number == 0)
        {
            sliderGameObject.active = true;
            sliderAnimator.SetBool("Slider", true);
        }
        else
        {
            SceneManager.LoadScene("ruka");
        }
    }
}
