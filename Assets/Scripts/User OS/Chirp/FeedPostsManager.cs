using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FeedPostsManager{
    private Queue<FeedPost> currPosts = new();

    public void StartDialogue(FeedEvent f){
        List<FeedPost> tmpPosts = f.FeedPosts;
        queuePosts(tmpPosts);
    }


    private void queuePosts(List<FeedPost> postTxts){
        foreach(FeedPost post in postTxts){
            currPosts.Enqueue(post);
        }
    }

    public FeedPost GetNextPost(){
        return currPosts.Dequeue();
    }

    public bool IsDialogueQueueEmpty(){
        if(currPosts.Any()){
            return false;
        }
        return true;
    }
}
