using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderTextValue : MonoBehaviour
{
    private Text sliderValue;
    // Start is called before the first frame update
    private void Start()
    {
        sliderValue = GetComponent<Text>();
        sliderValue.text = GetComponentInParent<Slider>().value.ToString();
    }
    public void UpdateTextValue(float value)
    {
        sliderValue.text = value.ToString();
    }
    public void UpdateTextValueF1(float value)
    {
        sliderValue.text = value.ToString("F1");
    }
}
