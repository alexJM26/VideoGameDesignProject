using UnityEngine;

// movement of kitten
public class KittenMovement : MonoBehaviour
{
    // variables
    private float speed = 10; // set sprite speed to 5
    private Rigidbody2D kitten; // kitten's rigid body
    private Animator animate;


    // runs once, when script is loaded
    private void Awake() 
    {
        kitten = GetComponent<Rigidbody2D>(); // get the rigid body of kitten
        animate = GetComponent <Animator>(); // get animator of kitten
    }


    // runs every frame
    private void Update()
    {
        // walking left/right, keep vertical velocity the same
        // velocity is a Vector2 with (x velocity, y velocity)
        kitten.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, kitten.velocity.y);

        // jumping 
        if (Input.GetKey(KeyCode.Space)) 
            kitten.velocity = new Vector2(kitten.velocity.x, speed);

        // cause kitten to flip when running in different directions
        if (Input.GetAxis("Horizontal") > 0.01f)
            transform.localScale = Vector3.one;
        else if (Input.GetAxis("Horizontal") < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // update walking status
        animate.SetBool("Walk", Input.GetAxis("Horizontal") != 0);

        animate.SetBool("Still", (Input.GetAxis("Horizontal") == 0) && (kitten.velocity.y > 0.01));

        animate.SetBool("Attack", Input.GetKey(KeyCode.E));
    }
}