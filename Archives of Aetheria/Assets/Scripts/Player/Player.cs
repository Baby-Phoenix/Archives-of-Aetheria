using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float jumpHorizontalSpeed;
    [SerializeField] private float jumpTime;
    [SerializeField] private float rechargeTime;
    [SerializeField] private Transform camTransform;
    public StatusBarManager healthBar;
    public StatusBarManager staminaBar;
    public GameObject Sword1;
    public GameObject Sword2;

    [SerializeField] AnimationCurve dodgeCurve;
    [SerializeField] private float dodgeSpeed = 1;
    private bool isDodging;
    private float dodgeTimer;
    [SerializeField] private float dodgeRate = 1;
    private float nextDodge;

    private Animator animator;
    private CharacterController characterController;

    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpStartTime;
    private bool isJumping;
    private bool isGrounded;
    private bool isAttacking;
    private Vector3 velocity;
    private Vector3 moveDir;

    int comboCount;

    // Start is called before the first frame update
    void Start()
    {
       // FindObjectOfType<AudioManager>().Play("BGM");
        Sword2.SetActive(false);

        rechargeTime = 0;
        StatusBarManager[] status = GameObject.Find("Player HUD").GetComponentsInChildren<StatusBarManager>();
        Debug.Log(status.Length);
        foreach(var i in status)
        {
            if (i.gameObject.tag == "HealthBar")
            {
                healthBar = i;
                healthBar.SetMaxValue(10f);
            }

            else if (i.gameObject.tag == "StaminaBar")
            {
                staminaBar = i;
                staminaBar.SetMaxValue(100f);
            }
        }
      
       
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;

        Keyframe dodgeLastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodgeLastFrame.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDodging && !isAttacking) PlayerMovement();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isAttacking && Time.time > nextDodge)
        {
            if (moveDir.magnitude != 0 && !isJumping)
            {
                nextDodge = Time.time + dodgeRate;

                StartCoroutine(Dodge());
            }
            rechargeTime = 0;
        }

        if (staminaBar.unit >= 5) 
            Attack();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isJumping && !isDodging)
        {
            AttackCombo();
        }
        if (staminaBar.unit >= 30)
        {
            if (Input.GetMouseButtonDown(1) && !isJumping && !isDodging && !isAttacking)
            {
                isAttacking = true;

                animator.SetTrigger("Slash Ability");
            }
            if (staminaBar.unit >= 99)
            {
                if (Input.GetMouseButtonDown(2) && !isJumping && !isDodging && !isAttacking)
                {
                    isAttacking = true;

                    animator.SetTrigger("Special Ability");
                }
            }
        }
    }
    
    private void AttackDone()
    {
        isAttacking = false;
    }

    void AttackCombo()
    {
        if(comboCount == 0)
        {
            isAttacking = true;
            
            animator.SetBool("isCombo1", true);

            animator.SetBool("isCombo2", false);
            animator.SetBool("isCombo3", false);

            animator.SetBool("isComboDone", false);

            comboCount = 1;
            return;
        }
        if (comboCount != 0)
        {
            if(animator.GetBool("isComboPossible"))
            {
                isAttacking = true;

                animator.SetBool("isComboPossible", false);
                comboCount++;
            }
        }
    }

    public void ComboPossible()
    {
        animator.SetBool("isComboPossible", true);
    }

    public void Combo()
    {
        if (comboCount == 2)
        {
            animator.SetBool("isCombo2", true);

            animator.SetBool("isCombo1", false);
            animator.SetBool("isCombo3", false);

            animator.SetBool("isComboDone", false);
        }

        else if (comboCount == 3)
        {
            animator.SetBool("isCombo3", true);

            animator.SetBool("isCombo1", false);
            animator.SetBool("isCombo2", false);

            animator.SetBool("isComboDone", false);
        }
    }

    public void ComboReset()
    {
        isAttacking = false;

        animator.SetBool("isCombo1", false);
        animator.SetBool("isCombo2", false);
        animator.SetBool("isCombo3", false);
        animator.SetBool("isComboPossible", false);

        animator.SetBool("isComboDone", true);

        comboCount = 0;
    }
    
    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        rechargeTime += Time.deltaTime;

        moveDir = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(moveDir.magnitude);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            inputMagnitude /= 2;
        }

        animator.SetFloat("Input Magnitude", inputMagnitude, 0.08f, Time.deltaTime);

        moveDir = Quaternion.AngleAxis(camTransform.rotation.eulerAngles.y, Vector3.up) * moveDir;
        moveDir.Normalize();

        float gravity = Physics.gravity.y * gravityMultiplier;
        ySpeed += gravity * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpStartTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpTime)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);


            if (Time.time - jumpStartTime <= jumpTime)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                animator.SetBool("isJumping", true);
                isJumping = true;
                jumpStartTime = null;
                lastGroundedTime = null;

                characterController.height = 2;
            }
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("isGrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -10.5)
            {
                animator.SetBool("isFalling", true);
                characterController.height = 2.9f;
            }
        }

        if (moveDir != Vector3.zero)
        {
            animator.SetBool("isMoving", true);

            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (!isGrounded)
        {
            velocity = moveDir * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }

        if (rechargeTime >= .5f)
        {
            rechargeTime = 0;
            StaminaManager(1);
        }
       
    }

    IEnumerator Dodge()
    {
        animator.SetTrigger("Dodge");
        isDodging = true;
        float timer = 0;
        
        while(timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 dir = (transform.forward * speed) + (Vector3.up * velocity.y * 0.05f);
            characterController.Move(dir * dodgeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
            
        }
        isDodging = false;
    }

    private void OnAnimatorMove()
    {
        if (isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
    }

    //Lock cursor from moving
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void SetCharacterControllerHeight(float height)
    {
        characterController.height = height;
    }

    private void SetCharacterControllerPosY(float yPos)
    {
        characterController.center = new Vector3(characterController.center.x, yPos, characterController.center.z);
    }

    private void StaminaManager(float value)
    {
           staminaBar.UpdateValue(value);  
    }

    public void ChangeSword()
    {
        Sword2.SetActive(true);
        Sword1.SetActive(false);
    }
}
