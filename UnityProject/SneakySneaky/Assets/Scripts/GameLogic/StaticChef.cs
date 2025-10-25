using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticChef : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("Working", 5);
    }
    public void Confusion(GameObject trans)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
