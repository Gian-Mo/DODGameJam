
using UnityEngine;
using UnityEngine.InputSystem;
public class playerController : MonoBehaviour
{
   [SerializeField] CharacterController controller;
    [SerializeField] int speed;    
    [SerializeField] int jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] int shootDistance;

    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference shoot; //Select
   public Vector3 moveDirection;
   public Vector3 playerVel;
    bool ableToShoot;
    bool ableToGravity;
    bool selected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    
    void Update()
    {

        Movement();
        ableToGravity = true;

    }

    void Movement()
    {

      ControllerMovement();

      
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
      
        playerVel.y = jumpSpeed;

    }
   

    void CanSelect()
    {
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance))
        {

            Debug.Log(hit.collider.name);
        }
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
        ableToShoot = true;
    }
    void ShootFalse(InputAction.CallbackContext context)
    {
        ableToShoot = false;
     
    }

}
