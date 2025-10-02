using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : BaseController<GameController>
{
    [Header("Timer Settings (in seconds)")]
    public float startTime = 300f;
    
    [Header("Text UI References")]
    public TextMeshProUGUI timerText;
    
    private float currentTime;
    private bool timerRunning;
    
    void Start()
    {
        InitializeTimer();
    }
    
    void Update()
    {
        if (timerRunning)
        {
            UpdateTimer();
        }
    }
    
    private void InitializeTimer()
    {
        currentTime = startTime;
        timerRunning = true;
        UpdateTimerDisplay();
    }
    
    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            currentTime = 0;
            timerRunning = false;
            OnTimerEnd();
        }
    }
    
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("Time : {0}:{1:00}", minutes, seconds);
        }
    }
    
    private void OnTimerEnd()
    {
        Debug.Log("Time over!");
    }
}