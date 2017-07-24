using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Rotation : MonoBehaviour
{
    //public float frequency1 = 0.1f;
    //public float frequency2 = 0.1f;

    //public float maxAngle1 = 360;
    //public float maxAngle2 = 360;

    //public Vector3 axis1 = Vector3.right;
    //public Vector3 axis2 = Vector3.up;

    //float seed1;
    //float seed2;

    //public GameObject model;

    void Start()
    {
        //seed1 = Random.value * 10;
        //seed1 = 10;
        //seed2 = Random.value * 10;
        //seed2 = 10;


    }

    void Update()
    {
        //rotate around object
        //moveX = transform.position.x++;
        //moveY = transform.position.y++;

        //Transform target = GetComponent<GameObject>().transform;
        //transform.localRotation = //rotating camera around center///
        //Quaternion.AngleAxis(Mathf.PerlinNoise(Time.time * frequency1, seed1) * maxAngle1, axis1) *
        //Quaternion.AngleAxis(Mathf.PerlinNoise(Time.time * frequency2, seed2) * maxAngle2, axis2);  //changed 360 to max angel 1 and 2
        /*
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * -20 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * 20 * Time.deltaTime);
        }
        */

        if(Input.GetButton("Horizontal"))
        {
            transform.Rotate(Vector3.up * CrossPlatformInputManager.GetAxis("Horizontal") * -20 * Time.deltaTime);
        }
      
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up * 40 * Time.deltaTime);
    }

    public void RotateLeft()
    {
        transform.Rotate(Vector3.up * -40 * Time.deltaTime);
    }

    /*
    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        m_Input = new Vector2(horizontal, vertical);
    }
    */
        /*
        public GameObject target;
        public float xOffset = 0;
        public float yOffset = 0;
        public float zOffset = 0;

        void LateUpdate()
        {
            this.transform.position = new Vector3(target.transform.position.x + xOffset,
                                                  target.transform.position.y + yOffset,
                                                  target.transform.position.z + zOffset);

        }
        */
    }
