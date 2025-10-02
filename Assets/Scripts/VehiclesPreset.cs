using UnityEngine;

[CreateAssetMenu(fileName = "Vehicles", menuName = "ScriptableObject/VehiclesPreset")]
public class VehiclesPreset : ScriptableObject
{
    [Header("Vehicle Settings")]
    [Space(10)]
    [Tooltip("The maximum speed that the car can reach in km/h")]
    [Range(20, 190)]
    public int maxSpeed = 90;
    
    [Tooltip("How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest")]
    [Range(1, 10)]
    public int accelerationMultiplier = 5;
    
    [Tooltip("How much grip the car loses when the user hit the handbrake")]
    [Range(1, 10)]
    public int handbrakeDriftMultiplier = 5;
    
    public GameObject VehiclePrefab; // The prefab of the car.
}
