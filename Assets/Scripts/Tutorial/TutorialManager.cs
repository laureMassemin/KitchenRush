using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum TutorialStep {
    Intro,
    Move,
    PickUpPlate,        // 1. Prendre une assiette
    PlaceOnCounter,     // 2. Poser l'assiette sur un comptoir
    SpawnIngredient,    // 3. Prendre un ingrédient dans un container
    Cut,                // 4. Poser sur la planche + couper
    PickUpCut,          // 5. Ramasser l'ingrédient coupé
    PlateIngredient,    // 6. Déposer l'ingrédient sur l'assiette
    PickUpPlateAgain,   // 7. Reprendre l'assiette
    Deliver,            // 8. Livrer
    Done
}

public class TutorialManager : MonoBehaviour {

    public static TutorialManager Instance { get; private set; }

    [Header("Références")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TutorialOverlayUI overlayUI;
    [SerializeField] private DeliveryManager deliveryManager;

    private TutorialStep currentStep = TutorialStep.Intro;
    private bool stepCompleted = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        deliveryManager.OnAnyPlateDelivered += OnAnyDeliverySuccess;
        GoToStep(TutorialStep.Intro);
    }

    private void OnDestroy() {
        deliveryManager.OnAnyPlateDelivered -= OnAnyDeliverySuccess;
    }

    private void Update() {
        if (stepCompleted) return;

        switch (currentStep) {

            case TutorialStep.Move:
                if (playerController.IsWalking())
                    CompleteStep();
                break;

            case TutorialStep.PickUpPlate:
                // Le joueur tient une assiette
                if (playerController.HasKitchenObject() &&
                    playerController.GetKitchenObject() is PlateKitchenObject)
                    CompleteStep();
                break;

            case TutorialStep.PlaceOnCounter:
                // Le joueur n'a plus rien en main (il a posé l'assiette)
                if (!playerController.HasKitchenObject())
                    CompleteStep();
                break;

            case TutorialStep.SpawnIngredient:
                // Le joueur a pris un ingrédient dans un container
                if (playerController.HasKitchenObject() &&
                    !(playerController.GetKitchenObject() is PlateKitchenObject))
                    CompleteStep();
                break;

            case TutorialStep.Cut:
                // Le joueur a posé l'ingrédient sur la planche et appuie F
                if (!playerController.HasKitchenObject() && Input.GetKeyDown(KeyCode.F))
                    CompleteStep();
                break;

            case TutorialStep.PickUpCut:
                // Ramasser l'ingrédient coupé (pas une assiette)
                if (playerController.HasKitchenObject() &&
                    !(playerController.GetKitchenObject() is PlateKitchenObject))
                    CompleteStep();
                break;

            case TutorialStep.PlateIngredient:
                // Le joueur n'a plus rien en main (a déposé sur l'assiette)
                if (!playerController.HasKitchenObject())
                    CompleteStep();
                break;

            case TutorialStep.PickUpPlateAgain:
                // Le joueur reprend l'assiette avec au moins 1 ingrédient
                if (playerController.HasKitchenObject() &&
                    playerController.GetKitchenObject() is PlateKitchenObject plate &&
                    plate.GetKitchenObjectSOList().Count > 0)
                    CompleteStep();
                break;
        }
    }

    private void OnAnyDeliverySuccess() {
        if (currentStep == TutorialStep.Deliver)
            CompleteStep();
    }

    public void CompleteStep() {
        if (stepCompleted) return;
        stepCompleted = true;
        StartCoroutine(NextStepCoroutine());
    }

    private IEnumerator NextStepCoroutine() {
        yield return new WaitForSeconds(0.8f);
        GoToStep(currentStep + 1);
    }

    private void GoToStep(TutorialStep step) {
        currentStep = step;
        stepCompleted = false;

        switch (step) {

            case TutorialStep.Intro:
                overlayUI.Show(
                    "Bienvenue en cuisine !",
                    "Tu vas apprendre les bases avant de te lancer.\nPrêt ?",
                    "C'est parti !",
                    () => GoToStep(TutorialStep.Move)
                );
                break;

            case TutorialStep.Move:
                overlayUI.ShowHint("Déplace-toi avec ZQSD ou les flèches directionnelles.");
                break;

            case TutorialStep.PickUpPlate:
                overlayUI.ShowHint("Va au distributeur d'assiettes et appuie sur [E] pour prendre une assiette.");
                break;

            case TutorialStep.PlaceOnCounter:
                overlayUI.ShowHint("Pose l'assiette sur un comptoir libre avec [E].");
                break;

            case TutorialStep.SpawnIngredient:
                overlayUI.ShowHint("Approche-toi d'un container et appuie sur [E] pour prendre un ingrédient.");
                break;

            case TutorialStep.Cut:
                overlayUI.ShowHint("Pose l'ingrédient sur la planche à découper [E], puis appuie sur [F] pour couper.");
                break;

            case TutorialStep.PickUpCut:
                overlayUI.ShowHint("Bien coupé ! Ramasse l'ingrédient avec [E].");
                break;

            case TutorialStep.PlateIngredient:
                overlayUI.ShowHint("Dépose l'ingrédient sur l'assiette avec [E].");
                break;

            case TutorialStep.PickUpPlateAgain:
                overlayUI.ShowHint("Reprends l'assiette avec [E].");
                break;

            case TutorialStep.Deliver:
                overlayUI.ShowHint("Livre l'assiette au comptoir de livraison avec [E] !");
                break;

            case TutorialStep.Done:
                overlayUI.Show(
                    "Parfait !",
                    "Tu maîtrises les bases. À toi de jouer !",
                    "Lancer la partie",
                    () => SceneManager.LoadScene("GameScene")
                );
                break;
        }
    }
}