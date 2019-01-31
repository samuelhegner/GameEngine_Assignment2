using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Controller : MonoBehaviour
{
    Animator anim;

    public bool attackPosition;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Attack", attackPosition);
    }
}
