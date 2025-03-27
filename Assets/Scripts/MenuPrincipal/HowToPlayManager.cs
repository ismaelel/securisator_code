using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayManager : MonoBehaviour
{
    public Image tutorialImage;  // L'image affichée
    public Text tutorialText;    // Le texte affiché
    public Button continueButton; // Bouton "Continuer"

    public Sprite[] images;  // Tableau des images à afficher
    public string[] texts;   // Tableau des textes à afficher

    private int currentStep = 0; // Étape actuelle

    void Start()
    {
        // Vérifier qu'il y a bien du contenu
        if (images.Length > 0 && texts.Length > 0)
        {
            UpdateTutorial(); // Affiche la première étape
        }
        else
        {
            Debug.LogError("🚨 Les images ou les textes ne sont pas assignés !");
        }

        // Associer la fonction au bouton "Continuer"
        continueButton.onClick.AddListener(NextStep);
    }

    void NextStep()
    {
        currentStep++; // Passer à l'étape suivante

        if (currentStep < images.Length)
        {
            UpdateTutorial(); // Met à jour l'UI
            if (currentStep == images.Length - 1)
            {
                continueButton.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("✅ Tutoriel terminé !");
            gameObject.SetActive(false); // Cache le tutoriel
        }
        
        
    }

    void UpdateTutorial()
    {
        tutorialImage.sprite = images[currentStep]; // Change l'image
        tutorialText.text = texts[currentStep]; // Change le texte
    }
    
    
}