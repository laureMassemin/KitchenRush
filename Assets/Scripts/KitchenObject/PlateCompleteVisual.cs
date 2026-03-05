using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public SO_KitchenObject kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    private void Start() {
        // On cache tout au début
        foreach (KitchenObjectSO_GameObject item in kitchenObjectSOGameObjectList) {
            item.gameObject.SetActive(false);
        }

        // On s'abonne à l'ajout d'ingrédients
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(SO_KitchenObject kitchenObjectSO) {
        foreach (KitchenObjectSO_GameObject item in kitchenObjectSOGameObjectList) {
            if (item.kitchenObjectSO == kitchenObjectSO) {
                item.gameObject.SetActive(true);
            }
        }
    }
}