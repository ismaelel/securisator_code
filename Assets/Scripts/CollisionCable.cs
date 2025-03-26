using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionCable : MonoBehaviour
{
    public GameObject dialogueBox; 
    public GameObject btnBegin;
    
    void Start()
    {
        dialogueBox.SetActive(false);
        btnBegin.SetActive(false);
        
        // Vérifie que l'image a bien un Collider2D
        if (!GetComponent<Collider2D>())
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Le joueur est bloqué par l'image !");
            
            btnBegin.SetActive(true); 
            
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Le joueur ne touche plus l'image en 2D !");
            btnBegin.SetActive(false); 
            dialogueBox.SetActive(false); 
        }
    }

    public void BeginTest()
    {
        if (TriggerInterruptor.cableActivated)
        {
            SceneManager.LoadScene("ElecTest");
        }
        else
        {
            dialogueBox.SetActive(true);
            dialogueBox.GetComponentInChildren<Text>().text += "\n Pénalité de 10 secondes !";
            GameTimer.Instance.AddPenalty(10f);

        }
    }
    
}