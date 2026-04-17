using UnityEngine;
using UnityEngine.SceneManagement; // Pour changer de scène

public class CharacterSelectionMenu : MonoBehaviour {

    [SerializeField] private GameObject[] characterModels; // Glisse tes modèles 3D ici
    private int currentIndex = 0;

    private void Start() {
        // On s'assure que seul le premier personnage est visible au début
        UpdateCharacterDisplay();
    }

    public void NextCharacter() {
        currentIndex++;
        if (currentIndex >= characterModels.Length) {
            currentIndex = 0; // On revient au premier si on dépasse la fin
        }
        UpdateCharacterDisplay();
    }

    public void PreviousCharacter() {
        currentIndex--;
        if (currentIndex < 0) {
            currentIndex = characterModels.Length - 1; // On va au dernier si on recule trop
        }
        UpdateCharacterDisplay();
    }

    private void UpdateCharacterDisplay() {
        // On désactive tous les modèles
        foreach (GameObject model in characterModels) {
            model.SetActive(false);
        }
        // On active seulement celui qui est sélectionné
        characterModels[currentIndex].SetActive(true);
    }

    public void StartGame() {
        // LA MAGIE EST ICI : On sauvegarde l'index du personnage choisi
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.Save();

        // Remplace "GameScene" par le nom exact de ta scène de jeu
        SceneManager.LoadScene("TutorialScene"); 
    }
}