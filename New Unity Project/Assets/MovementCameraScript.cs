using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementCameraScript : MonoBehaviour
{
    void AllowJump()
    {
        CanJump = true;
    }
    public float handOffset = -6.4f;
    public float handOffsetZ = 2.4f;
    public float handOffsetX = 0.7f;
    public Transform hands;
    public Transform Camera;
    public Transform CharacterCenter;
    public float CameraOffset = 8;
    public float MovementSpeed = 4;
    public float sensitivity = 1;
    public float MaxAngle = 30;
    public float xRn=0, yRn =0;
    private CharacterController Cc;
    public float Gravity = -0.3f;
    public float vGravity = 0;
    public float DelayBeforeJump = 1;
    public bool CanJump = true;

    public bool onGround = false;

    public float JumpHeight = 15;
        // Start is called before the first frame update\
    void Start()
    {
        Cc = CharacterCenter.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
     void Update ()
     {
         float rotateHorizontal = Input.GetAxis ("Mouse Y")*sensitivity;
         float rotateVertical = Input.GetAxis ("Mouse X")*sensitivity;
         yRn -= rotateHorizontal;
         xRn = (xRn + rotateVertical)%360;
         vGravity += Gravity*Time.deltaTime;
         yRn =Mathf.Clamp(yRn,-80,80);
         CharacterCenter.transform.localEulerAngles= new Vector3(0,xRn,0);
         Vector3 movementDirection =CharacterCenter.forward*Input.GetAxisRaw("Horizontal")-CharacterCenter.right*Input.GetAxisRaw("Vertical");
         movementDirection = movementDirection.normalized;
         if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0)){
            Cc.Move(movementDirection*MovementSpeed*Time.deltaTime);
         }
         Cc.enabled = false;
         if((Cc.isGrounded)& vGravity < 0){
            vGravity = 0;
            onGround= true ;
         }
         else{
             onGround=false;
         }
         if (((Input.GetButtonDown("Jump")) && CanJump) && (Physics.CheckSphere(CharacterCenter.forward*Cc.center.z + CharacterCenter.right*Cc.center.x + new Vector3(0,-Cc.height/2 + Cc.center.y+Cc.radius*0.7f-Cc.stepOffset,0)+CharacterCenter.position,Cc.radius*0.9f)||Cc.isGrounded)){
              vGravity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
              CanJump = false;
              Invoke("AllowJump",DelayBeforeJump);
              print("Jumping");
         }
         Cc.enabled = true;
         Cc.Move(new Vector3(0,vGravity,0)*Time.deltaTime);
         Camera.position = CharacterCenter.position + new Vector3(0,CameraOffset,0)+ CharacterCenter.forward*Cc.center.z+CharacterCenter.right*Cc.center.x;
         Camera.localEulerAngles= new Vector3(yRn,xRn-90,0);
         hands.position = Camera.position+ Camera.up*handOffset + Camera.forward*handOffsetZ + Camera.right*handOffsetX ;
         hands.localEulerAngles= new Vector3(0,xRn,yRn);
         
     }
     void FixedUpdate(){
         Cc.enabled = false;
         if(Physics.CheckSphere(CharacterCenter.forward*Cc.center.z + CharacterCenter.right*Cc.center.x + new Vector3(0,Cc.height/2 + Cc.center.y+Cc.radius*0.05f,0)+CharacterCenter.position,Cc.radius*0.9f)){
             vGravity = -Mathf.Abs(vGravity);
             //print("Gettng touched");
         }
         Cc.enabled = true;
     }
}