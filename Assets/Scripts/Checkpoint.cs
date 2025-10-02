using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : BaseController<Checkpoint>
{
    public bool isPassed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPassed = true;
            Debug.Log("Voiture a traversé l’arche !"); // Debug log
        }
    }
}