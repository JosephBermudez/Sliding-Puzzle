using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Text timer;
    public int seconds,minutes;

    private void Start()
    {
        StartTimer();
    }



    public void StartTimer()
    {
        seconds++;
        if (seconds > 59)
        {
            minutes++;
            seconds = 0;
        }
        timer.text = minutes.ToString("00") + " : " + seconds.ToString("00");
        Invoke(nameof(StartTimer), 1);
    }

    public void StopTimer()
    {
        CancelInvoke(nameof(StartTimer));
        timer.gameObject.SetActive(false);
    }
}
