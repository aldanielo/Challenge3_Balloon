using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private float topBound = 18;
    public float bounceForce = 10f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5 * Time.deltaTime, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        if (transform.position.y >= topBound)
        {
            transform.position = new Vector3(transform.position.x, topBound, transform.position.z);
            playerRb.AddForce(-Vector3.up, ForceMode.VelocityChange);
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
        // Check if the balloon collided with the ground
        if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            // Reset the velocity to prevent cumulative forces (so the balloon doesn't accelerate unnaturally)
            //playerRb.velocity = Vector3.zero;

            // Add an upward force to simulate a bounce
            playerRb.AddForce(Vector3.up * bounceForce * Time.deltaTime, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 1.0f);

        }

    }

}
