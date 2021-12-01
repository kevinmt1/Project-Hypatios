using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{

    public wallRun wallRunScript;
    float mouseSensitivity = 200f;
    public Transform playerBody;
    float xRot = 0f;
    public float x, y;
    public float maxPointingDistance = 1.5f;
    public LayerMask weaponMask;
    GameObject raycastedObject;
    public float throwingForce = 1f;

    Camera cam;

    //Camera Shake
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot -= y;
        xRot = Mathf.Clamp(xRot, -85f, 85f);
        transform.localRotation = Quaternion.Euler(xRot, 0f, wallRunScript.tilt);

        playerBody.Rotate(Vector3.up * x);
    }
}