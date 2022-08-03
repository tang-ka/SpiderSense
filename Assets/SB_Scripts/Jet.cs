using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Jet : MonoBehaviour
{
    public enum Jet_State
    {
        IDLE,
        FORWARD,
        RIGHT,
        LEFT,
        BACK,
        ATTACK
    }

    public Jet_State state;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");

        //Vector3 dir = transform.forward * v + transform.right * h;

        //dir.Normalize();

        //Move(dir);

       
        
        if(state == Jet_State.FORWARD)
        {
            Move(transform.forward);
        }
        else if(state == Jet_State.RIGHT)
        {
            Move(transform.right);
        }

        else if (state == Jet_State.LEFT)
        {
            Move(-transform.right);
        }
        else if (state == Jet_State.BACK)
        {
            Move(-transform.forward);
        }
        else if(state == Jet_State.ATTACK)
        {
            Attack();
        }

    }

    void Move(Vector3 dir)
    {
        transform.position += dir * 5 * Time.deltaTime;
    }

    void Attack()
    {
        //ÃÑ¾Ë¹ß»ç
    }
}
