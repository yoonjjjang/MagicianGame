using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChange : MonoBehaviour
{
    public Sprite[] changeImg;
    Image thisImg;
    void Start()
    {
        thisImg = GetComponent<Image>();   
    }


    public void ChangeImage(int ItemIndex)
    {
        thisImg.sprite = changeImg[ItemIndex];
    }
}
