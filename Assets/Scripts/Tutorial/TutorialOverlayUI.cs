using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialOverlayUI : MonoBehaviour {

    [Header("Panneau principal (popup)")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI confirmButtonText;

    [Header("Bandeau d'indication")]
    [SerializeField] private GameObject hintRoot;
    [SerializeField] private TextMeshProUGUI hintText;

    private void Awake() {
        HideAll();
    }

    /// <summary>
    /// Affiche un panneau de dialogue avec un bouton d'action.
    /// </summary>
    public void Show(string title, string body, string buttonLabel, Action onConfirm) {
        HideAll();
        panelRoot.SetActive(true);

        titleText.text = title;
        bodyText.text = body;
        confirmButtonText.text = buttonLabel;

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => {
            HideAll();
            onConfirm?.Invoke();
        });
    }

    /// <summary>
    /// Affiche juste un bandeau de texte en bas/haut de l'écran.
    /// </summary>
    public void ShowHint(string hint) {
        HideAll();
        hintRoot.SetActive(true);
        hintText.text = hint;
    }

    public void HideAll() {
        panelRoot.SetActive(false);
        hintRoot.SetActive(false);
    }
}