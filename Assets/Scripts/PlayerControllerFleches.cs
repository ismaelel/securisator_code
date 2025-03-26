using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControllerFleches : MonoBehaviour
{
    public float moveDistance = 0.2f;  // Distance à parcourir à chaque pression de bouton (plus petit pas)
    public float moveSpeed = 5f;       // Vitesse de déplacement pour gérer la fluidité
    private bool isMovingUp, isMovingDown, isMovingLeft, isMovingRight;  // Variables pour savoir si un bouton est pressé

    // Fonction pour commencer le mouvement (onPointerDown)
    public void StartMoveUp() => isMovingUp = true;
    public void StartMoveDown() => isMovingDown = true;
    public void StartMoveLeft() => isMovingLeft = true;
    public void StartMoveRight() => isMovingRight = true;

    // Fonction pour arrêter le mouvement (onPointerUp)
    public void StopMoveUp() => isMovingUp = false;
    public void StopMoveDown() => isMovingDown = false;
    public void StopMoveLeft() => isMovingLeft = false;
    public void StopMoveRight() => isMovingRight = false;

    void Update()
    {
        // Déplacement continu tant que le bouton est pressé
        if (isMovingUp)
        {
            MovePlayer(Vector3.up);
        }
        if (isMovingDown)
        {
            MovePlayer(Vector3.down);
        }
        if (isMovingLeft)
        {
            MovePlayer(Vector3.left);
        }
        if (isMovingRight)
        {
            MovePlayer(Vector3.right);
        }
    }    
    
    // Fonction pour avancer d'un petit "pas"
    void MovePlayer(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime); // Déplacement fluide
    }

    // Utiliser un EventTrigger dans l'Inspector pour relier les méthodes de Start et Stop
}