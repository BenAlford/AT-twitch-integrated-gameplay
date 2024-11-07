using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TestTwitchApp : MonoBehaviour
{
    public TextMeshProUGUI access_token_text;
    public ChannelScriptable channelScriptable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryHere()
    {
        StartCoroutine(TryAPI());
    }
    public IEnumerator TryAPI()
    {
        //string access_token = access_token_text.text;
        //channelScriptable.access_token = access_token;
        string access_token = channelScriptable.access_token;
        string client_ID = channelScriptable.client_id;

        string twitchAPIURL = "https://api.twitch.tv/helix/extensions";

        UnityWebRequest request = UnityWebRequest.Get(twitchAPIURL);
        request.SetRequestHeader("Client-ID", client_ID);
        request.SetRequestHeader("Authorization", "Bearer " + access_token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the response (this is a JSON string)
            string jsonResponse = request.downloadHandler.text;

            // For now, just output the response to the console
            Debug.Log("Response: " + jsonResponse);

        }
        else
        {
            Debug.Log("Error " +  request.error);
        }
    }
}
