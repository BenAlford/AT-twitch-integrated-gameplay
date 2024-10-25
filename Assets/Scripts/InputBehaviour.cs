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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    if (input_text.text != "")
        //    {
        //        ParseInput(input_text.text);
        //    }
        //}
    }

    void ParseInput(string user, string text)
    {
        if (text[0] == '!')
        {
            text = text.Substring(1);
            string[] words = text.Split(' ');
            if (words.Length == 4 && words[0] == "place")
            {
                if (viewer_last_command_time.TryGetValue(user, out float last_command_time))
                {
                    if (Time.time - last_command_time > cooldown)
                    {
                        tilemap_manager.PlaceTile(words[1], words[2], words[3]);
                        viewer_last_command_time[user] = Time.time;
                    }
                }
                else
                {
                    tilemap_manager.PlaceTile(words[1], words[2], words[3]);
                    viewer_last_command_time.Add(user, Time.time);
                }
            }
        }
    }

    public void OnChatMessage(string user, string msg)
    {
        ParseInput(user, msg);
    }
}
