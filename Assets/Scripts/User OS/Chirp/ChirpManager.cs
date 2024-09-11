using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChirpManager : MonoBehaviour
{
    [SerializeField] OSManager osManager;
    private FeedPostsManager feedManager = new();
    [SerializeField] private GameObject userPost;
    [SerializeField] private GameObject linkPost;
    [SerializeField] private GameObject photoPost;
    [SerializeField] private Transform FeedBox;
    private GameObject newPost;
    private FeedPost currPost;
    
    void Start(){
        feedManager.StartDialogue(osManager.GetNextFeedEvent());
        while(!feedManager.IsDialogueQueueEmpty()){
            SpawnPosts(feedManager.GetNextPost());
        }
        
    }
    public void NewFeedEvent(){
        gameObject.SetActive(true);
        foreach(Transform child in FeedBox.transform){
            Destroy(child.gameObject);
        }
        feedManager.StartDialogue(osManager.GetNextFeedEvent());
        while(!feedManager.IsDialogueQueueEmpty()){
            SpawnPosts(feedManager.GetNextPost());
        }
    }

    public void SpawnPosts(FeedPost feedData){
        currPost = feedData;
        if(!feedData.linkText.Equals("")){
            SpawnNewPost(linkPost);
            SetLink();
        }
        else if(feedData.postPic!=null){
            SpawnNewPost(photoPost);
            SetPhoto();
        }
        else{
            SpawnNewPost(userPost);
        }
    }
    public void SpawnNewPost(GameObject post){
        newPost = Instantiate(post,FeedBox,false);
        TextMeshProUGUI postTxt = newPost.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameTxt = newPost.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI usernameTxt = newPost.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Image userImg = newPost.transform.GetChild(3).GetComponent<Image>();
        userImg.sprite = currPost.userPic;
        postTxt.text = currPost.postText;
        nameTxt.text = currPost.charName;
        usernameTxt.text = currPost.username;
        //newPost.transform.SetParent(FeedBox,false);
    }
    public void SetLink(){
        TextMeshProUGUI linkTxt = newPost.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        linkTxt.text = currPost.linkText;
        //newPost.transform.SetParent(FeedBox,false);
    }
    public void SetPhoto(){
        Image postImg = newPost.transform.GetChild(4).GetComponent<Image>();
        postImg.sprite = currPost.postPic;
        //newPost.transform.SetParent(FeedBox,false);
    }
}
