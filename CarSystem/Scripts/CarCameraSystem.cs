using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraSystem : MonoBehaviour
{
    
    public Transform player;
    public float timeOfSpeed = 1.3f;

    void Update()
    {
        transform.position = player.position;

        if(transform.rotation.y != player.rotation.y)
        {
            transform.rotation = new Quaternion(0,Mathf.Lerp(transform.rotation.y,player.rotation.y, timeOfSpeed * Time.deltaTime),0,Mathf.Lerp(transform.rotation.w,player.rotation.w, timeOfSpeed * Time.deltaTime));
        }
    }
}
