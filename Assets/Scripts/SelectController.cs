using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public Vector3[] cameraPositions;
    public float transitionSpeed = 5f;
    
    [Header("UI Buttons")]
    public Button prevButton;
    public Button nextButton;
    
    [Header("Vehicle Selection")]
    public int currentIndex = 0;

    [Header("Events")]
    public UnityEvent<int> onVehicleChanged;
    
    private bool isTransitioning = false;
    
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }        
        if (cameraPositions.Length > 0)
        {
            mainCamera.transform.position = cameraPositions[currentIndex];
        }        
        if (prevButton != null)
        {
            prevButton.onClick.AddListener(SelectPrevious);
        }
        
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(SelectNext);
        }        
        UpdateButtonStates();
    }
    
    void Update()
    {
        if (isTransitioning && cameraPositions.Length > 0)
        {
            Vector3 targetPosition = cameraPositions[currentIndex];
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position, 
                targetPosition, 
                Time.deltaTime * transitionSpeed
            );            
            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.01f)
            {
                mainCamera.transform.position = targetPosition;
                isTransitioning = false;
            }
        }
    }
    
    public void SelectNext()
    {
        if (currentIndex < cameraPositions.Length - 1)
        {
            currentIndex++;
            MoveCameraToCurrentIndex();
            UpdateButtonStates();
            onVehicleChanged?.Invoke(currentIndex);
            Debug.Log($"Véhicule suivant sélectionné : {currentIndex}");
        }
    }
    
    public void SelectPrevious()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            MoveCameraToCurrentIndex();
            UpdateButtonStates();
            onVehicleChanged?.Invoke(currentIndex);            
            Debug.Log($"Véhicule précédent sélectionné : {currentIndex}");
        }
    }
    
    private void MoveCameraToCurrentIndex()
    {
        if (cameraPositions.Length > 0 && currentIndex < cameraPositions.Length)
        {
            isTransitioning = true;
        }
    }
    
    private void UpdateButtonStates()
    {
        if (prevButton != null)
        {
            prevButton.gameObject.SetActive(currentIndex > 0);
        }
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(currentIndex < cameraPositions.Length - 1);
        }
    }
    
    public void SelectVehicleByIndex(int index)
    {
        if (index >= 0 && index < cameraPositions.Length)
        {
            currentIndex = index;
            MoveCameraToCurrentIndex();
            UpdateButtonStates();
            onVehicleChanged?.Invoke(currentIndex);
        }
    }
    
    public int GetCurrentIndex()
    {
        return currentIndex;
    }
    
    void OnEnable()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left Arrow Pressed");
            SelectPrevious();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right Arrow Pressed");
            SelectNext();
        }
    }
}