using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FeedsDatabase : ScriptableObject
{
    public List<FeedEvent> allFeeds;
}
