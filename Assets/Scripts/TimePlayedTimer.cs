using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class TimePlayedTimer : MonoBehaviour
{
    // from YouTube video: https://www.youtube.com/watch?v=qc7J0iei3BU

    public static TimePlayedTimer instance;

    public Text timeCounter;

    private TimeSpan timePlaying;
    private bool timerRunning;
    private float startTime;

    private float elapsedTime;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeCounter.text = "Time: 00:00.00";
        timerRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer()
    {
        timerRunning = true;
        startTime = Time.time;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerRunning = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;

            yield return null;
        }
    }
}
