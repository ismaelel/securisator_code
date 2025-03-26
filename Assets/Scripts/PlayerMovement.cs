using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FloatingJoystick joystick; // Assigne le joystick depuis l'Inspector
    public float speed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Récupération du Rigidbody2D
    }

    void FixedUpdate()
    {
        // Récupération des valeurs du joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Application du mouvement
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.linearVelocity = movement * speed;
    }
}