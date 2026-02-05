using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//MATEJ
public class CharacterController2D : MonoBehaviour
{
    
	private float shiftAmountForFlip = 0.96f;
    private float shiftAmountForFlipTaskIndicatorArrow = 1.1f;
    [SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;
    [SerializeField] private Transform aboveHeadIcon;
    [SerializeField] private Transform NoMoreTasksIndicator;
    [SerializeField] private Transform directionIndicator;                      // DirectionIndicator  
    [SerializeField] private string whereDoesDirectionIndicatorPoint; // left,right, empty room


    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	public GameObject[] itemsToFlip;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		whereDoesDirectionIndicatorPoint = "right";

    }

	private void FixedUpdate()
	{
		
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
		string tmp = DirectionIndicatorScript.directionOfTask;
		
		//Debug.Log(whereDoesDirectionIndicatorPoint + " i od indikatorot " + tmp);
        if (whereDoesDirectionIndicatorPoint != tmp && whereDoesDirectionIndicatorPoint != null && tmp != null) {
			whereDoesDirectionIndicatorPoint = tmp;
			FlipTaskIndicator();

        }

	}

	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			
			if (GameManagerScript.instance.BlockKeyboard == false) {
                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.linearVelocity.y);
                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.linearVelocity = Vector3.SmoothDamp(m_Rigidbody2D.linearVelocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            }
			

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

    private void Flip()
    {
        
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        Vector3 indicatorWorldPos = Vector3.zero;
        if (directionIndicator != null)
        {
            indicatorWorldPos = directionIndicator.position;
        }
        

        // Save current scale
        Vector3 theScale = transform.localScale;
        // Flip scale
        theScale.x *= -1;
        transform.localScale = theScale;



        // Compensate position shift caused by flipping around pivot
        Vector3 position = transform.position;
        position.x += m_FacingRight ? shiftAmountForFlip : -shiftAmountForFlip;
        transform.position = position;

        //to put back to normal the aboveHeadIcon
        if (aboveHeadIcon != null)
        {
            Vector3 iconScale = aboveHeadIcon.localScale;
            iconScale.x *= -1;
            aboveHeadIcon.localScale = iconScale;
            
        }
        if (NoMoreTasksIndicator != null)
        {
            Vector3 iconScale = NoMoreTasksIndicator.localScale;
            iconScale.x *= -1;
            NoMoreTasksIndicator.localScale = iconScale;
        }
        if (directionIndicator != null)
        {
            Vector3 dirScale = directionIndicator.localScale;
            dirScale.x *= -1;
            directionIndicator.localScale = dirScale;
        }
		if (directionIndicator != null) {
            directionIndicator.position = indicatorWorldPos;
        }

        FlipObjects();
    }

    private void FlipTaskIndicator() {
		// Sega vo whereDoesDirectionIndicatorPoint imame nakaj treba da ja vrtime sega strelkata
		Transform arrow = directionIndicator.transform;


        Vector3 theScale = arrow.localScale;
        // Flip scale
        theScale.x *= -1;
        arrow.localScale = theScale;


        Vector3 position = arrow.position;
		
            
            //if (whereDoesDirectionIndicatorPoint == "left")
            //{ //znaci sea ja flipame na elvo strelkata
            //    position.x = position.x - shiftAmountForFlipTaskIndicatorArrow;
            //}
            //else
            //{ // koga ja flipame na desno strelkata
            //    position.x = position.x + shiftAmountForFlipTaskIndicatorArrow;
            //}
        



			arrow.position = position;

    }

    public void FlipObjects()
    {
        if (itemsToFlip == null || itemsToFlip.Length == 0)
            return;

        foreach (GameObject obj in itemsToFlip)
        {
            //if (obj == null) continue;

            Vector3 scale = obj.transform.localScale;
            scale.x *= -1;
            obj.transform.localScale = scale;
        }
    }

}
