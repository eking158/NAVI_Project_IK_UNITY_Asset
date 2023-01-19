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
        float shoulder_pitch =get_EulerAngles(left_target_elbow).x;
        float shoulder_roll =get_EulerAngles(left_target_elbow).z-90;
        float shoulder_yaw =get_EulerAngles(left_target_elbow).y;

        //Debug.Log(left_target_shoulder.transform.localEulerAngles);
        //Debug.Log(left_target_elbow.transform.eulerAngles);
        //Debug.Log(shoulder_roll-shoulder_pitch);
        //Debug.Log(Quaternion.Angle(left_target_shoulder.transform.rotation, left_target_elbow.transform.rotation));
        //Debug.Log("shoulder_pitch: "+shoulder_pitch+" "+"shoulder_roll: "+shoulder_roll+" "+"shoulder_yaw: "+shoulder_yaw);
        Debug.Log(get_EulerAngles(left_target_elbow));
        //get_EulerAngles(left_target_elbow);
        
        
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

    Vector3 get_EulerAngles(Transform target_object){
        float standard_1 = 0.5f;
        float standard_2 = 0.70711f;

        float x_pre = target_object.transform.eulerAngles.x;
        float y_pre = target_object.transform.eulerAngles.y;
        float z_pre = target_object.transform.eulerAngles.z;

        float quat_x = target_object.transform.rotation.x;
        float quat_y = target_object.transform.rotation.y;
        float quat_z = target_object.transform.rotation.z;
        float quat_w = target_object.transform.rotation.w;

        Debug.Log(target_object.transform.rotation);

        float x = quat_x>-standard_1 && quat_x<standard_1 ? x_pre : 180-x_pre;
        float y = quat_x>-standard_1 && quat_x<standard_1 ? y_pre : y_pre-180;
        float z = quat_x>-standard_1 && quat_x<standard_1 ? z_pre : z_pre-180;

        if(quat_x==standard_1){
            y = y_pre+90;
            z = z_pre+270;
        }
        else if(quat_x==-standard_1){
            y = y_pre-90;
            z = z_pre+90;
        }
        
        Vector3 result = new Vector3(x, y, z);

        return result;
    }
}
