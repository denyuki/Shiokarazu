using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGenerator : MonoBehaviour
{
    [SerializeField] Slider slider;

    [SerializeField] float gDwounPow = 0.2f;
    [SerializeField] float gUpPow = 1;
    float gPow = 1;

    public bool powrUp = false;


    // Update is called once per frame
    void Update()
    {
        if (powrUp)
        {
            gPow += gUpPow * Time.deltaTime;
            slider.value = gPow;
        }
        else
        {
            gPow -= gDwounPow * Time.deltaTime;
            slider.value = gPow;
        }
    }

}
