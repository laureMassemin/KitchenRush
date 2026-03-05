using UnityEngine;
using System; 

public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed = 7f;
    [SerializeField] private LayerMask collisionsLayer;
    [SerializeField] private LayerMask countersLayer; 
    private BaseCounter selectedCounter;
    private Vector3 lastInteractDir;
    public event Action<BaseCounter> OnSelectedCounterChanged;
    [SerializeField] private Transform holdPoint; // Glissez l'objet HoldPoint ici dans l'UI
    private KitchenObject kitchenObject; // L'objet actuellement porté  
    
    private bool isWalking;

    void Update() {
        HandleMovement();
        HandleInteractions();
        if (Input.GetKeyDown(KeyCode.E)) {
            if (selectedCounter != null) {
                if (this.HasKitchenObject()) {
                    // LE JOUEUR PORTE QUELQUE CHOSE
                    if (selectedCounter is TrashCounter) {
                        Destroy(this.GetKitchenObject().gameObject);
                        this.SetKitchenObject(null);
                    } else if (selectedCounter is DeliveryCounter deliveryCounter) {
                        if (this.GetKitchenObject() is PlateKitchenObject plateKitchenObject) {
                            // Le joueur tient une assiette et regarde le comptoir de livraison
                            deliveryCounter.DeliverPlate(plateKitchenObject);
                            this.SetKitchenObject(null);
                        }
                    }
                    else if (selectedCounter.HasKitchenObject()) {
                        // Le comptoir a déjà un objet (peut-être une assiette ?)
                        if (selectedCounter.GetKitchenObject() is PlateKitchenObject plateKitchenObject) {
                            // Si on regarde une assiette, on essaie d'ajouter ce qu'on a en main dessus
                            if (plateKitchenObject.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSO())) {
                                Destroy(this.GetKitchenObject().gameObject);
                                this.SetKitchenObject(null);
                            }
                        }
                    }
                    else {
                        // Le comptoir est vide : on pose l'objet
                        KitchenObject itemToDrop = this.GetKitchenObject(); 
                        itemToDrop.SetKitchenObjectParent(selectedCounter.GetSpawnPoint());
                        selectedCounter.SetKitchenObject(itemToDrop);
                        this.SetKitchenObject(null);

                        if (selectedCounter is StoveCounter stoveCounter) {
                            stoveCounter.StartFrying(itemToDrop.GetKitchenObjectSO());
                        }
                    }
                } else {
                    // LE JOUEUR N'A RIEN DANS LES MAINS
                    if (selectedCounter.HasKitchenObject()) {
                        // 1. On récupère l'objet qui est sur le comptoir
                        KitchenObject objectToPickup = selectedCounter.GetKitchenObject();

                        // 2. On change son parent pour le mettre dans les mains du joueur (HoldPoint)
                        objectToPickup.SetKitchenObjectParent(this.GetHoldPoint());

                        // 3. On informe le joueur qu'il porte cet objet
                        this.SetKitchenObject(objectToPickup);

                        // 4. On libère le comptoir
                        selectedCounter.SetKitchenObject(null);
                        
                        Debug.Log("Objet ramassé : " + objectToPickup.name);
                    } else {
                        // Le comptoir est vide : on tente l'action de base du meuble (ex: distributeur d'assiettes)
                        selectedCounter.Interact();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            if (selectedCounter is CuttingCounter cuttingCounter) {
                cuttingCounter.Cut();
            }
        }
    }

    private void HandleMovement() {
        // Lecture des touches ZQSD ou Flèches
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = speed * Time.deltaTime;
        float playerRadius = .7f;
        
        // Détection simple des collisions (inspiré de ton fichier original)
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * 2f, playerRadius, moveDir, moveDistance, collisionsLayer);

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        // Rotation fluide du personnage
        float rotateSpeed = 10f;
        if (moveDir != Vector3.zero) {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }
    private void HandleInteractions() {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }


        float interactDistance = 2f;
    
        // Positionne le rayon à 0.5 unité du sol (pile au milieu du comptoir)
        Vector3 rayOrigin = transform.position + Vector3.up * -0.5f;

        // Visualisation pour t'aider à régler
        Debug.DrawRay(rayOrigin, lastInteractDir * interactDistance, Color.red);

        if (Physics.Raycast(rayOrigin, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayer)) {
            // Si vous arrivez ici, c'est que le rayon a touché un objet sur le layer "Counters"
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter)) {
                if (counter != selectedCounter) {
                    selectedCounter = counter;
                    Debug.Log("Comptoir détecté : " + raycastHit.transform.name); // Plus précis pour le débug
                }
            }
        } else {
            selectedCounter = null;
        }
    }

    public bool IsWalking() => isWalking;

    // Ajoutez ces méthodes pour gérer l'objet
    public Transform GetHoldPoint() => holdPoint;

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() => kitchenObject;

    public bool HasKitchenObject() => kitchenObject != null;
}