
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class playerController : MonoBehaviour
{
   [SerializeField] CharacterController controller;
    [SerializeField] int speed;    
    [SerializeField] int jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] int shootDistance;
    [SerializeField] LayerMask selectables;
    [SerializeField] float selectingDur;

    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference shoot; //Select
   public Vector3 moveDirection;
   public Vector3 playerVel;
    bool selecting;
    bool ableToGravity;
    bool selected;

    float selectingTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        ableToGravity = true;
    }

    // Update is called once per frame
    
    void Update()
    {

        Movement();

    }

    void Movement()
    {

        ControllerMovement();
        if (CanSelect())
        {

            if (selecting)
            {
                selectingTimer += Time.deltaTime;

                if (selectingTimer >= selectingDur)
                {
                    Debug.Log("It Works");

                }

            }

        }
    }
 
 void ControllerMovement()
  {
      if (ableToGravity)
      {
          if (!controller.isGrounded)
          {
              playerVel.y -= gravity * Time.deltaTime;
          }
          else
          {
              playerVel.y = 0;
          }
      }

      Vector3 inputDirection = move.action.ReadValue<Vector3>().normalized * speed * Time.deltaTime;

      moveDirection = transform.TransformDirection(inputDirection);

      controller.Move(moveDirection);
      controller.Move(playerVel * Time.deltaTime);



 }

    void Jump(InputAction.CallbackContext context)
    {

        StartCoroutine(turnOffGravity());
        playerVel.y = jumpSpeed;

    }
   

    bool CanSelect()
    {
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance,selectables))
        {
            Debug.Log(hit.collider.name);
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        EnableShoot(true);
        jump.action.started += Jump;
    }
    private void OnDisable()
    {
        EnableShoot(false);
        jump.action.started -= Jump;
    }

    private void EnableShoot(bool enable)
    {
        if (enable)
        {
            shoot.action.started += ShootTrue;
            shoot.action.performed += ShootTrue;
            shoot.action.canceled += ShootFalse;
        }
        else
        {
            shoot.action.started -= ShootTrue;
            shoot.action.performed -= ShootTrue;
            shoot.action.canceled -= ShootFalse;
        }
    }
    void ShootTrue(InputAction.CallbackContext context)
    {
        selecting = true;
    }
    void ShootFalse(InputAction.CallbackContext context)
    {
        selecting = false;
        selectingTimer = 0;
     
    }


    IEnumerator turnOffGravity()
    {
        ableToGravity = false;
      yield return new WaitForSeconds(0.01f);
        ableToGravity = true;
    }
}
