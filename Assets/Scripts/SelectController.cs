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

    [Header("Vehicle Settings")]
    public GameObject[] vehiclePrefabs;

    [Header("UI Buttons")]
    public Button prevButton;
    public Button nextButton;
    public Button goButton;

    [Header("Vehicle Selection")]
    public int currentIndex = 0;

    [Header("Events")]
    public UnityEvent<int> onVehicleChanged; // Event quand on change de véhicule
    public UnityEvent<GameObject> onVehicleSelected; // Event quand on sélectionne avec Go!

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
        if (goButton != null)
        {
            goButton.onClick.AddListener(ConfirmSelection);
        }
        UpdateButtonStates();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectNext();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectPrevious();
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
        }
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
        }
    }

    public void ConfirmSelection()
    {
        if (vehiclePrefabs.Length > 0 && currentIndex < vehiclePrefabs.Length)
        {
            GameObject selectedVehicle = vehiclePrefabs[currentIndex];
            Debug.Log($"GO! Véhicule sélectionné : {selectedVehicle.name} (Index: {currentIndex})");
            onVehicleSelected?.Invoke(selectedVehicle);

            // Ici vous pouvez ajouter la logique pour charger la scène de jeu, etc.
        }
        else
        {
            Debug.LogWarning("Aucun véhicule à sélectionner ou index invalide!");
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

    public GameObject GetCurrentVehicle()
    {
        if (vehiclePrefabs.Length > 0 && currentIndex < vehiclePrefabs.Length)
        {
            return vehiclePrefabs[currentIndex];
        }
        return null;
    }

    public string GetCurrentVehicleName()
    {
        GameObject vehicle = GetCurrentVehicle();
        return vehicle != null ? vehicle.name : "Aucun";
    }
}