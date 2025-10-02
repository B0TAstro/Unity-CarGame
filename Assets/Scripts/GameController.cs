using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : BaseController<GameController>
{
    [Header("Timer Settings")]
    public float startTime = 300f;

    [Header("UI References")]
    public TextMeshProUGUI timerText;

    [Header("Checkpoint Settings")]
    public GameObject[] arches;
    public float displayInterval = 10f;

    private float currentTime;
    private bool timerRunning;
    private int currentArchIndex = 0;
    private float nextArchTime;

    void Start()
    {
        InitializeTimer();
        InitializeArches();
    }

    void Update()
    {
        if (timerRunning)
        {
            UpdateTimer();
            UpdateArches();
        }
    }

    private void InitializeArches()
    {
        foreach (GameObject arch in arches)
        {
            if (arch != null)
            {
                arch.SetActive(false);
            }
        }

        if (arches.Length > 0 && arches[0] != null)
        {
            arches[0].SetActive(true);
            currentArchIndex = 0;
            nextArchTime = displayInterval;
            Debug.Log("Arche 0 affichée");
        }
    }

    private void UpdateArches()
    {
        if (currentArchIndex < arches.Length - 1 && currentTime <= (startTime - nextArchTime))
        {
            if (arches[currentArchIndex] != null)
            {
                arches[currentArchIndex].SetActive(false);
            }

            currentArchIndex++;
            if (arches[currentArchIndex] != null)
            {
                arches[currentArchIndex].SetActive(true);
                Debug.Log($"Arche {currentArchIndex} affichée");
            }

            nextArchTime += displayInterval;
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
            timerText.text = string.Format("Timer : {0}:{1:00}", minutes, seconds);
        }
    }

    private void OnTimerEnd()
    {
        Debug.Log("Time Over!");
        
        foreach (GameObject arch in arches)
        {
            if (arch != null)
            {
                arch.SetActive(false);
            }
        }
    }

    public void ShowNextArch()
    {
        if (currentArchIndex < arches.Length - 1)
        {
            if (arches[currentArchIndex] != null)
            {
                arches[currentArchIndex].SetActive(false);
            }

            currentArchIndex++;
            if (arches[currentArchIndex] != null)
            {
                arches[currentArchIndex].SetActive(true);
                Debug.Log($"Arche {currentArchIndex} affichée (manuel)");
            }
        }
    }

    public int GetCurrentArchIndex()
    {
        return currentArchIndex;
    }
}