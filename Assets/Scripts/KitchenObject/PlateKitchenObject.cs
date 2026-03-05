using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {
    
    public event Action<SO_KitchenObject> OnIngredientAdded; // Pour mettre à jour le visuel

    [SerializeField] private List<SO_KitchenObject> validKitchenObjectSOList; // Liste des objets acceptés (Pain, Steak cuit, Tomate tranchée)
    
    private List<SO_KitchenObject> kitchenObjectSOList = new List<SO_KitchenObject>();

    public bool TryAddIngredient(SO_KitchenObject kitchenObjectSO) {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false; // Pas un ingrédient valide pour un burger
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false; // Déjà présent sur l'assiette
        } else {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(kitchenObjectSO);
            return true;
        }
    }

    public List<SO_KitchenObject> GetKitchenObjectSOList() => kitchenObjectSOList;
}