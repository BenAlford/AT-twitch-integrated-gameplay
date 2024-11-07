using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class CheckTwitchChannel : MonoBehaviour
{
    public ChannelScriptable channel_scriptable;
    public void CheckChannel()
    {
        //string authURL = "https://id.twitch.tv/oauth2/authorize?response_type=token&client_id=lqz26yxqt9usryhf6tn9mah0knd852&redirect_uri=http://localhost:3000&scope=channel%3Amanage%3Apolls+channel%3Aread%3Apolls+chat%3Aread&state=c3ab8aa609ea11e793ae92361f002671";

        //Application.OpenURL(authURL);

        string channel = GetComponent<TextMeshProUGUI>().text;
        channel = channel.Substring(0, channel.Length - 1);
        if (!string.IsNullOrEmpty(channel))
        {
            channel_scriptable.channel = channel;
            SceneManager.LoadScene("Level");
        }
    }
}
