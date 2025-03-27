using PixelCrushers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Fire : MonoBehaviour
{
    
    public static Fire Instance;  // Déclaration du Singleton

    public Text timer;
    public GameObject dialogueBox; 
    public Text panelText; // Référence au texte du panel
    public Image panelImage; // Référence à l'image du panel
    public GameObject btnBegin;
    void Start()
    {
        GameTimer.Instance.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
        timer.text = GameTimer.Instance.getTime();
        
    }

    // void Awake()
    // {
    //     Debug.Log("➡️ GameTimer Awake - " + SceneManager.GetActiveScene().name);
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //         Debug.Log("✅ GameTimer conservé !");
    //     }
    //     else
    //     {
    //         Debug.Log("❌ GameTimer supprimé (déjà existant)");
    //         //Destroy(gameObject);
    //     }
    // }
    public void BeginTest()
    {
        Debug.Log("Begin Test");
        Debug.Log("goodBtn" + ButtonDetector.goodBtn);
        if (ButtonDetector.goodBtn)
        {
            Debug.Log("Début de l'épreuve");
            SceneManager.LoadScene("FireTest");
        }
        else
        {
            dialogueBox.SetActive(true);
            panelText.text = "Impossible de commencer l'épreuve" +
                             "Pénalité de 10 sec";
            GameTimer.Instance.AddPenalty(10f);

            Debug.Log("Impossible de commencer l'épreuve");
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            btnBegin.SetActive(true); 
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            btnBegin.SetActive(false); 
            dialogueBox.SetActive(false); 
        }
    }
}
