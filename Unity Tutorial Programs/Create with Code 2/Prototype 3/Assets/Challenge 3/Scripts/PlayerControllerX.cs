using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;
    private Vector3 ceilingHeight;
    private Vector3 resetPositionTop;
    private Vector3 resetPositionBot;
    private Vector3 floorHeight;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

        // Assign value to vector for ceiling height
        // ** would need to make this adhere to changing screen sizes for an actual release
        ceilingHeight = new Vector3(-3, 16, 0);

        // Assign value to vector for reset position for the bottom of the screen
        // ** would need to make this adhere to changing screen sizes for an actual release
        floorHeight = new Vector3(-3, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        // Check to see if the player is to high. If so, reset position y
        if (transform.position.y > ceilingHeight.y) {
            ResetPositionTop();
            playerRb.AddForce(Vector3.down * 1, ForceMode.Impulse);
        }

        // Check to see if player is to low. Stop game if so
        if (transform.position.y < floorHeight.y) {
            ResetPositionBot();
            playerRb.AddForce(Vector3.up * 1, ForceMode.Impulse);
            gameOver = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

    private void ResetPositionTop() {
        transform.position = ceilingHeight;
    }

    private void ResetPositionBot() {
        transform.position = floorHeight;
    }


}
