using UnityEngine;

public class PlayerStyleManager : MonoBehaviour {

    [SerializeField] private GameObject[] characterStyles; // Glisse les modèles enfants ici

    private void Start() {
        // On récupère le choix sauvegardé dans le menu (par défaut 0 si rien n'est trouvé)
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        // On désactive tous les styles
        foreach (GameObject style in characterStyles) {
            style.SetActive(false);
        }

        // On vérifie que l'index est valide pour éviter les bugs, puis on l'active
        if (selectedIndex >= 0 && selectedIndex < characterStyles.Length) {
            characterStyles[selectedIndex].SetActive(true);
        } else {
            // Sécurité : on active le premier si l'index est bizarre
            characterStyles[0].SetActive(true); 
        }
    }
}