using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSManager : MonoBehaviour
{
    private FeedEvent tmp;
    public FeedEvent GetNextFeedEvent(){
        return tmp;
    }
}
