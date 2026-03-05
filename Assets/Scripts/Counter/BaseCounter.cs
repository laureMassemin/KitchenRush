using UnityEngine;

public class BaseCounter : MonoBehaviour {
    [SerializeField] private Transform spawnPoint;
    private KitchenObject kitchenObject;

    public virtual void Interact() {
        Debug.Log("BaseCounter.Interact()");
    }

    public Transform GetSpawnPoint() => spawnPoint;

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() => kitchenObject;

    public bool HasKitchenObject() => kitchenObject != null;
}