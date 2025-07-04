using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.Events;

public class TwitchIntegration : MonoBehaviour
{
    public UnityEvent<string, string> OnChatMessage;

    TcpClient twitch;
    StreamReader reader;
    StreamWriter writer;

    const string url = "irc.chat.twitch.tv";
    const int port = 6667;

    string user = "BenTheGameDev";
    string OAuth = "oauth:authpcswx7ytnvdetwmscysxwom0hw";
    string channel = "BenTheGameDev";

    public ChannelScriptable channel_scriptable;

    float ping_counter = 0;

    private void ConnectToTwitch()
    {
        twitch = new TcpClient(url, port);
        reader = new StreamReader(twitch.GetStream());
        writer = new StreamWriter(twitch.GetStream());

        writer.WriteLine("PASS " + OAuth);
        writer.WriteLine("NICK " + user.ToLower());
        writer.WriteLine("JOIN #" + channel.ToLower());
        writer.Flush();
    }

    // Start is called before the first frame update
    void Awake()
    {
        channel = channel_scriptable.channel;
        ConnectToTwitch();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        ping_counter += Time.deltaTime;
        if (ping_counter > 60)
        {
            writer.WriteLine("PING " + url);
            writer.Flush();
            ping_counter = 0;
        }
        if (!twitch.Connected)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            ConnectToTwitch();
        }
        if (twitch.Available > 0)
        {
            string message = reader.ReadLine();
            if (message.Contains("PRIVMSG"))
            {
                string name = message.Substring(1, message.IndexOf("!") - 1);
                string msg = message.Substring(message.IndexOf(":",1) + 1, message.Length - message.IndexOf(":", 1) - 1);
                print(name);
                print(msg);
                OnChatMessage?.Invoke(name, msg);
            }
        }
    }
}
