using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanguardController : MonoBehaviour
{
    public CharacterController controller;
    public Transform camera;
    static Animator anim;
    //public float speed = 4f;
    public float maxSpeed = 4f;
    public float rotationSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        */

        /*
        float vertAxis = Input.GetAxis("Vertical");
        float translation = vertAxis * speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (translation != 0)
        {
            if (vertAxis < 0)
            {
                //anim.SetBool("isWalkingForward", false);
                anim.SetBool("isWalkingBackward", true);
            }
            else
            {
                //anim.SetBool("isWalkingBackward", false);
                anim.SetBool("isWalkingForward", true);
            }
        }
        else
        {
            anim.SetBool("isWalkingBackward", false);
            anim.SetBool("isWalkingForward", false);
        }
        */


        ///new
        ///
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        float speed = inputMagnitude * maxSpeed;
        movementDirection = Quaternion.AngleAxis(camera.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        if (verticalInput * speed * Time.deltaTime != 0)
        {
            if (verticalInput < 0)
            {
                anim.SetBool("isWalkingBackward", true);
            }
            else
            {
                anim.SetBool("isWalkingForward", true);
            }
        }
        else
        {
            anim.SetBool("isWalkingBackward", false);
            anim.SetBool("isWalkingForward", false);
        }

        Vector3 velocity = movementDirection * speed;
        controller.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
