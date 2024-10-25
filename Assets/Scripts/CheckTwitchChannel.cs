using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckTwitchChannel : MonoBehaviour
{
    public ChannelScriptable channel_scriptable;
    public void CheckChannel()
    {
        string channel = GetComponent<TextMeshProUGUI>().text;
        channel = channel.Substring(0, channel.Length - 1);
        if (!string.IsNullOrEmpty(channel))
        {
            channel_scriptable.channel = channel;
            SceneManager.LoadScene("Level");
        }
    }
}
