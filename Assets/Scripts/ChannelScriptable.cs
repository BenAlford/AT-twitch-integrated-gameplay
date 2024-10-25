using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Channel Name")]
public class ChannelScriptable : ScriptableObject
{
    public string channel;
    public float viewer_cooldown;
}
