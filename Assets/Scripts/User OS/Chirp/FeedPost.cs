using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FeedPost : ScriptableObject
{
    public string charName;
    public string username;
    public string postText;
    public string linkText;
    public Sprite userPic;
    public Sprite postPic;
}