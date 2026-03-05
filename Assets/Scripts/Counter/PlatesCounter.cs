using UnityEngine;
using System;

public class PlatesCounter : BaseCounter {

    public event Action OnPlateSpawned;
    public event Action OnPlateRemoved;

    [SerializeField] private SO_KitchenObject plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;
            if (platesSpawnedAmount < platesSpawnedAmountMax) {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke();
            }
        }
    }

    public override void Interact() {
        if (platesSpawnedAmount > 0) { // Si il y a des assiettes dispo
            platesSpawnedAmount--;
            // On instancie l'assiette et on la donne au joueur
            // (Logique identique à celle de ton ContainerCounter)
            // Crée l'objet à partir du Prefab stocké dans le Scriptable Object
            GameObject kitchenObjectTransform = Instantiate(plateKitchenObjectSO.prefab);
            KitchenObject ko = kitchenObjectTransform.GetComponent<KitchenObject>();
            
            // Donne l'objet au comptoir
            ko.SetKitchenObjectParent(GetSpawnPoint());
            SetKitchenObject(ko);
            OnPlateRemoved?.Invoke();
        }
    }
}