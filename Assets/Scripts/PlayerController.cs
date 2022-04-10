using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float walkSpeed = 2;
    [SerializeField] public float runSpeed = 4;
    [SerializeField] public Transform playerCamera = null;
    [SerializeField] public float mouseSensitivity = 3.5f;
    [SerializeField] public bool lockCursor = true;

    private Player player;
    private float speed;
    private float cameraPitch = 0.0f;
    private Animator animator;
    private Rigidbody rb;
    private bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        speed = walkSpeed;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        //Whilst in a level, the cursor will be invisible
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();

        UpdateVelocity();

        InputHandle();
    }

    private void UpdateVelocity()
    {
        var vVelocity = Input.GetAxis("Vertical") * speed * Vector3.forward;
        var hVelocity = Input.GetAxis("Horizontal") * speed * Vector3.right;

        //Clamping the speed so that speed is in a certain range
        float vSpeed = Mathf.Clamp(vVelocity.z, -walkSpeed, runSpeed);
        float hSpeed = Mathf.Clamp(hVelocity.x, -runSpeed, runSpeed);

        animator.SetFloat("vSpeed", vSpeed);
        animator.SetFloat("hSpeed", hSpeed);

        float horizontal = Time.deltaTime * hSpeed;
        float vertical = Time.deltaTime * vSpeed;

        transform.Translate(horizontal, 0, vertical);
    }

    //This method handles the how the camera will transform based on mouse movement
    private void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //Horizontal motion of mouse
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);

        //Vertical motion of mouse
        cameraPitch -= mouseDelta.y * mouseSensitivity; //Subtracting fixes the issue with inverted y-axis
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 40.0f); //Prevents camera from flipping

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
    }

    //Method handles all inputs
    private void InputHandle()
    {
        //If spacebar is pressed, the character will jump
        if (Input.GetButtonDown("Jump") && grounded == true)
        {
            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            grounded = false;
            animator.SetBool("grounded", grounded);
        }

        //Run button assigned to Left Shift key
        if (Input.GetButtonDown("Run"))
        {
            speed = runSpeed;
        }

        if (Input.GetButtonUp("Run"))
        {
            speed = walkSpeed;
        }
    }

    //Method handles what happens when you enter into collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            animator.SetBool("grounded", grounded);
        }
    }

    //Method is used to teleport to a band if it hasn't stuck to a teleportable object
    public void TeleportToBand(Wristband band)
    {
        this.transform.position = band.GetCoords();
        player.ReduceAmountThrown();
    }

    //Used to swap positions between teleportable object and player
    public void SwapPositions(Wristband band)
    {
        Vector3 playerPos = this.transform.position; //saves player position
        this.transform.position = band.GetAttachedObjectCoords();
        band.GetAttachedObject().transform.position = playerPos;
        
        //Returns band to player if the object the band is attached to is not teleportable
        if (!band.GetAttachedObject().CompareTag("Teleportable"))
        {
            band.UpdateAttachedObject(null);
            band.UpdateThrown(false);
            player.ReduceAmountThrown();
        }
        
    }

    //Used to swap positions between teleportable objects
    public void SwapPositions(Wristband bandOne, Wristband bandTwo)
    {
        Vector3 objectOnePos = bandOne.GetAttachedObjectCoords(); //saves objectOne position
        bandOne.GetAttachedObject().transform.position = bandTwo.GetAttachedObjectCoords();
        bandTwo.GetAttachedObject().transform.position = objectOnePos;
    }

    //Used to return all bands the player has thrown out onto the field
    public void ReturnBand()
    {
        player.getBands()[0].UpdateThrown(false);
        player.getBands()[0].UpdateAttachedObject(null);
        if (player.getBands().Count == 2)
        {
            player.getBands()[1].UpdateThrown(false);
            player.getBands()[1].UpdateAttachedObject(null);
        }
    }
}
