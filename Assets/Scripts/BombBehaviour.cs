using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    TilemapManager tilemapManager;
    [SerializeField] float timer;
    bool exploded = false;
    public int radius;
    [SerializeField] float explode_timer;
    // Start is called before the first frame update
    void Start()
    {
        tilemapManager = GameObject.FindGameObjectWithTag("tilemap").GetComponent<TilemapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded)
        {
            explode_timer -= Time.deltaTime;
            if (explode_timer < 0 )
            {
                Destroy(gameObject);
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                tilemapManager.RemoveTiles((int)transform.position.x, (int)transform.position.y, radius - 1);
                exploded = true;
                float newrad = radius * 2 - 1.3f;
                transform.localScale = new Vector3(newrad, newrad, newrad);
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
            }
        }
    }
}
