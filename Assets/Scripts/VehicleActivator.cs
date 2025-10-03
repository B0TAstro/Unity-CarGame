using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VehicleActivator : MonoBehaviour
{
    [Header("Vehicle References")]
    public GameObject[] allVehicles;

    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera;

    public void DeactivateAllVehicles()
    {
        foreach (GameObject vehicle in allVehicles)
        {
            if (vehicle != null)
            {
                vehicle.SetActive(false);
            }
        }
        Debug.Log("Tous les véhicules désactivés"); // Debug
    }

    public void ActivateVehicleByName(string vehicleName)
    {
        DeactivateAllVehicles();
        foreach (GameObject vehicle in allVehicles)
        {
            if (vehicle != null && vehicle.name == vehicleName)
            {
                vehicle.SetActive(true);
                Debug.Log($"Véhicule activé : {vehicleName}"); // Debug
                SetCameraTarget(vehicle);
                return;
            }
        }
    }

    public void ActivateVehicleByIndex(int index)
    {
        if (index >= 0 && index < allVehicles.Length)
        {
            DeactivateAllVehicles();

            if (allVehicles[index] != null)
            {
                allVehicles[index].SetActive(true);
                SetCameraTarget(allVehicles[index]);
            }
        }
        else
        {
            Debug.LogWarning($"Index de véhicule invalide : {index}");
        }
    }

    private void SetCameraTarget(GameObject vehicle)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = vehicle.transform;
            virtualCamera.LookAt = vehicle.transform;        }
    }
}
