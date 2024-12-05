using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBehaviour : MonoBehaviour
{
    public string chatter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // passes on the viewer that spawned it
            collision.gameObject.GetComponent<PlayerBehaviour>().Damage(transform.position, chatter);
        }
    }
}
