using UnityEngine;
using UnityEngine.UI;

public class TriggerInterruptor : MonoBehaviour
{
    public GameObject btn;
    public GameObject image1;
    public GameObject image2;
    public static bool cableActivated = false; // Variable partagée

    private bool playerIsNear = false;

    void Start()
    {
        btn.SetActive(false);
        if (!GetComponent<Collider2D>())
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        image1.gameObject.SetActive(true);
        image2.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            btn.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            btn.SetActive(false);
        }
    }

    public void SwitchImages()
    {
        bool isImage1Active = image1.gameObject.activeSelf;

        image1.gameObject.SetActive(!isImage1Active);
        image2.gameObject.SetActive(isImage1Active);

        if (isImage1Active)
        {
            cableActivated = true;
            btn.GetComponentInChildren<Text>().text = "Rebrancher l'électricité";
        }
        else
        {
            cableActivated = false;
            btn.GetComponentInChildren<Text>().text = "Débrancher l'électricité";

        }
        
        Debug.Log("Bouton câbles cliqué. Images switchées.");
    }
}