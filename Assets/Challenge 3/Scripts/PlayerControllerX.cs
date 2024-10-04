using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    //public float floatForce = 5.0f;
    public float floatForce;
    public float maxUpwardVelocity = 10.0f;
   // public float horizontalSpeed = 5.0f;
    private float gravityModifier = 1.5f;
    private float upperBound = 14.0f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        
        Physics.gravity *= gravityModifier;
        playerRb.drag = 1.0f;

        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        //playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

         // Initially hide the Game Over text
        gameOverText.gameObject.SetActive(false);

    }

    // Update is called once per frame
   void Update()
    {
        // Balloon float up control with spacebar and add a height limit
        if (Input.GetKey(KeyCode.UpArrow) && !gameOver && transform.position.y < upperBound)
        {
            // Limit the upward velocity
            if (playerRb.velocity.y < maxUpwardVelocity)
            {
                playerRb.AddForce(Vector3.up * floatForce);
            }
        }

        // Prevent balloon from exceeding the upper bound
        if (transform.position.y >= upperBound)
        {
            // Set position to upperBound
            transform.position = new Vector3(transform.position.x, upperBound, transform.position.z);
        }

        // Prevent balloon from going below the ground level
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.transform.position = transform.position;
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;

            // Display the Game Over text
            gameOverText.gameObject.SetActive(true);

            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            // Set the fireworks particle position to the player's position before playing it
            fireworksParticle.transform.position = transform.position;
            fireworksParticle.Play();

            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
    }
}