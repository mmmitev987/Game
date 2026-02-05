using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D rbody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;
    public AudioSource flyAudio;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && birdIsAlive) {
            playJumpAudio();
            rbody.linearVelocity = Vector2.up * flapStrength;
        }

    }
    void OnBecameInvisible()
    {
        birdIsAlive = false;
        logic.gameOver();
        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 6) {
            birdIsAlive = false;
            logic.gameOver();

        }
    }

    private void playJumpAudio() {
        flyAudio.Play();
    }
}
