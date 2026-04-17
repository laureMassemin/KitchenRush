using UnityEngine;

public class TutorialDeliveryBridge : MonoBehaviour {

    // Ce script écoute la livraison et informe le TutorialManager
    // À mettre sur le même GameObject que DeliveryCounter

    private void Start() {
        // Si DeliveryManager a un event OnRecipeCompleted accessible, abonne-toi ici.
        // Sinon, tu peux appeler TutorialManager.Instance.OnDeliverySuccess()
        // directement depuis DeliveryCounter.DeliverPlate() en ajoutant :
        //   TutorialManager.Instance?.OnDeliverySuccess();
        Debug.Log("TutorialDeliveryBridge prêt.");
    }
}