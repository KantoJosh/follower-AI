using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius = 5f;
    public float forwardRadius = 5f/2;
    public CharacterController controller;
    public float speed = 4f;
    void Start()
    {
        
    }
    void deleteIndicators()
    {
        GameObject[] validPositions = GameObject.FindGameObjectsWithTag("validFollow");
        GameObject[] invalidPositions = GameObject.FindGameObjectsWithTag("invalidFollow");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");

        foreach (GameObject go in validPositions)
        {
            Destroy(go);
        }

        foreach (GameObject go in invalidPositions)
        {
            Destroy(go);
        }

        foreach (GameObject go in obstacles)
        {
            Destroy(go);
        }
    }

    void drawIndicators()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            deleteIndicators();
            /*
            Dictionary<Vector3,float> positions = generateCandidatePositions();
            Debug.Log("Positions=");
            foreach (KeyValuePair<Vector3, float> kvp in positions)
            {
                Debug.Log(kvp.Key + " = " + kvp.Value);
            }
            GameObject[] validPositions = GameObject.FindGameObjectsWithTag("validFollow");
            GameObject[] invalidPositions = GameObject.FindGameObjectsWithTag("invalidFollow");
            foreach (GameObject go in validPositions)
            {
                Renderer go_render = go.GetComponent<Renderer>();
                go_render.material.color = Color.green;
            }

            foreach (GameObject go in invalidPositions)
            {
                Renderer go_render = go.GetComponent<Renderer>();
                go_render.material.color = Color.red;
            }
            */

        }
        // delete the follow position indicators
        else if (Input.GetKeyDown(KeyCode.F))
        {
            deleteIndicators();
        }
    }
    /*
    public Vector3? getBestPosition(Vector3 currentPosition, bool debug = true)
    {
        Dictionary<Vector3, float> candidatePositions = generateCandidatePositions(debug);
        Vector3? maxVec = null;
        List<Vector3> bestPositions = new List<Vector3>();
        if (candidatePositions.Count != 0)
        {
            float maxScore = 0f;
            foreach (Vector3 vec in candidatePositions.Keys)
            {
                if (candidatePositions[vec] >= maxScore)
                {
                    maxScore = candidatePositions[vec];
                    maxVec = vec;
                }
            }
        }
        return maxVec;
    }
    */
    public Vector3? getBestPosition(Vector3 followerPosition, bool debug = true)
    {
        Dictionary<Vector3, float> candidatePositions = generateCandidatePositions(followerPosition,debug);
        List<Vector3> bestPositions = new List<Vector3>();
        if (candidatePositions.Count != 0)
        {
            float maxScore = 0f;
            foreach (Vector3 vec in candidatePositions.Keys)
            {
                if (candidatePositions[vec] > maxScore)
                {
                    maxScore = candidatePositions[vec];
                    bestPositions.Clear();
                    bestPositions.Add(vec);
                }
                else if (candidatePositions[vec] == maxScore)
                {
                    bestPositions.Add(vec);
                }
            }
        }

        if (bestPositions.Count == 0 || bestPositions.Contains(followerPosition))
        {
            return null;
        }
        else
        {
            return bestPositions[0];
        }
    }

    /*
    Dictionary<Vector3,float> generateCandidatePositions(bool debug = true)
    {
        Color rayColor;
        Color colorForward;
        Color colorLOSForward;
        Dictionary<Vector3, float> candidatePositions = new Dictionary<Vector3, float>();
        Vector3 vector = transform.forward;

        foreach (int angle in new List<int> { 110, 30, 40, 40, 30 })
        {
            RaycastHit hit;
            vector = Quaternion.Euler(0, -1 * angle, 0) * vector;   // rotate vector by angle
            bool isHit = Physics.Raycast(transform.position, vector, out hit, radius);


            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.position = transform.position + vector.normalized * radius;
            cylinder.transform.localScale = new Vector3(0.25f, 1f, 0.25f);

            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = cylinder.transform.position + transform.forward * radius / 1.5f;
            //cube.tag = "obstacle";
            bool isHitForward = Physics.Raycast(cylinder.transform.position, transform.forward, out hit, forwardRadius);

            Vector3 forwardPos = cylinder.transform.position + transform.forward * forwardRadius;
            bool isHitLOSForward = Physics.Raycast(transform.position, forwardPos - transform.position, out hit, Vector3.Distance(transform.position,forwardPos));


            if (isHit)
            {
                cylinder.tag = "invalidFollow";
                rayColor = Color.red;
            }
            else
            {
                cylinder.tag = "validFollow";
                rayColor = Color.green;
            }

            if (isHitForward)
            {
                colorForward = Color.red;
            }
            else
            {
                colorForward = Color.green;
            }

            if (isHitLOSForward)
            {
                colorLOSForward = Color.red;
            }
            else
            {
                colorLOSForward = Color.green;
            }


            if (debug)
            {
                Debug.DrawRay(cylinder.transform.position, transform.forward * radius / 2, colorForward, 10);
                Debug.DrawRay(transform.position, vector.normalized * radius, rayColor, 10);
                Debug.DrawRay(transform.position, forwardPos - transform.position, colorLOSForward, 10);

            }

            Renderer cylinder_render = cylinder.GetComponent<Renderer>();
            if (!isHit && !isHitForward && !isHitLOSForward)
            {
                cylinder_render.material.color = Color.green;
                float position_score = scorePosition(cylinder.transform.position);
                candidatePositions.Add(cylinder.transform.position,position_score);
            }
            else
            {
                cylinder_render.material.color = Color.red;
            }

        }
        return candidatePositions;
    }
    */

    Dictionary<Vector3, float> generateCandidatePositions(Vector3 followerPosition,bool debug = true)
    {
        Color rayColor;
        Color colorForward;
        Color colorLOSForward;
        Dictionary<Vector3, float> candidatePositions = new Dictionary<Vector3, float>();
        Vector3 vector = transform.forward;

        foreach (int angle in new List<int> { 110, 30, 40, 40, 30 })
        {
            RaycastHit hit;
            RaycastHit hitForward;
            RaycastHit hitLOSForward;
            vector = Quaternion.Euler(0, -1 * angle, 0) * vector;   // rotate vector by angle
            bool isHit = Physics.Raycast(transform.position, vector, out hit, radius);

            Vector3 cylinderPosition = transform.position + vector.normalized * radius;
            bool isHitForward = Physics.Raycast(cylinderPosition, transform.forward, out hitForward, forwardRadius);

            Vector3 forwardPos = cylinderPosition + transform.forward * forwardRadius;
            bool isHitLOSForward = Physics.Raycast(transform.position, forwardPos - transform.position, out hitLOSForward, Vector3.Distance(transform.position, forwardPos));


            rayColor = (isHit && !hit.transform.CompareTag("Follower")) ? Color.red : Color.green;
            colorForward = isHitForward ? Color.red : Color.green;
            colorLOSForward = (isHitLOSForward && !hitLOSForward.transform.CompareTag("Follower")) ? Color.red : Color.green;

            if (debug)
            {
                Debug.DrawRay(cylinderPosition, transform.forward * radius / 2, colorForward, 10);
                Debug.DrawRay(transform.position, vector.normalized * radius, rayColor, 10);
                Debug.DrawRay(transform.position, forwardPos - transform.position, colorLOSForward, 10);

            }
            /*
            Debug.Log(angle.ToString() + " " + isHit.ToString() + " " + isHitForward.ToString() + " " + isHitLOSForward.ToString() + " ");
            if (isHit)
            {
                Debug.Log(hit.transform.tag);
            }
            
            if (hit.transform)
            {
                Debug.Log(hit.transform.tag);
            }
            else
            {
                Debug.Log("N/A");
            }
            */
            if ((!isHit || isHit && hit.transform.CompareTag("Follower")) && !isHitForward && !isHitLOSForward)
            {
                float position_score = scorePosition(followerPosition,cylinderPosition);
                candidatePositions.Add(cylinderPosition, position_score);
                Debug.Log(cylinderPosition.ToString() + position_score.ToString());
            }


        }
        /*
        foreach (Vector3 vec in candidatePositions.Keys)
        {
            Debug.Log(vec.ToString() + " " + candidatePositions[vec].ToString());
        }
        */
        return candidatePositions;
    }

    float scorePosition(Vector3 position,Vector3 followerPosition)
    {
        float distanceToLeader = 1 / Vector3.Distance(transform.position, position);
        float distanceToPosition = 1 / Vector3.Distance(followerPosition, position);
        return distanceToLeader + distanceToPosition;
    }
}
