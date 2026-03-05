using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private SO_KitchenObject kitchenObjectSO;

    public SO_KitchenObject GetKitchenObjectSO() => kitchenObjectSO;

    public void SetKitchenObjectParent(Transform targetParent) {
        transform.parent = targetParent;
        transform.localPosition = Vector3.zero; // L'objet se colle au HoldPoint ou au SpawnPoint
        transform.localRotation = Quaternion.identity;
    }
}