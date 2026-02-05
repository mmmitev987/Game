using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Platform : MonoBehaviour {

    public float Jump_Force;

    void OnCollisionEnter2D(Collision2D Other)
    {
        // Add force when player fall from top
        if (-Other.relativeVelocity.y <= 0f)
        {
            Rigidbody2D Rigid = Other.collider.GetComponent<Rigidbody2D>();

            if (Rigid != null)
            {
                //Vector2 Force = Rigid.linearVelocity;
                //Force.y = Jump_Force;
                //Rigid.linearVelocity = Force;

                Vector2 force = Rigid.linearVelocity;
                force.y = Jump_Force;
                Rigid.linearVelocity = force;

                // Play jump sound
                GetComponent<AudioSource>().Play();
            }
        }
    }

    public void loadGame() {
        SceneManager.LoadSceneAsync("DoodleJumpClassroom");
    }
}
