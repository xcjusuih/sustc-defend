using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlayer : BaseRobot
{
    static RobotPlayer instance;
    public static RobotPlayer GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    public List<WeaponBase> Weapons;
    public Transform Hand;
    public float WalkSpeed = 10;

    private WeaponBase CurWeapon;
    private int CurWeaponIdx = 0;

    private Animator animator;
    private CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var trans = Camera.main.transform;
        var forward = Vector3.ProjectOnPlane(trans.forward, Vector3.up);
        var right = Vector3.ProjectOnPlane(trans.right, Vector3.up);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var moveDirection = v * forward + h * right;
        cc.Move(moveDirection.normalized * WalkSpeed * Time.deltaTime);
        animator.SetFloat("Speed", cc. velocity.magnitude / WalkSpeed);
        //rotate
        var r = GetAimPoint();
        RotateToTarget(r);
        //fire
        if(Input.GetButton("Fire1"))
        {
            Shoot();
        }

        //ui
        HUD.GetInstance().UpdateHpUI(hp);

    }

    public Vector3 GetAimPoint()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay,out floorHit, 100.0f, LayerMask.GetMask("Floor")))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0;
            return playerToMouse;
        }
        return Vector3.zero;
    }

    public void RotateToTarget(Vector3 rot)
    {
        transform.LookAt(rot + transform.position);
    }

    public void Shoot(){
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Idle"))
        {
            animator.SetBool("Shoot",true);
        }
    }

    public override void OpenFire()
    {
        base.OpenFire();
        //
        Debug.Log("OpenFire");
    }
}
