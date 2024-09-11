using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WebpageLoader : MonoBehaviour
{
    [SerializeField] private Scrollbar sc;
    [SerializeField] private List<Sprite> pgImgList;
    [SerializeField] private Image currImage;
    [SerializeField] private RectTransform webpageRes;
    [SerializeField] private int lastHomeRes;
    [SerializeField] private Vector2 longPageRes;
    [SerializeField] private Vector2 homePageRes;
    void Start(){
        currImage.sprite = pgImgList[0];
        webpageRes.sizeDelta = longPageRes;
        webpageRes.position = new Vector3(webpageRes.position.x, (longPageRes.y / 2 * -1) + 1, webpageRes.position.z);
        //sc.size = 1;
    }

    public void UpdateViewport(NewsType news){
        currImage.sprite = pgImgList[0];
        if(1 > lastHomeRes){
            webpageRes.sizeDelta = longPageRes;
            webpageRes.position = new Vector3(webpageRes.position.x, longPageRes.y / 2 * -1, webpageRes.position.z);
            return;
        }
        webpageRes.sizeDelta = homePageRes;
        return;
    }

}
