using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    //float elapsedTime; // ����ð�
    [SerializeField] float remainingTime;

    // Update is called once per frame
    void Update()
    {
        /*elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  //����ð� 0�ʿ��� �����ϴ� ��� */

        if (remainingTime > 0) 
        {
            remainingTime -= Time.deltaTime;
        }
        else if(remainingTime < 0)
        {
            remainingTime = 0;
            timerText.color = Color.red;
        }
        if (remainingTime <= 30 && remainingTime > 0)
        {
            timerText.color = Color.yellow;
        }
        
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
