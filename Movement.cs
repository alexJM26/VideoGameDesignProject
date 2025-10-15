using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    // declarations
    private Rigidbody2D kittenRB;
    private Vector2 moveInput;
    private Animator animate;
    SpriteRenderer spriteRenderer; // for flipping sprite

    // movement variables
    public float moveSpeed = 5f; // is public, so can be changed in Unity
    public float jumpForce = 10f;

    // status variables
    private bool onGround = true;
    private int keyCount = 0;
    private int currentScene = 1;

    void Start()
    {
        kittenRB = GetComponent<Rigidbody2D>(); // grab rigid body of kitten
        spriteRenderer = GetComponent<SpriteRenderer>(); // get sprite renderer of kitten
        animate = GetComponent<Animator>(); // get animator of kitten
    }


    void FixedUpdate()
    {
        kittenRB.velocity = new Vector2(moveInput.x * moveSpeed, kittenRB.velocity.y); // update kitten's x velocity based on input and speed

        animate.SetBool("Walk", moveInput.x != 0); // update walking status
        animate.SetBool("Jump", !onGround);

        // flip sprite based on movement
        if (moveInput.x < 0) spriteRenderer.flipX = true;
        if (moveInput.x > 0) spriteRenderer.flipX = false;
    }

    // handles walking
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // move based on input
    }

    // handles the jumping function
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && onGround) // performed = just once as the button press is completed
        {
            kittenRB.AddForce(new Vector2(kittenRB.velocity.x, jumpForce), ForceMode2D.Impulse); // impulse mode to make the jump a single "burst"
            onGround = false;
        }
    }

    // method that is called automatically by unity when kitten's 2D collider makes contact with another
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // checks for collision with ground
        {
            onGround = true;
        }
    }

    // handles collisions with door/key
    public void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "Key")
        {
            other.gameObject.SetActive(false); // removes key when cat enters the trigger/collision radius
            keyCount++; // update key count

            // get array of all breakable walls and then inactivate all of them
            GameObject[] breakableWalls = GameObject.FindGameObjectsWithTag("BreakableWall");
            for (int i = 0; i < breakableWalls.Length; i++)
            {
                breakableWalls[i].SetActive(false);
            }
        }

        if (other.tag == "Door")
        {
            if (currentScene >= 1) // loop through room two infinitely for now
            {
                SceneManager.LoadScene("RoomTwo");
                currentScene++;
            }
            else
            {
                SceneManager.LoadScene("RoomOne");
                currentScene--;
            }
        }
    }
}
