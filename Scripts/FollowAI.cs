using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAI : MonoBehaviour
{
    public BuddyAI buddyAIScript;
    public float speed = 12f;
    public bool debug = false;
    public Transform leader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != new Vector3() || Vector3.Distance(transform.position,leader.position) > buddyAIScript.radius / 4)
        {

            if (debug)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Vector3? positionToBeIn = buddyAIScript.getBestPosition(transform.position);
                    if (positionToBeIn.HasValue && transform.position != positionToBeIn)
                    {
                        Debug.Log("Moving from" + transform.position.ToString() + "to " + positionToBeIn.ToString());
                        transform.position = positionToBeIn.Value;
                    }
                }
            }
            else
            {
                Vector3? positionToBeIn = buddyAIScript.getBestPosition(transform.position, false);
                if (positionToBeIn.HasValue && transform.position != positionToBeIn)
                {
                    Debug.Log("Moving from" + transform.position.ToString() + "to " + positionToBeIn.ToString());
                    transform.position = Vector3.MoveTowards(transform.position, positionToBeIn.Value, speed * Time.deltaTime);
                }
            }
        }
    }
}
