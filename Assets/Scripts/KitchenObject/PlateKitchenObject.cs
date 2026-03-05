using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {
    
    public event Action<SO_KitchenObject> OnIngredientAdded;

    // Liste des objets acceptés (ex: Pain, Steak cuit, Tomate tranchée) définie dans l'Inspecteur
    [SerializeField] private List<SO_KitchenObject> validKitchenObjectSOList;
    
    private List<SO_KitchenObject> kitchenObjectSOList = new List<SO_KitchenObject>();

    // Dans PlateKitchenObject.cs
    public bool TryAddIngredient(SO_KitchenObject kitchenObjectSO) {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) return false;
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) return false;

        kitchenObjectSOList.Add(kitchenObjectSO);
        
        // On prévient le visuel qu'on a ajouté cet ingrédient
        OnIngredientAdded?.Invoke(kitchenObjectSO);
        return true;
    }

    public List<SO_KitchenObject> GetKitchenObjectSOList() => kitchenObjectSOList;
}