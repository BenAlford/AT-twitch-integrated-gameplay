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
    [SerializeField] InputBehaviour input;

    const string url = "irc.chat.twitch.tv";
    const int port = 6667;

    string username = "BenTheGameDev";
    string OAuthToken = "oauth:aqvkhqc4stflmciixkaa9q8u7vsr9o";
    string channel;

    public ChannelScriptable channel_scriptable;

    float ping_timer = 0;

    private void Connect()
    {
        twitch = new TcpClient(url, port);
        reader = new StreamReader(twitch.GetStream());
        writer = new StreamWriter(twitch.GetStream());

        writer.WriteLine("PASS " + OAuthToken);
        writer.WriteLine("NICK " + username.ToLower());
        writer.WriteLine("JOIN #" + channel.ToLower());
        writer.Flush();
    }

    void Awake()
    {
        channel = channel_scriptable.channel;
        Connect();
    }

    void Update()
    {
        // ping keeps the connection alive
        GetComponent<SpriteRenderer>().color = Color.green;
        ping_timer += Time.deltaTime;
        if (ping_timer > 60)
        {
            writer.WriteLine("PING " + url);
            writer.Flush();
            ping_timer = 0;
        }

        // if there is a message
        while (twitch.Available > 0)
        {
            string chat_message = reader.ReadLine();
            print(chat_message);
            // processes the message if it is a twitch chat message
            if (chat_message.Contains("PRIVMSG"))
            {
                string viewer_name = chat_message.Substring(1, chat_message.IndexOf("!") - 1);
                string viewer_msg = chat_message.Substring(chat_message.IndexOf(":",1) + 1, chat_message.Length - chat_message.IndexOf(":", 1) - 1);
                print(viewer_name);
                print(viewer_msg);
                input.OnChatMessage(viewer_name, viewer_msg);
            }
        }

        if (!twitch.Connected)
        {
            // reconnects to twitch if it gets disconnected
            GetComponent<SpriteRenderer>().color = Color.red;
            Connect();
        }
    }
}
