using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spherical_joint_test : MonoBehaviour
{
    public Transform left_target_hand, left_target_wrist, left_target_elbow, left_target_shoulder;
    public Transform left_shoulder_pitch, left_shoulder_roll, left_shoulder_yaw;
    public Transform left_elbow_pitch, left_elbow_yaw;
    public Transform left_wrist_pitch, left_wrist_roll;
    void Start()
    {
        
    }

    void Update()
    {
        float shoulder_pitch =left_target_shoulder.transform.localEulerAngles.x;
        float shoulder_roll =left_target_shoulder.transform.localEulerAngles.z;
        float shoulder_yaw =left_target_shoulder.transform.localEulerAngles.y;
        
        if (!float.IsNaN(shoulder_pitch)){
            left_shoulder_pitch.transform.localEulerAngles = new Vector3(shoulder_pitch, 0, 0);
        }
        if (!float.IsNaN(shoulder_roll)){
            left_shoulder_roll.transform.localEulerAngles = new Vector3(0, 0, shoulder_roll);
        }
        if (!float.IsNaN(shoulder_yaw)){
            left_shoulder_yaw.transform.localEulerAngles = new Vector3(0, shoulder_yaw, 0);
        }
        
    }
}
