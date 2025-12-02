using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float ForceBounce = 5f;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
            collision.rigidbody.AddForce(transform.up * ForceBounce, ForceMode.Impulse);
    }
}
