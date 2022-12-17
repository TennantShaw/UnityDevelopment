using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables
    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool doubleJumpSkill = true;
    public bool gameOver = false;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
       playerRb = GetComponent<Rigidbody>();
       playerAnim = GetComponent<Animator>();
       Physics.gravity *= gravityModifier;
       playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if player has pressed the space bar and player is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver) {
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false; 
            playerAnim.SetTrigger("Jump_trig");
        }

        // check to see if the player has pressed the b button, is not on the ground, and the doublejump skill is true
        if (Input.GetKeyDown(KeyCode.B) && !isOnGround && doubleJumpSkill) {
            DoubleJump();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
            dirtParticle.Play();
            doubleJumpSkill = true;
        } else if (collision.gameObject.CompareTag("Obstacle")) {
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
        }
        
    }

    private void DoubleJump() {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        doubleJumpSkill = false;
    }

}
