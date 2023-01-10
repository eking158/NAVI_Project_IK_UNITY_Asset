using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imitate : MonoBehaviour
{
    public Transform left_target_wrist, left_target_elbow, left_target_shoulder;
    public Transform left_shoulder_up, left_shoulder_down;
    public Transform left_elbow;
    public Transform left_wrist;
    void Start()
    {
        
    }

    void Update()
    {
        float shoulder_pitch =left_target_shoulder.transform.localEulerAngles.x;
        float shoulder_roll =left_target_shoulder.transform.localEulerAngles.z;
        float shoulder_yaw =left_target_shoulder.transform.localEulerAngles.y;

        float elbow_pitch =left_target_elbow.transform.localEulerAngles.x;
        float elbow_yaw =left_target_elbow.transform.localEulerAngles.z;

        float wrist_pitch =left_target_wrist.transform.localEulerAngles.x;
        float wrist_roll =left_target_wrist.transform.localEulerAngles.z;

        //Debug.Log(left_target_shoulder.transform.localEulerAngles);
        //Debug.Log(shoulder_roll-shoulder_pitch);
        //Debug.Log(Quaternion.Angle(left_target_shoulder.transform.rotation, left_target_elbow.transform.rotation));
        Debug.Log("shoulder_pitch: "+shoulder_pitch+" "+"shoulder_roll: "+shoulder_roll+" "+"shoulder_yaw: "+shoulder_yaw);
        
        if (!float.IsNaN(shoulder_pitch)){
            left_shoulder_up.transform.localEulerAngles = new Vector3(shoulder_pitch, 0, 90);
        }
        if (!float.IsNaN(shoulder_roll)){
            left_shoulder_down.transform.localEulerAngles = new Vector3(0, 0, shoulder_roll);
        }
        if (!float.IsNaN(shoulder_yaw)){
            left_shoulder_down.transform.localEulerAngles = new Vector3(0, shoulder_yaw, 0);
        }

        if (!float.IsNaN(elbow_pitch)){
            left_elbow.transform.localEulerAngles = new Vector3(elbow_pitch, 0,  0);
        }
        if (!float.IsNaN(elbow_yaw)){
            left_elbow.transform.localEulerAngles = new Vector3(0, elbow_yaw,  0);
        }

        if (!float.IsNaN(wrist_pitch)){
            left_wrist.transform.localEulerAngles = new Vector3(wrist_pitch, 0,  0);
        }
        if (!float.IsNaN(wrist_roll)){
            left_wrist.transform.localEulerAngles = new Vector3(0, 0,  wrist_roll);
        }
        
    }
}
