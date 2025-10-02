using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : BaseController<GameController>
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public bool showTimer = true;

    [Header("Checkpoint Settings")]
    public GameObject[] arches;
    public float displayInterval = 10f;

    private float currentTime = 0f;
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
        }
    }

    private void UpdateArches()
    {
        if (currentArchIndex < arches.Length - 1 && currentTime >= nextArchTime)
        {
            if (arches[currentArchIndex] != null)
            {
                arches[currentArchIndex].SetActive(false);
            }
            currentArchIndex++;
            if (arches[currentArchIndex] != null)
            {
                arches[currentArchIndex].SetActive(true);
                Debug.Log($"Arche {currentArchIndex} affichée"); // Debug log
            }
            nextArchTime += displayInterval;
        }

        if (currentArchIndex == arches.Length - 1 && currentTime >= nextArchTime)
        {
            if (arches[currentArchIndex] != null && arches[currentArchIndex].activeSelf)
            {
                arches[currentArchIndex].SetActive(false);
                Debug.Log($"Dernière arche {currentArchIndex} désactivée"); // Debug log
                StopTimer();
            }
        }
    }

    private void InitializeTimer()
    {
        timerRunning = true;
        UpdateTimerDisplay();
    }

    private void UpdateTimer()
    {
        currentTime += Time.deltaTime;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(showTimer);
            if (showTimer)
            {
                int minutes = Mathf.FloorToInt(currentTime / 60f);
                int seconds = Mathf.FloorToInt(currentTime % 60f);
                timerText.text = string.Format("Timer : {0}:{1:00}", minutes, seconds);
            }
        }
    }

    private void StopTimer()
    {
        if (timerRunning)
        {
            timerRunning = false;

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(currentTime / 60f);
                int seconds = Mathf.FloorToInt(currentTime % 60f);
                timerText.text = string.Format("Bravo ! Temps final : {0} min {1:00} sec", minutes, seconds);
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
            }
        }
    }

    public void HideCurrentArch()
    {
        if (arches[currentArchIndex] != null)
        {
            arches[currentArchIndex].SetActive(false);
        }
    }
}