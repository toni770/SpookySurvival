using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Follows the target above a specific height
public class CameraFollow : MonoBehaviour
{
	///////////PUBLIC VARS/////////
	[Header("Config")]
    public float smoothSpeed = 0.125f;
    public float height = 6;
    public float distanceZ = -5;
	
	///////////PRIVATE VARS/////////
    Camera cam;
    Transform target;
    float desiredSize = 8;
    bool changeSize = false;

    //Change vars
    float smoothTime = 0.3f;
    float yVelocity = 0.0f;

    ///////////FUNCTIONS/////////
    void Awake()
	{
		cam = GetComponent<Camera>();
	}
	
    void FixedUpdate()
    {
        if(target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, target.position.z + distanceZ);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        if(changeSize)
        {
            if(cam.orthographicSize != desiredSize)
            {
                cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, desiredSize, ref yVelocity, smoothTime);
            }
            else
            {
                changeSize = false;
            }
        }

    }
	
	
    public void ConfigureCamera(Transform player) //Called by: Manager(Om configure game)
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(target.position.x, target.position.y + height, target.position.z + distanceZ);
        cam.orthographicSize = height;
    }

    public void IncreaseSize(float h)//Called by: Manager(Om configure game. Only Jester)
    {
        cam.orthographicSize += h;
    }
    public void ChangeSizeAnimation(float h)
    {
        changeSize = true;
        desiredSize = cam.orthographicSize + h;
    }
}
