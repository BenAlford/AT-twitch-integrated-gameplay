using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputBehaviour : MonoBehaviour
{
    Dictionary<string, float> viewer_last_command_time = new Dictionary<string, float>();
    public TextMeshProUGUI input_text;
    public TilemapManager tilemap_manager;
    public float cooldown = 10;
    public ChannelScriptable channelScriptable;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = channelScriptable.viewer_cooldown;
    }

    // checks if the message is a valid command
    void ParseInput(string user, string text)
    {
        // the command needs to be in the form: !place <block> <x> <y> <username>
        if (text[0] == '!')
        {
            text = text.Substring(1);
            string[] words = text.Split(' ');
            if (words.Length == 5 && words[0] == "place")
            {
                if (cooldown == 0)
                {
                    tilemap_manager.PlaceTile(words[1], words[2], words[3], words[4]);
                }
                else
                {
                    // only lets individual viewers place blocks with a specified cooldown
                    if (viewer_last_command_time.TryGetValue(words[4], out float last_command_time))
                    {
                        if (Time.time - last_command_time > cooldown)
                        {
                            tilemap_manager.PlaceTile(words[1], words[2], words[3], words[4]);
                            viewer_last_command_time[words[4]] = Time.time;
                        }
                    }
                    else
                    {
                        // adds user to the dictionary if this is their first message
                        tilemap_manager.PlaceTile(words[1], words[2], words[3], words[4]);
                        viewer_last_command_time.Add(words[4], Time.time);
                    }
                }
            }
        }
    }

    public void OnChatMessage(string user, string msg)
    {
        ParseInput(user, msg);
    }
}
