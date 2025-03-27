using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    public GameObject infoPanel;  // Référence au panneau avec le texte et le fond
    public Button closeButton;    // Référence au bouton de fermeture

    void Start()
    {
        // Au lancement, afficher le panneau
        infoPanel.SetActive(true);

        // Ajoute un écouteur pour fermer le panneau
        closeButton.onClick.AddListener(CloseInfoPanel);
    }

    // Fonction pour fermer le panneau
    void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}