using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static Unity.VisualScripting.Member;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] Vector3 respawnPos;
    bool grounded = false;
    bool on_ground = false;
    bool can_jump = true;
    float coyote_time = 0;
    public float max_coyote_time = 0.1f;
    float jump_cd_timer = 0;
    float jump_cd = 0.3f;
    Rigidbody2D rb;
    public float acc;
    public float max_speed;
    public float jump_vel;
    bool on_wall = false;
    bool wall_sliding = false;
    public float wall_coyote_time;
    float wall_coyote_timer = 0;
    public float wall_pushoff_mult;

    float gravity;
    public float max_fall_velocity;
    float max_fall_wall_mult = 4;
    Vector2 wall_normal = new Vector2(0, 0);

    bool jumping = false;
    public float max_jump_hold_time;
    float jump_hold_timer;

    bool jump_pressed = false;
    bool jump_released = false;

    float jump_early_timer = 0;
    public float jump_early_time;

    public TilemapManager tilemap;

    public float max_hp;
    public float hp;
    bool invul = false;
    public float invul_time;
    float invul_timer;

    bool disable_controls = false;
    public float disable_controls_time;
    float disable_controls_timer;

    SpriteRenderer render;

    [SerializeField] ChannelScriptable channelScriptable;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        disable_controls_timer = disable_controls_time;
        invul_timer = invul_time;
        hp = max_hp;
        jump_early_timer = jump_early_time;
        jump_hold_timer = max_jump_hold_time;
        jump_cd_timer = jump_cd;
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;

        channelScriptable.helpers.Clear();
        channelScriptable.enemies.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump_pressed = true;
            jump_early_timer = jump_early_time;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            jump_released = true;
        }
        if (!on_ground && grounded)
        {
            coyote_time -= Time.deltaTime;
            if (coyote_time < 0)
            {
                grounded = false;
            }
        }
        if (!on_wall && wall_sliding)
        {
            wall_coyote_time -= Time.deltaTime;
            if (wall_coyote_time < 0)
            {
                wall_sliding = false;
            }
        }
        if (!can_jump)
        {
            jump_cd_timer -= Time.deltaTime;
            if (jump_cd_timer < 0)
            {
                can_jump = true;
                jump_cd_timer = jump_cd;
            }
        }
        if (jump_pressed)
        {
            jump_early_timer-= Time.deltaTime;
            if (jump_early_timer < 0)
            {
                jump_pressed = false;
            }
        }
        if (invul)
        {
            invul_timer -= Time.deltaTime;
            if (invul_timer < 0)
            {
                invul_timer = invul_time;
                invul = false;
                render.color = new Color(render.color.r, render.color.g, render.color.b, 1);
            }
        }
        if (disable_controls)
        {
            disable_controls_timer -= Time.deltaTime;
            if (disable_controls_timer < 0)
            {
                disable_controls = false;
                disable_controls_timer = disable_controls_time;
            }
        }
    }

    private void FixedUpdate()
    {
        if (jumping)
        {
            jump_hold_timer -= Time.fixedDeltaTime;
            if (jump_hold_timer <= 0)
            {
                jump_hold_timer = max_jump_hold_time;
                jumping = false;
            }
        }

        if (rb.velocity.y < (wall_sliding ? -max_fall_velocity / max_fall_wall_mult : -max_fall_velocity))
        {
            rb.velocity = new Vector2(rb.velocity.x, (wall_sliding ? -max_fall_velocity / max_fall_wall_mult : -max_fall_velocity));
        }
        if (wall_sliding && rb.velocity.y < 0)
        {
            rb.gravityScale = gravity / 4;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        on_ground = false;

        if (CheckForGround(transform.position - new Vector3(0.4f, 0, 0), new Vector2(0, -1), 0.5f) ||
            CheckForGround(transform.position + new Vector3(0.4f, 0, 0), new Vector2(0, -1), 0.5f))
        {
            {
                on_ground = true;
                grounded = true;
                coyote_time = max_coyote_time;
            }
        }

        on_wall = false;

        if (CheckForGround(transform.position - new Vector3(0, 0.4f, 0), new Vector2(1, 0), 0.5f) ||
            CheckForGround(transform.position + new Vector3(0, 0.4f, 0), new Vector2(1, 0), 0.5f))
        {
            on_wall = true;
            wall_sliding = true;
            wall_normal = new Vector2(-1, 0);
            wall_coyote_timer = wall_coyote_time;
        }
        if (CheckForGround(transform.position - new Vector3(0, 0.4f, 0), new Vector2(-1, 0), 0.5f) ||
            CheckForGround(transform.position + new Vector3(0, 0.4f, 0), new Vector2(-1, 0), 0.5f))
        {
            on_wall = true;
            wall_sliding = true;
            wall_normal = new Vector2(1, 0);
            wall_coyote_timer = wall_coyote_time;
        }

        if (!disable_controls)
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && rb.velocity.x != 0)
            {
                if (Mathf.Abs(rb.velocity.x) < acc * 1.25f * Time.fixedDeltaTime)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else if (rb.velocity.x < 0)
                {
                    rb.AddForce(new Vector2(acc * 1.25f, 0));
                }
                else
                {
                    rb.AddForce(new Vector2(-acc * 1.25f, 0));
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if (grounded && rb.velocity.x > 0)
                    {
                        rb.AddForce(new Vector2(-acc * 2, 0));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(-acc, 0));
                    }
                    if (rb.velocity.x < -max_speed) { rb.velocity = new Vector2(-max_speed, rb.velocity.y); }
                }
                if (Input.GetKey(KeyCode.D))
                {
                    if (grounded && rb.velocity.x < 0)
                    {
                        rb.AddForce(new Vector2(acc * 2, 0));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(acc, 0));
                    }
                    if (rb.velocity.x > max_speed) { rb.velocity = new Vector2(max_speed, rb.velocity.y); }
                }
            }
            if (jump_pressed && can_jump)
            {
                if (grounded)
                {
                    jump_pressed = false;
                    rb.gravityScale = gravity;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    jumping = true;
                    grounded = false;
                    on_ground = false;
                    can_jump = false;
                }
                else if (wall_sliding)
                {
                    jump_pressed = false;
                    rb.velocity = new Vector2(0, 0);
                    rb.AddForce(wall_normal * wall_pushoff_mult, ForceMode2D.Impulse);
                    on_wall = false;
                    wall_sliding = false;
                    rb.gravityScale = gravity;
                    can_jump = false;
                    jumping = true;
                }
            }
            if (jumping && jump_released)
            {
                jumping = false;
                jump_hold_timer = max_jump_hold_time;
            }
            else if (jumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump_vel);
            }
        }

        jump_released = false;
    }

    bool CheckForGround(Vector3 start, Vector3 end, float distance)
    {
        RaycastHit2D[] ray = Physics2D.RaycastAll(start, end, distance);
        for (int i = 0; i < ray.Length; i++)
        {
            if (ray[i].collider.gameObject.tag == "Ground")
            {
                Vector3 trueend = start + end * distance;
                string name = tilemap.GetName(Mathf.RoundToInt(trueend.x),Mathf.RoundToInt(trueend.y));
                if (name != null)
                {
                    print(name);
                    if (channelScriptable.helpers.Contains(name))
                    {
                        channelScriptable.helpers.Remove(name);
                        channelScriptable.helpers.Insert(0, name);
                    }
                    else
                    {
                        channelScriptable.helpers.Insert(0,name);
                        if (channelScriptable.helpers.Count > 5)
                        {
                            channelScriptable.helpers.RemoveAt(5);
                        }
                    }
                }
                return true;
            }
        }
        return false;
    }

    public void Damage(Vector3 source, string name)
    {
        if (!invul)
        {
            if (name != null)
            {
                print(name);
                if (channelScriptable.enemies.Contains(name))
                {
                    channelScriptable.enemies.Remove(name);
                    channelScriptable.enemies.Insert(0, name);
                }
                else
                {
                    channelScriptable.enemies.Insert(0, name);
                    if (channelScriptable.enemies.Count > 5)
                    {
                        channelScriptable.enemies.RemoveAt(5);
                    }
                }
            }
            hp -= 1;
            if (hp <= 0)
            {
                SceneManager.LoadScene("Lose");
            }
            else
            {
                invul = true;
                render.color = new Color(render.color.r, render.color.g, render.color.b, 0.5f);
                Vector3 push_direction = transform.position - source;
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(push_direction.x, push_direction.y) * 10, ForceMode2D.Impulse);
                jumping = false;
                jump_hold_timer = max_jump_hold_time;
                disable_controls = true;
            }

        }
    }

    public void ResetPos()
    {
        TakeDamage();
        transform.position = respawnPos;
    }

    void TakeDamage()
    {
        hp -= 1;
        if (hp <= 0)
        {
            SceneManager.LoadScene("Lose");
        }
        else
        {
            invul = true;
            render.color = new Color(render.color.r, render.color.g, render.color.b, 0.5f);
            jumping = false;
            jump_hold_timer = max_jump_hold_time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "deathPlane")
        {
            ResetPos();
        }

        if (collision.gameObject.tag == "Goal")
        {
            SceneManager.LoadScene("Win");
        }
    }

    public bool Heal(string name)
    {
        if (hp <= max_hp)
        {
            hp += 1;
            if (channelScriptable.helpers.Contains(name))
            {
                channelScriptable.helpers.Remove(name);
                channelScriptable.helpers.Insert(0, name);
            }
            else
            {
                channelScriptable.helpers.Insert(0, name);
                if (channelScriptable.helpers.Count > 5)
                {
                    channelScriptable.helpers.RemoveAt(5);
                }
            }
            return true;
        }
        return false;
    }
}
