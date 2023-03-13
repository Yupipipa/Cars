using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float horsePower = 10000f;
    [SerializeField] float turnSpeed = 50f;
    private float speed;
    private float zBound = 8f;
    private float maxRotateAngle = 50f;
    private float angle;

    private Rigidbody playerRb;
    private AudioSource playerAudio;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] GameManager gameManager;
    public ParticleSystem explosionParticle;
    [SerializeField] AudioClip pickUpSound;
    [SerializeField] ParticleSystem coinParticle;
    [SerializeField] int wheelOnGround;
    [SerializeField] List<WheelCollider> allWheels;
    [SerializeField] TextMeshProUGUI guidText;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        playerRb.centerOfMass = centerOfMass.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        CheckZBounds();
    }

    private void CheckZBounds()
    {
        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
        if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
    }

    private void MovePlayer()
    {
        if (gameManager.isGameActive)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            speed = Mathf.Round(playerRb.velocity.magnitude * 3.6f);
            if (IsOnGround())
            {
                playerRb.AddRelativeForce(Vector3.forward * horsePower * verticalInput);
                if (speed > 0)
                {
                    guidText.gameObject.SetActive(false);
                    transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
                }

            }
            WheelTurnRotation(horizontalInput);
        }
    }

    private void WheelTurnRotation(float horizontalInput)
    {
        angle += turnSpeed * horizontalInput;
        angle = Mathf.Clamp(angle, -maxRotateAngle, maxRotateAngle);
        if (horizontalInput == 0 && angle != 0)
        {
            angle = 0;
        }
        allWheels[0].transform.localRotation = Quaternion.Euler(0, angle, 0);
        allWheels[1].transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    bool IsOnGround()
    {
        wheelOnGround = 0;
        foreach (var wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelOnGround++;
            }
        }

        if (wheelOnGround > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Instantiate(coinParticle, transform.position, explosionParticle.transform.rotation);
            playerAudio.PlayOneShot(pickUpSound, 1.0f);
            gameManager.score += 100;
            Destroy(other.gameObject);
        }
    }
}
