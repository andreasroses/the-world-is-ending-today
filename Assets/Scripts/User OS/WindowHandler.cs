using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour{
    public void CloseWindow(){
        gameObject.SetActive(false);
    }
}
