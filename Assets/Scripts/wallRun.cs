using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallRun : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    [SerializeField] Transform body;
    public weaponManager weapon;

    public characterScript character;

    public float maxWallDistance = 1f;
    public float minHeight = 1f;
    float wallRunGravity;
    public float wallRunJumpForce;
    public bool isWallRunning = false;
    public float baseWallRunGravity = -1f;

    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    //Camera
    public Camera cam;
    [SerializeField] float fov;
    [SerializeField] float wallRunFov;
    [SerializeField] float wallRunFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    int wallLayer = 10;

    public float tilt { get; private set; }

    bool canWallRun()
    {
        return !character.isGrounded;
    }

    void checkWall()
    {
        if(Physics.Raycast(transform.position, -body.right, out leftWallHit, maxWallDistance))
        {
            if (leftWallHit.transform.gameObject.layer == wallLayer)
            {
                wallLeft = true;
            }
            else
            {
                wallLeft = false;
            }
        }
        else
        {
            wallLeft = false;
        }
        if(Physics.Raycast(transform.position, body.right, out rightWallHit, maxWallDistance))
        {
            if (rightWallHit.transform.gameObject.layer == wallLayer)
            {
                wallRight = true;
            }
            else
            {
                wallRight = false;
            }
        }
        else
        {
            wallRight = false;
        }
    }

    private void Start()
    {
        wallRunGravity = baseWallRunGravity;
    }
    void Update()
    {
        anim = weapon.anim;

        checkWall();

        if (canWallRun())
        {
            if (wallLeft)
            {
                startWallRun();
            }
            else if (wallRight)
            {
                startWallRun();
            }
            else
            {
                stopWallRun();
            }
        }
        else
        {
            stopWallRun();
        }
    }

    void startWallRun()
    {
        isWallRunning = true;
        rb.useGravity = false;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        wallRunGravity += Time.deltaTime * 3f;

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }
        else
        {
            tilt = 0;
        }

       

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("jumping");
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }

    void stopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
        wallRunGravity = baseWallRunGravity;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
