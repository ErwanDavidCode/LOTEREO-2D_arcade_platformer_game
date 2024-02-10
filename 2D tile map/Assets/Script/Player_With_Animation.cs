using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_With_Animation : MonoBehaviour
{
    public float movementSpeed2 = 10f;
    public float jumpForce2 = 20f;
    private bool isGrounded = false;
    private bool canJump = true;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private string WALK_ANIMATION = "Walk";
    public ProceduralGeneration ProceduralGeneration;
    private GameObject gunObject;
    private Gun2D gunScript;
    private GameObject swordObject;
    private Sword swordScript;
    private GameObject pickaxeObject;
    private Pickaxe pickaxeScript;
    bool isFacingRight = true; // Determine the player's facing direction
    public LayerMask groundLayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(2f, transform.position.y, transform.position.z);


        //ATTENTION : Pour que les rotations d'objets marchent, il faut lancer le jeu avec les 3 obj actifs

        //référence au script du GUN pour le flip 
        gunObject = GameObject.FindGameObjectWithTag("Gun");
        if (gunObject != null)
        {
            gunScript = gunObject.GetComponent<Gun2D>();
            gunObject.transform.localPosition = new Vector3(1.4f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);
        }

        simulateRotation();

        //SWORD
        swordObject = GameObject.FindGameObjectWithTag("Sword");
        if (swordObject != null)
        {
            swordScript = swordObject.GetComponent<Sword>();
            swordObject.transform.localPosition = new Vector3(1.4f, swordObject.transform.localPosition.y, swordObject.transform.localPosition.z);
        }

        //PICKAXE
        pickaxeObject = GameObject.FindGameObjectWithTag("Pickaxe");
        if (pickaxeObject != null)
        {
            pickaxeScript = pickaxeObject.GetComponent<Pickaxe>();
            pickaxeObject.transform.localPosition = new Vector3(1.4f, pickaxeObject.transform.localPosition.y, pickaxeObject.transform.localPosition.z);
        }
    }

    void simulateRotation()
    {
        //GUN
        if (gunScript != null && gunObject != null)
        {
            //on initialise le spawnpoint des balles
            gunScript.bulletSpawnPoint.localPosition = new Vector3(3.5f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);

            //SIMULER DROITE GAUCHE JOUEUR POUR CONDITIONS INITIALES
            anim.SetBool(WALK_ANIMATION, true);
            sr.flipX = true;
            gunScript.sr.flipX = false; //face à gauche
            gunScript.bullet_right = true; //tire à gauche
            isFacingRight = false; // Update facing direction
            gunScript.bulletSpawnPoint.localPosition = new Vector3(3.5f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);
            // Place the gun on the left side of the player
            gunObject.transform.localPosition = new Vector3(-1.5f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);
            // Player is moving right and facing left, so flip the gun and sprite
            anim.SetBool(WALK_ANIMATION, true);
            sr.flipX = false;
            gunScript.sr.flipX = true; //face à droite
            gunScript.bullet_right = false; //tire à droite
            isFacingRight = true; // Update facing direction
            gunScript.bulletSpawnPoint.localPosition = new Vector3(-3.5f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);
            // Place the gun on the right side of the player
            gunObject.transform.localPosition = new Vector3(1.5f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);
        }
    }

    void Update()
    {

        // //A SUPPRIMER : FILM SOUTENANCE
        // // Déplacement du joueur le long de l'axe +X
        // float deplacement = -10f * Time.deltaTime;
        // transform.Translate(Vector3.right * deplacement);
        // transform.position = new Vector3(transform.position.x, 68f, transform.position.z);
        // if (Input.GetButtonDown("Jump"))
        // {
        //     transform.position = new Vector3(2f, 68f, transform.position.z);
        // }
        // //A SUPPRIMER : SOUTENANCE

        // Jumping
        if (isGrounded && canJump && Input.GetButtonDown("Jump"))
        {
            // Start the coroutine to prevent additional jumps for a certain duration
            StartCoroutine(JumpCooldown());
            Jump();
        }
    }

    void FixedUpdate()
    {
        //Empêcher le joueur d'aller au-delà des limites
        if (transform.position.x <= 1)
        {
            transform.position = new Vector3(1f, transform.position.y, transform.position.z);


        }
        if (transform.position.x >= (ProceduralGeneration.width - 1))
        {
            transform.position = new Vector3(ProceduralGeneration.width - 1, transform.position.y, transform.position.z);
        }

        // Movement
        float horizontalMovement = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(-horizontalMovement, 0f) * movementSpeed2 * Time.deltaTime;
        transform.Translate(movement);

        AnimatePlayer(horizontalMovement);
    }

    void AnimatePlayer(float horizontalMovement)
    {
        if (horizontalMovement < 0 && isFacingRight)
        {
            // Player is moving left and facing right, so flip the gun and sprite
            anim.SetBool(WALK_ANIMATION, true);
            sr.flipX = true;
            isFacingRight = false; // Update facing direction

            //GUN
            if (gunScript != null && gunObject != null)
            {
                gunScript.sr.flipX = false; //face à gauche
                gunScript.bullet_right = true; //tire à gauche
                gunScript.bulletSpawnPoint.localPosition = new Vector3(2f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);

                // Place the gun on the left side of the player
                gunObject.transform.localPosition = new Vector3(-1.4f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);
            }

            //SWORD
            if (swordScript != null && swordObject != null)
            {
                swordScript.sr.flipX = false; //face à gauche
                swordScript.sword_right = false;
                // Place the gun on the left side of the player
                swordObject.transform.localPosition = new Vector3(-1.4f, swordObject.transform.localPosition.y, swordObject.transform.localPosition.z);
            }

            //PICKAXE
            if (pickaxeScript != null && pickaxeObject != null)
            {
                pickaxeScript.sr.flipX = false; //face à gauche
                pickaxeScript.pickaxe_right = false;
                // Place the gun on the left side of the player
                pickaxeObject.transform.localPosition = new Vector3(-1.4f, pickaxeObject.transform.localPosition.y, pickaxeObject.transform.localPosition.z);
                pickaxeScript.varDirectionMinage = -1;
            }

        }
        else if (horizontalMovement > 0 && !isFacingRight)
        {
            // Player is moving right and facing left, so flip the gun and sprite
            anim.SetBool(WALK_ANIMATION, true);
            sr.flipX = false;
            isFacingRight = true; // Update facing direction

            //GUN
            if (gunScript != null && gunObject != null)
            {
                gunScript.sr.flipX = true; //face à droite
                gunScript.bullet_right = false; //tire à droite
                gunScript.bulletSpawnPoint.localPosition = new Vector3(-2f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);

                // Place the gun on the right side of the player
                gunObject.transform.localPosition = new Vector3(1.4f, gunObject.transform.localPosition.y, gunObject.transform.localPosition.z);
            }

            //SWORD
            if (swordScript != null && swordObject != null)
            {
                swordScript.sr.flipX = true; //face à droite
                swordScript.sword_right = true;
                // Place the sword on the right side of the player
                swordObject.transform.localPosition = new Vector3(1.4f, swordObject.transform.localPosition.y, swordObject.transform.localPosition.z);
            }

            //PICKAXE
            if (pickaxeScript != null && pickaxeObject != null)
            {
                pickaxeScript.sr.flipX = true; //face à droite
                pickaxeScript.pickaxe_right = true;
                // Place the pickaxe on the right side of the player
                pickaxeObject.transform.localPosition = new Vector3(1.4f, pickaxeObject.transform.localPosition.y, pickaxeObject.transform.localPosition.z);
                pickaxeScript.varDirectionMinage = 1;
            }

        }
        else
        {
            anim.SetBool(WALK_ANIMATION, horizontalMovement != 0); // Update walking animation based on movement
        }
    }



    void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the object is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }


    void Jump() // Permet au joueur de sauter
    {
        rb.AddForce(Vector2.up * jumpForce2, ForceMode2D.Impulse);
        isGrounded = false;
        SoundEffects.Instance.MakeJumpingSound();
        Debug.Log(isGrounded);
    }

    IEnumerator JumpCooldown()
    {
        // Disable jumping
        canJump = false;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.2f);

        // Enable jumping again
        canJump = true;

    }
}