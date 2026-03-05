using UnityEngine;

public class DeliveryCounter : BaseCounter {

    // On utilise un Singleton simple pour trouver le DeliveryManager
    [SerializeField] private DeliveryManager deliveryManager;

    public override void Interact() {
        // On ne peut interagir que si on a une assiette en main
        // (Le code du PlayerController doit gérer le passage d'objet ici)
    }

    // Cette méthode sera appelée par ton PlayerController
    public void DeliverPlate(PlateKitchenObject plateKitchenObject) {
        deliveryManager.DeliverRecipe(plateKitchenObject);
        // On détruit l'assiette après livraison
        Destroy(plateKitchenObject.gameObject);
    }
}