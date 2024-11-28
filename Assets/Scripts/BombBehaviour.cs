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
    public string chatter;
    // Start is called before the first frame update
    void Start()
    {
        tilemapManager = GameObject.FindGameObjectWithTag("tilemap").GetComponent<TilemapManager>();
    }

    public void Fall()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
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
                tilemapManager.RemoveTiles(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), radius - 1);
                exploded = true;
                float newrad = radius * 2 - 1.3f;
                transform.localScale = new Vector3(newrad, newrad, newrad);
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
                GetComponent<Rigidbody2D>().simulated = false;
                Collider2D[] objs = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(newrad,newrad),0);
                print(objs.Length);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].gameObject.tag == "Player")
                    {
                        objs[i].gameObject.GetComponent<PlayerBehaviour>().Damage(transform.position, chatter);
                    }
                }
            }
        }
    }
}
