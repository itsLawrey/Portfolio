using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    /// <summary>
    /// Add listener to update value display.
    /// </summary>
    void Start()
    {
        _slider.onValueChanged.AddListener(v =>
        {
            _sliderText.text = v.ToString();
        });
    }

    void Update()
    {
        SaveRounds();
    }

    /// <summary>
    /// Saves the chosen amount of rounds for other scripts to read.
    /// </summary>
    public void SaveRounds()
    {
        int rounds = (int)_slider.value;
        PlayerPrefs.SetInt("NumberOfRounds", rounds);
    }
}
