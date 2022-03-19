using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
//test
    private float speed;
    [SerializeField] private float walkSpeed = 2;
    [SerializeField] private float runSpeed = 4;
    private Animator animator;
    private Rigidbody rb;
    private bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        speed = walkSpeed;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var vVelocity = Input.GetAxis("Vertical") * speed * Vector3.forward;
        var hVelocity = Input.GetAxis("Horizontal") * speed * Vector3.right;

        //Clamping the speed so that speed makes sense
        float vSpeed = Mathf.Clamp(vVelocity.z, -walkSpeed, runSpeed);
        float hSpeed = Mathf.Clamp(hVelocity.x, -runSpeed, runSpeed);

        animator.SetFloat("vSpeed", vSpeed);
        animator.SetFloat("hSpeed", hSpeed);

        float horizontal = Time.deltaTime * hSpeed;
        float vertical = Time.deltaTime * vSpeed;

        transform.Translate(horizontal, 0, vertical);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            animator.SetBool("grounded", grounded);
        }
    }
}
