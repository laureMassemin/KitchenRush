using UnityEngine;
using UnityEngine.SceneManagement; // Obligatoire pour changer de scène
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private void Awake() {
        // On dit au bouton d'écouter le clic
        playButton.onClick.AddListener(() => {
            // Remplace "GameScene" par le nom exact de ta scène de cuisine
            SceneManager.LoadScene("GameScene"); 
        });
    }
}