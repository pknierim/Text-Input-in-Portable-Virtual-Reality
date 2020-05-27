using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    public GameObject Glass;
    public GameObject Light;

    public Slider slider;
    public Toggle toggle;

    public Text textName;

    public Material[] materials;

    public int count = 0;

    void Update()
    {
        Glass.GetComponent<Renderer>().material.SetFloat("_BumpPower", slider.value);    
        Light.SetActive(toggle.isOn);
        textName.text = Glass.GetComponent<Renderer>().material.name;   
    }

    public void Next()
    {
        if (count < materials.Length - 1)
        {
            count++;
            Glass.GetComponent<Renderer>().material = materials[count];
        }
    }

    public void Back()
    {
        if (count > 0)
        {
            count--;
            Glass.GetComponent<Renderer>().material = materials[count];
        }
    }
}