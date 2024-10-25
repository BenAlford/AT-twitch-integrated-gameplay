using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextBehaviour : MonoBehaviour
{
    public ChannelScriptable channel_scriptable;
    public GameObject slider_object;
    Slider slider;
    public TextMeshProUGUI textobj;
    // Start is called before the first frame update
    void Start()
    {
        slider = slider_object.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        textobj.text = slider.value.ToString();
        channel_scriptable.viewer_cooldown = slider.value;
    }
}
