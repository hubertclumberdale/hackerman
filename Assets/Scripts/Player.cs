using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float speed;
    public int timesJumped;
    public float jumpSpeed;
    public bool isGrounded;
    public bool isAttacking;
    public float attackDuration;
    public float deadZone;
    private float distToGround;
    private Vector3 movementDirection;
    private Rigidbody rigidBody;
    private Animator meshAnimator;
    public GameObject mazza;
    private TouchControlManager touchControls;
    // Start is called before the first frame update
    
    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
        meshAnimator = transform.GetChild(0).GetComponent<Animator>();
        touchControls = TouchControlManager.Instance;
    }

    // Update is called once per frame
    void FixedUpdate(){
        MovePlayer();
    }

    private void Update()
    {
        PlayerAttack();
        PlayerJump();
    }

    void PlayerAttack(){
        // Don't attack if clicking on UI elements
        bool isOverUI = EventSystem.current.IsPointerOverGameObject();
        
        bool attackInput = Input.GetButtonDown("Fire1") && !isOverUI;
        
        // Check for touch input if available (touch buttons are separate)
        if (touchControls != null)
        {
            attackInput = attackInput || touchControls.GetAttackInput();
        }
        
        if(attackInput){
            meshAnimator.SetTrigger("attack");
            AudioManager.Instance.PlaySwearingsSound();
            StartCoroutine(AttackCoroutine(attackDuration));
        }
    }

    IEnumerator AttackCoroutine(float attackDuration){
        isAttacking = true;
        mazza.GetComponent<TrailRenderer>().enabled = true;
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        mazza.GetComponent<TrailRenderer>().enabled = false;
    }

    void MovePlayer(){
        float horizontal = Input.GetAxis("Horizontal");
        
        // Add touch input if available
        if (touchControls != null)
        {
            float touchHorizontal = touchControls.GetHorizontalInput();
            if (Mathf.Abs(touchHorizontal) > 0)
            {
                horizontal = touchHorizontal;
            }
        }

        if(Mathf.Abs(horizontal) > deadZone){
            AudioManager.Instance.PlayMovementSound();
            if(horizontal > 0){
                transform.localEulerAngles = new Vector3(0, 90, 0);
            } else {
                transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            
            // Usa AddForce con VelocityChange per movimento più naturale
            Vector3 targetVelocity = new Vector3(horizontal * speed, rigidBody.linearVelocity.y, 0);
            Vector3 velocityChange = targetVelocity - rigidBody.linearVelocity;
            velocityChange.y = 0; // Non modificare la velocità Y (salto/gravità)
            
            rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
            
            meshAnimator.SetBool("isRunning", true);
        } else {
            // Ferma il movimento orizzontale gradualmente
            Vector3 stopForce = new Vector3(-rigidBody.linearVelocity.x, 0, 0);
            rigidBody.AddForce(stopForce, ForceMode.VelocityChange);
            meshAnimator.SetBool("isRunning", false);
        }

   
    }

    void PlayerJump(){
        bool jumpInput = Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.W);
        
        // Check for touch input if available
        if (touchControls != null)
        {
            jumpInput = jumpInput || touchControls.GetJumpInput();
        }
        
        if(jumpInput && timesJumped < 1){  
            rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);        
            timesJumped++;
        }
        isGrounded = IsGrounded();
        if(isGrounded){
            timesJumped = 0;
            meshAnimator.SetBool("isFalling", false);
        } else {
            meshAnimator.SetBool("isFalling", true);
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
