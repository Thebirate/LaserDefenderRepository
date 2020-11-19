using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
     [SerializeField] float movementSpeed = 10f;
     [SerializeField] float laserSpeed = 20f;

     [SerializeField] GameObject laserPrefab;

     float xMin;
     float xMax;
     float yMin;
     float yMax;
       private void Move()
    {
        /* GetAxis is a method, found in the Input class, which retrieves the particular axis settings which its name is passed
         * as a parameter.
         * This method waits for any user input made by the keys specified by the axis. If there is in fact user input, small
         * values (+ve or -ve) are returned. 
         * We are saving this return in the deltaX variable.
         */

        // The deltaTime property is used to make the movement frame independent since the method is being called in the Update
        // and the number of times Move will be called in one second depends on the frame rate
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        //local variables
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        /*
         * To fetch properties from the Unity Editor the syntax is:
         *    object.ComponentName.Property
         * If the property is for the current object the script is controlling, the object does not need to be specified so:
         *     ComponentName.Property
         */

        /* Clamp restricts values to be between a set min and max. If the value is
         * within the range the same value will be returned, otherwiswe if the
         * value is less than the minimum, the minimum value is returned and if it
         * is higher than the maximum, the maximum will be returned.
         */

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector3(newXPos, newYPos, transform.position.z);

    }
    void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main; // fetching the main camera
        float padding = 0.5f;

        /* ViewportToWorldPoint checks the camera's view at runtime and calculates
         * the actual coordinates but in our code we just refer to the minimum as
         * 0 and maximum as 1.
         * Thus, the code will always be camera size independent.
         */

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;

    }

    private void Fire()
    {
        /* The GetButtonDown method returns true whenever as soon as the user presses down on the key/s 
         * represented by the given button name. The method is executed only once per one key down.
         * (GetButton would keep on running while the user keeps on holding the button)
         */

        if (Input.GetButtonDown("Fire1")) //if(Input.GetButtonDown("Fire1") == true)
        {
            /* The Instantiate method generates a clone/copy of the object which is passed as the first
             * parameter. There are different ways on how we can call this method, we needed to indicate
             * the position where the clone/copy will be created in the scene (we need it to appear in the
             * same position as the player ship).
             * Quaternion.identity refers to no rotation (0,0,0)
             * 
             * Instantiate returns a reference to the clone which has just been generated.
             */

            GameObject laserClone = Instantiate(laserPrefab, transform.position, Quaternion.identity);

            laserClone.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpBoundaries();
        
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        Move();
    }
}
