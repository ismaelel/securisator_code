using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayOnCollision : MonoBehaviour
{
    //public GameObject dialogueBox; 
    public GameObject btn;
    
    void Start()
    {
       // dialogueBox.SetActive(false);
        btn.SetActive(false);
        
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
            
            btn.SetActive(true); 
            
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           // Debug.Log("Le joueur ne touche plus l'image en 2D !");
            btn.SetActive(false); 
         //   dialogueBox.SetActive(false); 
        }
    }

    
    
}