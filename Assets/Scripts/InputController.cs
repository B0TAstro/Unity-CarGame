using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputController : BaseController<InputController>
{
    public delegate void EmptyEvent();
    public EmptyEvent OnInputPauseToggle;
    
    public GameObject pausePanel;
    
    void Update()
    {
        // Détecter la touche Échap pour la pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel != null)
                pausePanel.SetActive(!pausePanel.activeSelf);
            if (OnInputPauseToggle != null)
                OnInputPauseToggle.Invoke();
        }
    }
}
