using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D Rigid;
    public float Movement_Speed = 10f;
    private float Movement = 0;
    private Vector3 Player_LocalScale;

    public Sprite[] Spr_Player = new Sprite[2];

    void Start()
    {
        Rigid = GetComponent<Rigidbody2D>();
        Player_LocalScale = transform.localScale;
    }

    void Update()
    {
        float moveInput = 0f;

#if UNITY_STANDALONE || UNITY_WEBGL   // PC / Web Controls
        moveInput = Input.GetAxisRaw("Horizontal");
#endif

#if UNITY_ANDROID || UNITY_IOS        // Mobile Tilt Controls
        moveInput = Input.acceleration.x;
#endif

        Movement = moveInput * Movement_Speed;

        // Flip player depending on direction
        if (Movement > 0)
            transform.localScale = new Vector3(Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
        else if (Movement < 0)
            transform.localScale = new Vector3(-Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
    }

    //void FixedUpdate()
    //{
    //    Vector2 Velocity = Rigid.linearVelocity;
    //    Velocity.x = Movement;
    //    Rigid.linearVelocity = Velocity;

    //    // Sprite change & collider toggle
    //    if (Velocity.y < 0)
    //    {
    //        GetComponent<SpriteRenderer>().sprite = Spr_Player[0];
    //        GetComponent<BoxCollider2D>().enabled = true;
    //        Propeller_Fall();
    //    }
    //    else
    //    {
    //        GetComponent<SpriteRenderer>().sprite = Spr_Player[1];
    //        GetComponent<BoxCollider2D>().enabled = false;
    //    }

    //    // Screen wrap
    //    Vector3 Top_Left = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
    //    float Offset = 0.5f;

    //    if (transform.position.x > -Top_Left.x + Offset)
    //        transform.position = new Vector3(Top_Left.x - Offset, transform.position.y, transform.position.z);
    //    else if (transform.position.x < Top_Left.x - Offset)
    //        transform.position = new Vector3(-Top_Left.x + Offset, transform.position.y, transform.position.z);
    //}
    void FixedUpdate()
    {
        //Vector2 velocity = Rigid.linearVelocity;
        Vector2 velocity = Rigid.linearVelocity;
        velocity.x = Movement;
        //Rigid.linearVelocity = velocity;
        Rigid.linearVelocity = velocity;

        // Sprite change & collider toggle
        if (velocity.y < 0)
        {
            GetComponent<SpriteRenderer>().sprite = Spr_Player[0];
            GetComponent<BoxCollider2D>().enabled = true;
            Propeller_Fall();
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Spr_Player[1];
            GetComponent<BoxCollider2D>().enabled = false;
        }

        // Screen wrap (FIXED)
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float offset = 0.5f;

        if (Rigid.position.x > -topLeft.x + offset)
            Rigid.position = new Vector2(topLeft.x - offset, Rigid.position.y);
        else if (Rigid.position.x < topLeft.x - offset)
            Rigid.position = new Vector2(-topLeft.x + offset, Rigid.position.y);
    }

    void Propeller_Fall()
    {
        if (transform.childCount > 0)
        {
            Propeller propeller = transform.GetChild(0).GetComponent<Propeller>();
            if (propeller != null)
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("Active", false);
                propeller.Set_Fall(gameObject);
                transform.GetChild(0).parent = null;
            }
        }
    }
}
