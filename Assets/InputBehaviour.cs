using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputBehaviour : MonoBehaviour
{
    public TextMeshProUGUI input_text;
    public TilemapManager tilemap_manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input_text.text != "")
            {
                ParseInput(input_text.text);
            }
        }
    }

    void ParseInput(string text)
    {
        if (text[0] == '!')
        {
            text = text.Substring(1);
            string[] words = text.Split(' ');
            if (words.Length == 3 && words[0] == "place")
            {
                tilemap_manager.PlaceTile(words[1], words[2]);
            }
        }
    }

    public void OnChatMessage(string user, string msg)
    {
        ParseInput(msg);
    }
}
