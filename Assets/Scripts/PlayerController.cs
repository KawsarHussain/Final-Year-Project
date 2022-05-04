using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Config")]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float throwForce;
    [SerializeField] public float throwUpwardForce;
    [SerializeField] public Transform playerCamera = null;
    [SerializeField] public float mouseSensitivity = 3.5f;
    [SerializeField] public bool lockCursor = true;
    public List<Wristband> bands = new List<Wristband>();
    private List<GameObject> bandObjects = new List<GameObject>();

    [Header("Interact Config")]
    [SerializeField] private TextMeshPro useText;
    [SerializeField] private float maxUseDistance = 5f;
    [SerializeField] LayerMask useLayers;

    public Transform throwPointOne = null;
    public Transform throwPointTwo = null;
    private Player player;
    private float speed;
    private float cameraPitch = 0.0f;
    private Animator animator;
    private Rigidbody rb;
    private bool grounded = true;
    private Vector3 teleportOffset = new Vector3(0, 0.415f, 0);


    // Start is called before the first frame update
    void Start()
    {
        player = new Player(2);
        bands.Add(GameObject.Find("Band1").GetComponent<Wristband>());
        bandObjects.Add(null);
        if (player.GetAmountOfBands() == 2)
        {
            bands.Add(GameObject.Find("Band2").GetComponent<Wristband>());
            bandObjects.Add(null);
        }
        throwForce = 35;
        throwUpwardForce = 0;
        walkSpeed = player.GetWalkSpeed();
        runSpeed = player.GetRunSpeed();
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

        //UpdateText();
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
            rb.AddForce(new Vector3(0, 250, 0));
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

        //Handling throwing
        if(Input.GetButtonDown("Fire1"))
        {
            if (bands[0].GetThrown())
            {
                if (bandObjects[0].transform.parent == null || !bandObjects[0].transform.parent.CompareTag("Teleportable")) TeleportToBand(0);
                else if (bandObjects[0].transform.parent.CompareTag("Teleportable")) SwapPositions(0);
            }
            else ThrowBand(0);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (bands[1].GetThrown())
            {
                if (bandObjects[1].transform.parent == null || !bandObjects[1].transform.parent.CompareTag("Teleportable")) TeleportToBand(1);
                else if (bandObjects[1].transform.parent.CompareTag("Teleportable")) SwapPositions(1);
            }
            else ThrowBand(1);
        }

        if (Input.GetButtonDown("Reload")) ReturnBand();

        if (Input.GetButtonDown("Swap"))
        {
            if (bandObjects[0].transform.parent.CompareTag("Teleportable") && bandObjects[1].transform.parent.CompareTag("Teleportable"))
            {
                SwapPositions();
            }
        }

        //Handles opening doors
        if (Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, maxUseDistance, useLayers))
            {
                //If raycast has hits a door component
                if (hit.collider.TryGetComponent<Door>(out Door door))
                {
                    if (door.GetOpen()) door.Close();
                    else door.Open(transform.position);
                }
            }
        }
    }

    //This method updates text on the screen
    private void UpdateText()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, maxUseDistance, useLayers) 
            && hit.collider.TryGetComponent<Door>(out Door door))
        {
            if (door.GetOpen()) useText.SetText("Close Door \"E\"");
            else useText.SetText("Open Door \"E\"");
            useText.gameObject.SetActive(true);
            //offsets text to prevent Z-fighting
            useText.transform.position = hit.point - (hit.point - playerCamera.position).normalized * 0.01f;
            useText.transform.rotation = Quaternion.LookRotation((hit.point - playerCamera.position).normalized);

        }
        else
        {
            useText.gameObject.SetActive(false);
        }
    }

    private void ThrowBand(int n)
    {
        bool status;
        Vector3 pos = throwPointOne.position;
        if (n == 0) 
        {
            status = ThrowBandOne();
            if (!status) return;
        }

        else if (n == 1)
        {
            status = ThrowBandTwo();
            if (!status) return;
            pos = throwPointTwo.position;
        }


        GameObject projectile = Instantiate(bands[n].GetBandObject(), pos, playerCamera.rotation);
        bandObjects[n] = projectile;

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();


        Vector3 forceDirection = playerCamera.transform.forward;

        RaycastHit hit; // raycast created through the centre of the screen

        //calculate the correct direction that band should be thrown
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 500f))
        {
            forceDirection = (hit.point - pos).normalized;
        }

        Vector3 force = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectile.transform.parent = null;

        projectileRb.AddForce(force, ForceMode.Impulse);        
    }

    //Player is going to have two bands so seperate methods are going to be made for them
    public bool ThrowBandOne()
    {
        //if first band hasn't been thrown
        if (!bands[0].GetThrown())
        {
            bands[0].UpdateThrown(true);
            player.IncreaseAmountThrown();
            return true;
        }
        return false;
    }

    public bool ThrowBandTwo()
    {
        if (bands.Count != 2) return false;
        if (!bands[1].GetThrown())
        {
            bands[1].UpdateThrown(true);
            player.IncreaseAmountThrown();
            return true;
        }
        return false;
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
    public void TeleportToBand(int n)
    {
        this.transform.position = bandObjects[n].transform.position;
        Destroy(bandObjects[n]);
        bands[n].UpdateThrown(false);
        player.ReduceAmountThrown();
    }

    private void setKinematic(int n, bool value)
    {
        bandObjects[n].transform.parent.GetComponent<Rigidbody>().isKinematic = value;
    }

    //Used to swap positions between teleportable object and player
    public void SwapPositions(int n)
    {
        /*setKinematic(n, false);*/
        Vector3 playerPos = this.transform.position; //saves player position
        this.transform.position = bandObjects[n].transform.parent.position;
        bandObjects[n].transform.parent.position = playerPos + teleportOffset;
    }

    //Used to swap positions between teleportable objects
    public void SwapPositions()
    {
        Vector3 objectOnePos = bandObjects[0].transform.parent.position; //saves objectOne position
        bandObjects[0].transform.parent.position = bandObjects[1].transform.parent.position + teleportOffset;
        bandObjects[1].transform.parent.position = objectOnePos + teleportOffset;
    }

    //Used to return all bands the player has thrown out onto the field
    public void ReturnBand()
    {
        if (bands[0].GetThrown())
        {
            bands[0].UpdateThrown(false);
            Destroy(bandObjects[0]);
            player.ReduceAmountThrown();
        }
        if (bands.Count == 2)
        {
            if (bands[1].GetThrown())
            {
                bands[1].UpdateThrown(false);
                Destroy(bandObjects[1]);
                player.ReduceAmountThrown();
            }
        }
    }
}
