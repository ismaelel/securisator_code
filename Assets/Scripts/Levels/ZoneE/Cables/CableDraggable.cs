using UnityEngine;
using System.Collections.Generic;

public class CableDraggable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;  // L'offset entre la souris et le câble
    private Transform endPoint;  // Le point de fin du câble
    private LineRenderer lineRenderer;

    public Transform startPoint; // Le point de départ du câble
    public Transform endConnectPoint; // Le point où le câble peut se connecter (dynamique)
    
    public string cableID; // Identifiant unique pour chaque câble

    // Dictionnaire pour suivre les connexions
    public static Dictionary<string, string> cableConnections = new Dictionary<string, string>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;  // 2 positions : départ et fin

        // Initialisation : on s'assure que le câble n'est pas relié au départ
        if (startPoint != null)
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, startPoint.position + new Vector3(0.5f, 0, 0));  // La fin commence à la position du début
        }
        
        // Désactiver la connexion de la fin du câble au début
        endConnectPoint = null;
    }

    void Update()
    {
        if (isDragging)
        {
            // Calcule la position de la souris avec l'offset
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // On s'assure qu'on reste en 2D

            // Applique l'offset pour que le câble suive la souris sans décalage
            transform.position = mousePos + offset;

            // Mise à jour de la position de l'extrémité du câble (fin)
            lineRenderer.SetPosition(1, transform.position);
        }
        else
        {
            // Si on ne déplace pas le câble, on met à jour la ligne de connexion
            // Si le câble est relié à un point de connexion, on le met à jour
            if (endConnectPoint != null)
            {
                lineRenderer.SetPosition(1, endConnectPoint.position);
                Debug.Log("🔌 Câble relié à : " + endConnectPoint.name);

                // Enregistrer la connexion dans le dictionnaire
                if (!cableConnections.ContainsKey(cableID))
                {
                    cableConnections.Add(cableID, endConnectPoint.name);
                }
            }
        }
    }

    void OnMouseDown()
    {
        // Ajout de log pour vérifier si le câble est cliqué
        Debug.Log("Le câble a été cliqué !");

        // Quand le câble est cliqué, on commence à le déplacer
        isDragging = true;

        // Calcule l'offset entre la position actuelle du câble et la souris
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        offset = transform.position - mousePos;
    }

    void OnMouseUp()
    {
        // Quand la souris est relâchée, le câble s'arrête de se déplacer
        Debug.Log("Le câble a été relâché.");

        isDragging = false;

        // Si le câble est proche du point de connexion, il se fixe
        if (endConnectPoint != null && Vector3.Distance(transform.position, endConnectPoint.position) < 1f)
        {
            transform.position = endConnectPoint.position;
            Debug.Log("🔌 Câble relié à " + endConnectPoint.name);
            // Enregistrer la connexion dans le dictionnaire
            if (cableConnections.ContainsKey(cableID))
            {
                cableConnections.Remove(cableID);
                Debug.Log("retrait dans le dico " + cableID);
            }
            cableConnections.Add(cableID, endConnectPoint.name);
            Debug.Log("ajout dans le dico " + cableID + " " + endConnectPoint.name);
        }
        else
        {
            // Sinon, le câble retourne à son point de départ
            transform.position = startPoint.position;
            Debug.Log("⚠️ Câble retourné au point de départ");
            if (cableConnections.ContainsKey(cableID))
            {
                cableConnections.Remove(cableID);
                Debug.Log("retrait dans le dico " + cableID);
            }
        }

        // Mise à jour de la position de fin du câble
        lineRenderer.SetPosition(1, transform.position);

        // Désactive la connexion après le relâchement
        endConnectPoint = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Objet touché : " + other.name + " avec le tag " + other.tag);

        if (other.CompareTag("CableEnd"))
        {
            endConnectPoint = other.transform;
            Debug.Log("🔌 Câble connecté au point de connexion : " + other.name);
        }
        else
        {
            Debug.Log("Câble touche un autre objet : " + other.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Si le câble sort du point de connexion, il ne peut plus se connecter
        if (other.CompareTag("CableEnd"))
        {
            endConnectPoint = null;
            Debug.Log("⚠️ Câble éloigné du point de connexion.");
        }
    }
}
