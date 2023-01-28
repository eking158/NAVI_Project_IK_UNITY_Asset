using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_3Joint_Test : MonoBehaviour
{
    public Transform left_target_neck, left_target_head;  //목 관절, 머리
    public Transform left_head_pitch, left_head_roll, left_head_yaw;
    void Start()
    {
        
    }

    void Update()
    {
        float L1=(float)Vector3.Distance(left_target_neck.transform.position,left_target_head.transform.position);  //머리 ~ 목까지 거리

        float ax=(float)left_target_head.transform.position.x;
        float ay=(float)left_target_head.transform.position.y;
        float az=(float)left_target_head.transform.position.z;

        float head_pitch =-left_target_neck.transform.localEulerAngles.x;
        float head_roll =left_target_neck.transform.localEulerAngles.z;
        float head_yaw =left_target_neck.transform.localEulerAngles.y;
        
        if (!float.IsNaN(head_pitch)){
            left_head_pitch.transform.localEulerAngles = new Vector3(0, head_pitch, 0);
        }
        if (!float.IsNaN(head_roll)){
            left_head_roll.transform.localEulerAngles = new Vector3(0, head_roll, 0);
        }
        if (!float.IsNaN(head_yaw)){
            left_head_yaw.transform.localEulerAngles = new Vector3(0, head_yaw, 0);
        }
    }
}
