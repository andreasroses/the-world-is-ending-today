using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public enum NewsType{
    Conflict, Oils, Stocks
}
public class WebViewer : MonoBehaviour
{
    
    
    private WebpageLoader webpageLoader;

    void Awake(){
        webpageLoader = GetComponent<WebpageLoader>();
    }

    public void LoadNews(NewsType news){
        webpageLoader.UpdateViewport(news);
    }

    
}
