using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    public string chatter_name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bool success = collision.gameObject.GetComponent<PlayerBehaviour>().Heal(chatter_name);
            if (success)
            {
                Destroy(gameObject);
            }

        }
    }
}
