using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    private int totalHour = 0;
    private int totalMinute = 0;
    private int totalSecond = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioController.Instance.StopBGM();

        CalculateTime();

        transform.Find("TxtJumps").GetComponent<TMP_Text>().text = GameManager.Instance.GetTotalJumps().ToString();
        transform.Find("TxtSoars").GetComponent<TMP_Text>().text = GameManager.Instance.GetTotalSoars().ToString();
        transform.Find("TxtStamina").GetComponent<TMP_Text>().text = GameManager.Instance.GetLeftStamina().ToString("N2");

        CalculateDeath();
    }

    private void CalculateTime()
    {
        float totalTime = GameManager.Instance.GetTotalPlaytime();

        while (totalTime >= 3600f)
        {
            totalHour++;
            totalTime -= 3600f;
        }

        while (totalTime >= 60f)
        {
            totalMinute++;
            totalTime -= 60f;
        }

        while (totalTime >= 1f)
        {
            totalSecond++;
            totalTime -= 1f;
        }

        transform.Find("TxtTime").GetComponent<TMP_Text>().text = 
            totalHour.ToString("D2") + ":" + totalMinute.ToString("D2") + ":" + totalSecond.ToString("D2");
    }

    private void CalculateDeath()
    {
        int deaths = GameManager.Instance.GetMaxLives() - GameManager.Instance.GetLives();
        transform.Find("TxtDeaths").GetComponent<TMP_Text>().text = deaths.ToString();
    }
}
