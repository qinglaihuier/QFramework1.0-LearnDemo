using System.Collections;
using System.Collections.Generic;
using LikeSoulKnight;
using UnityEngine;

public class CameraController : AbstractMonoController
{
    private Transform mPlayer;
    private void Start()
    {
        mPlayer = GameObject.FindWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        // Vector3 cameraPos = transform.position;

        // Vector3 targetPos = mPlayer.position + new Vector3(3 * Mathf.Sign(mPlayer.localScale.x), 2, 0);
        // targetPos.z = cameraPos.z;

        // cameraPos = Vector3.MoveTowards(cameraPos, targetPos, 7f * Time.deltaTime);
        // transform.position = cameraPos;
        Vector3 playerPos = mPlayer.position;
        playerPos.z = transform.position.z;
        transform.position = playerPos;
    }
}
