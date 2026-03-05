using UnityEngine;

public class ContainerCounter : BaseCounter {
    [SerializeField] private SO_KitchenObject kitchenObjectSO;

    public override void Interact() {
        if (!HasKitchenObject()) {
            // Crée l'objet à partir du Prefab stocké dans le Scriptable Object
            GameObject kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            KitchenObject ko = kitchenObjectTransform.GetComponent<KitchenObject>();
            
            // Donne l'objet au comptoir
            ko.SetKitchenObjectParent(GetSpawnPoint());
            SetKitchenObject(ko);
        }
    }
}