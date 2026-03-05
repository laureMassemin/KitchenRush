using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    [Serializable]
    public struct PlateElement {
        public SO_KitchenObject kitchenObjectSO;
        public GameObject lowerPartPrefab; // Le bas du pain ou l'ingrédient simple
        public float lowerYOffset;
        public GameObject upperPartPrefab; // Le haut du pain (optionnel)
        public float upperYOffset;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<PlateElement> plateElementsList;

    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(SO_KitchenObject kitchenObjectSO) {
        foreach (PlateElement element in plateElementsList) {
            if (element.kitchenObjectSO == kitchenObjectSO) {
                
                // 1. On instancie la partie basse (ou l'ingrédient normal)
                InstantiateVisual(element.lowerPartPrefab, element.lowerYOffset);

                // 2. Si l'objet a une partie haute (comme le pain), on l'instancie aussi
                if (element.upperPartPrefab != null) {
                    InstantiateVisual(element.upperPartPrefab, element.upperYOffset);
                }
                break;
            }
        }
    }

    private void InstantiateVisual(GameObject prefab, float yOffset) {
        GameObject visualInstance = Instantiate(prefab, transform);
        visualInstance.transform.localPosition = new Vector3(0, yOffset, 0);
        visualInstance.transform.localRotation = Quaternion.identity;
    }
}