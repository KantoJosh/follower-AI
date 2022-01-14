using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Vector3 target;
    public float speed = 5f;
    static Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        setTarget(new Vector3(transform.position.x + 10, transform.position.y, transform.position.z + 10));
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            anim.SetTrigger("isWalking");
        }
    }

    void setTarget(Vector3 newTarget)
    {
        target = newTarget;
        transform.LookAt(target);
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
