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
    public UnityEvent<int> onVehicleChanged;
    public UnityEvent<GameObject> onVehicleSelected;

    private bool isTransitioning = false;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Démarre sur le véhicule déjà choisi (sauvegardé dans GameManager)
        InitIndexFromSavedVehicle();

        if (cameraPositions.Length > 0)
            mainCamera.transform.position = cameraPositions[currentIndex];

        if (prevButton != null)
            prevButton.onClick.AddListener(SelectPrevious);

        if (nextButton != null)
            nextButton.onClick.AddListener(SelectNext);

        if (goButton != null)
            goButton.onClick.AddListener(ConfirmSelection);

        UpdateButtonStates();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SelectNext();

        if (Input.GetKeyDown(KeyCode.RightArrow))
            SelectPrevious();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            ConfirmSelection();

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

    private void InitIndexFromSavedVehicle()
    {
        if (GameManager.Instance == null || vehiclePrefabs.Length == 0)
            return;

        string saved = GameManager.Instance.selectedVehicleName;
        for (int i = 0; i < vehiclePrefabs.Length; i++)
        {
            if (vehiclePrefabs[i] != null && vehiclePrefabs[i].name == saved)
            {
                currentIndex = i;
                break;
            }
        }

        // Garde l'index valide pour les positions caméra
        if (cameraPositions.Length > 0)
            currentIndex = Mathf.Clamp(currentIndex, 0, cameraPositions.Length - 1);

        onVehicleChanged?.Invoke(currentIndex);
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

            // 🔥 DIT AU GAMEMANAGER QUEL VÉHICULE EST CHOISI
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetSelectedVehicle(selectedVehicle);
                GameManager.Instance.StartGameFromSelect(); // change de scène vers le jeu
            }

            onVehicleSelected?.Invoke(selectedVehicle);
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
            prevButton.gameObject.SetActive(currentIndex > 0);

        if (nextButton != null)
            nextButton.gameObject.SetActive(currentIndex < cameraPositions.Length - 1);
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