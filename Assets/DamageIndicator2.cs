using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator2 : MonoBehaviour
{

    RectTransform rect;
    public GameObject spider;
    public Transform player;
    public Camera cam;
    Quaternion targetRot;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame

    /*
    void Update()
    {
        Vector3 dir = player.position - spider.transform.position;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        targetRot.z = targetRot.y;
        targetRot.x = 0;
        targetRot.y = 0;

        Vector3 northDir = new Vector3(0f, 0f, player.eulerAngles.y);
        rect.localRotation = targetRot * Quaternion.Euler(northDir);
    }
    */

    private void Update()
    {
        Vector2 dir = new Vector2(player.position.x - spider.transform.position.x, player.position.z - spider.transform.position.z);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        float angleAfterConsideringCamera = angle + cam.transform.eulerAngles.y + 180f;

        rect.rotation = Quaternion.Euler(0f, 0f, angleAfterConsideringCamera);
    }

}
