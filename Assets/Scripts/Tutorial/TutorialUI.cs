using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour {

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Button helpButton; // Le bouton "?" pendant la partie

    private void Start() {
        Time.timeScale = 0f;
        tutorialPanel.SetActive(true);

        helpButton.onClick.AddListener(ShowTutorial);
        helpButton.gameObject.SetActive(true);
    }

    private void ShowTutorial() {
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f; // Pause le jeu
        helpButton.gameObject.SetActive(false); // Cache le "?" pendant qu'on lit
    }

    private void HideTutorial() {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f; // Relance le jeu
        helpButton.gameObject.SetActive(true); // Réaffiche le "?"
    }

    private void Update() {
        if (tutorialPanel.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            HideTutorial();
        }
    }
}