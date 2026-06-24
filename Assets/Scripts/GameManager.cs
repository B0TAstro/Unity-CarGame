using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Selected Vehicle")]
    public string selectedVehicleName = "car_bus";

    [Header("Day/Night (persisté entre les scènes)")]
    public bool isNight = false;

    [Header("Scene Indices")]
    public int introSceneIndex = 0;
    public int selectCarSceneIndex = 2;
    public int gameSceneIndex = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // Une instance persiste déjà (ex: venant de l'Intro). On reprend
            // sa sélection puis on la remplace par l'instance de cette scène,
            // afin que les boutons du menu (référencés dans la scène) restent
            // liés à un objet vivant. Sinon les clics ne font rien.
            selectedVehicleName = Instance.selectedVehicleName;
            isNight = Instance.isNight;
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == gameSceneIndex)
        {
            ActivateSelectedVehicle();
        }
    }

    public void SetSelectedVehicle(string vehicleName)
    {
        selectedVehicleName = vehicleName;
        Debug.Log($"Véhicule sélectionné sauvegardé : {vehicleName}"); // Debug
    }

    public void SetSelectedVehicle(GameObject vehiclePrefab)
    {
        if (vehiclePrefab != null)
        {
            selectedVehicleName = vehiclePrefab.name;
            Debug.Log($"Véhicule sélectionné sauvegardé : {selectedVehicleName}"); // Debug
        }
    }

    private void ActivateSelectedVehicle()
    {
        VehicleActivator activator = FindObjectOfType<VehicleActivator>();
        if (activator != null)
        {
            activator.ActivateVehicleByName(selectedVehicleName);
        }
        else
        {
            Debug.LogWarning("VehicleActivator non trouvé dans la scène de jeu!"); // Debug
        }
    }

    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void StartGameFromIntro()
    {
        selectedVehicleName = "car_bus";
        SwitchScene(gameSceneIndex);
    }

    public void StartGameFromSelect()
    {
        SwitchScene(gameSceneIndex);
    }

    public void GoToSelectCar()
    {
        SwitchScene(selectCarSceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}