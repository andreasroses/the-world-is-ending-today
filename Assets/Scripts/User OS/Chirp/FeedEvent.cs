using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FeedEvent : ScriptableObject
{
    public string EventName;
    public List<FeedPost> FeedPosts;
}