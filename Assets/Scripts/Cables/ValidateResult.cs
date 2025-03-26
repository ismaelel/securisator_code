using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ValidateResult : MonoBehaviour
{
    public static bool isValid = false;
    public GameObject dialogueBox; 
    public Image panelImage;
    public Button XBtn;
    public GameObject Outro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClosePanel();
        Outro.SetActive(false);
        if (XBtn != null)
            XBtn.onClick.AddListener(ClosePanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestResult()
    {
        isValid = true;
        

        //SceneManager.LoadScene("Elec");
        // Utiliser l'instance de LevelManager pour appeler la méthode non statique
        
    }
    public LevelManager levelManager;  // Référence à LevelManager

    public void ValidateCableConnections()
    {
        Debug.Log("Validation des câbles appelée");

        bool goodCabling = true;
        Debug.Log("LISTE DES CABLES");
        foreach (var connection in CableDraggable.cableConnections)
        {
            Debug.Log($"Le câble {connection.Key} est connecté au point {connection.Value}");
        }
        if (CableDraggable.cableConnections.Count == 4)
        {

            foreach (var entry in CableDraggable.cableConnections)
            {
                string key = entry.Key;
                string value = entry.Value;


                // Vérification des connexions correctes
                if (key == "G" && value != "GEnd")
                {
                    Debug.LogError("❌ Connexion incorrecte pour G. Attendu GEnd, mais trouvé " + value);
                    goodCabling = false;
                }
                else if (key == "R" && value != "REnd")
                {
                    Debug.LogError("❌ Connexion incorrecte pour R. Attendu REnd, mais trouvé " + value);
                    goodCabling = false;
                }
                else if (key == "B" && value != "BEnd")
                {
                    Debug.LogError("❌ Connexion incorrecte pour B. Attendu BEnd, mais trouvé " + value);
                    goodCabling = false;
                }
                else if (key == "Y" && value != "YEnd")
                {
                    Debug.LogError("❌ Connexion incorrecte pour Y. Attendu YEnd, mais trouvé " + value);
                    goodCabling = false;
                }
                else
                {
                    Debug.Log("✅ Connexion correcte : " + key + " -> " + value);

                }

                if (!goodCabling)
                {
                    dialogueBox.SetActive(true);
                    dialogueBox.GetComponentInChildren<Text>().text = "Erreur dans le cablage !";
                }



            }
        }
        else
        {
            Debug.Log("Connectez tous les cables");
            goodCabling = false;
            dialogueBox.SetActive(true);
            dialogueBox.GetComponentInChildren<Text>().text = "Connectez tous les cables";
           
        }
        if (goodCabling)
        {
           // levelManager.IncreaseCurrentUserLevel();
            //SceneManager.LoadScene("MenuPrincipal");
            Outro.SetActive(true);
        }

        
    }
    
    public void ClosePanel()
    {
        dialogueBox.SetActive(false); // Masquer le panneau
    }
}
