using UnityEngine;
using UnityEngine.UI;

public class ButtonDetector : MonoBehaviour
{
    public static bool goodBtn = false; // Variable partagée
    public GameObject clickButton; // Référence au bouton "Cliquer"
    private string buttonColor; // Stocke la couleur du bouton actuel
    public GameObject dialogueBox; 
    public Text panelText; // Référence au texte du panel
    public Image panelImage; // Référence à l'image du panel
    void Start()
    {
        clickButton.SetActive(false); // Cache le bouton au démarrage
        buttonColor = gameObject.name; // Utilise le nom du GameObject comme couleur
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Vérifie si c'est le joueur qui entre dans la zone
        {
            clickButton.SetActive(true);
            clickButton.GetComponent<Button>().onClick.RemoveAllListeners(); // Nettoie les anciens events
            clickButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked());
        }
    }
    
   

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Cache le bouton quand le joueur s'éloigne
        {
            clickButton.SetActive(false);
            dialogueBox.SetActive(false);
        }
    }

    void OnButtonClicked()
    {
        Debug.Log("Bouton " + buttonColor + " pressé !");

        switch (buttonColor)
        {
            case "dn_0" :
                dialogueBox.SetActive(true);
                panelImage.color = Color.red;
                goodBtn = false;
                GameTimer.Instance.AddPenalty(10f);
                panelText.text = "Ceci est un bouton de Plan Particulier de Mise en Sureté (PPMS). " +
                                 "Ce n'est pas le bon bouton à utiliser en cas d'incendie !" +
                                 "Pénalité de 10 secondes !";
                break;
            case "dv_0" :
                dialogueBox.SetActive(true);
                panelImage.color = Color.red;
                goodBtn = false;
                GameTimer.Instance.AddPenalty(10f);
                panelText.text = "Ceci est un bouton d'ouverture de porte de secours. " +
                                 "Ce n'est pas le bon bouton à utiliser en cas d'incendie !"+
                                 "Pénalité de 10 secondes !";
                break;
            case "dr_0" :
                dialogueBox.SetActive(true);
                panelImage.color = Color.green;
                panelText.text = "Ceci est un bouton d'alarme incendie. " +
                                 "Vous avez (enfin) eu le bon réflexe !";
                goodBtn = true;
                break;
        }
      
        
    }
    
    
    
}