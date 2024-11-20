using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattersDisplay : MonoBehaviour
{
    public bool helpersShown;
    public ChannelScriptable channelScriptable;
    // Start is called before the first frame update
    void Start()
    {
        string names = "";
        if (helpersShown)
        {
            for (int i = 0; i < channelScriptable.helpers.Count; i++)
            {
                if (i >= channelScriptable.helpers.Count - 1)
                {
                    names += " and ";
                }
                else if (i > 0)
                {
                    names += ", ";
                }
                names += channelScriptable.helpers[i];
            }
            names += " helped!";
        }
        else
        {
            for (int i = 0; i < channelScriptable.enemies.Count; i++)
            {
                if (i >= channelScriptable.enemies.Count-1)
                {
                    names += " and ";
                }
                else if (i > 0)
                {
                    names += ", ";
                }
                names += channelScriptable.enemies[i];
            }
            names += " hindered!";
        }
        GetComponent<TextMeshProUGUI>().text = names;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
