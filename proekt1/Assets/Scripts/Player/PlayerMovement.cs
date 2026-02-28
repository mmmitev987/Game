using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 23f;
    public Animator animator;

    float horizontalMove = 0f;

    

    // Update is called once per frame
    void Update()
    {
        if (GameManagerScript.instance.BlockKeyboard == false)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }
        else {
            animator.SetFloat("Speed", 0);
        }
        
        
        

    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
    }
}
