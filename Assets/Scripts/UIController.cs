using UnityEngine;
using UnityEngine.UI;

public class UIController : BaseController<UIController>
{
    public InputController.EmptyEvent OnUIPauseToggle;

    public Button PauseButton;
    public Button DayNightCycleButton;

    public void Start()
    {
        if (PauseButton != null)
            PauseButton.onClick.AddListener(ClickPause);

        if (DayNightCycleButton != null)
            DayNightCycleButton.onClick.AddListener(ClickDayNight);
    }
    
    public void ClickPause()
    {
        if (OnUIPauseToggle != null)
            OnUIPauseToggle.Invoke();
    }

    public void ClickDayNight()
    {
        if (GameController.Instance != null)
            GameController.Instance.ToggleDayNight();
        Debug.Log("Day Night");
    }
}