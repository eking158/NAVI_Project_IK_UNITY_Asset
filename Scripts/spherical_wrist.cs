using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spherical_wrist : MonoBehaviour
{
    public Transform target_joint_1, target_joint_2, target_joint_3, target_joint_4, target_end;
    public Transform servo_1, servo_2, servo_3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float L1=(float)Vector3.Distance(target_joint_1.transform.position,target_joint_2.transform.position);  // = 높이
        float L2=(float)Vector3.Distance(target_joint_4.transform.position,target_end.transform.position);  // ~ end effector
        //Debug.Log("L1: "+L1+" "+ "L2: " + L2);

        float px_pre=(int)((-target_end.transform.position.x)*10000);
        float pz_pre=(int)((target_end.transform.position.y-L1)*10000);
        float py_pre=(int)((target_end.transform.position.z)*10000);
        float px = px_pre/10000;
        float py = py_pre/10000;
        float pz = pz_pre/10000;
        //Debug.Log("px: "+px+" "+ "py: " + py + " " + "pz: " + pz + " ");

        float c2 = pz/L2;
        //float c2 = Mathf.Sqrt(1-s2*s2);
        float s2 = Mathf.Sqrt(1-c2*c2);
        //Debug.Log("s2: "+s2+" "+ "c2: " + c2);

        float c1 = px/(s2*L2);
        float s1 = py/(s2*L2);

        float theta2 = (pz)> 0 ? Mathf.Atan2(s2,c2)*Mathf.Rad2Deg : Mathf.Atan2(-s2,c2)*Mathf.Rad2Deg;
        //float theta2 = (pz)> 0 ? (px > 0 ? Mathf.Atan2(s2,c2)*Mathf.Rad2Deg : Mathf.Atan2(s2,-c2)*Mathf.Rad2Deg) : (px > 0 ? Mathf.Atan2(-s2,-c2)*Mathf.Rad2Deg : Mathf.Atan2(-s2,c2)*Mathf.Rad2Deg);
        float theta1 = s2 > 0 ? Mathf.Atan2(s1*s2, c1*s2)*Mathf.Rad2Deg : Mathf.Atan2(-s1*s2, -c1*s2)*Mathf.Rad2Deg;
        //float theta1 = s2 > 0 ? Mathf.Atan2(pz,-px)*Mathf.Rad2Deg;
        Debug.Log("theta 1: "+theta1+" "+ "theta 2: " + theta2 );



        //
        /*
        if (!float.IsNaN(theta1))
            servo_1.transform.localEulerAngles = new Vector3(0, theta1,0);
            */
        if (!float.IsNaN(theta2))
            servo_2.transform.localEulerAngles = new Vector3(0, 0, theta2-90);
    }
}



/*
<spherical wrist matlab code(normal)>
syms theta1 theta2 theta3 L1 L2
syms c1 c2 c3 s1 s2 s3

T_B1 = kinematics_T(0, L1, 0, 0);
T_12 = kinematics_T(theta1, 0, 0, -90);
T_23 = kinematics_T(theta2, 0, 0, 90);
T_3E = kinematics_T(theta3, L2, 0, 0);

T_12 = [c1, 0, -s1, 0;s1, 0, c1, 0;0, -1, 0, 0;0,0,0,1];
T_23 = [c2, 0, s2, 0;s2, 0, -c2, 0;0, 1, 0, 0;0, 0, 0, 1];
T_3E = [c3, -s3, 0, 0;s3, c3, 0, 0;0, 0, 1, L2;0, 0, 0, 1];

T_BE = T_B1 * T_12 * T_23 * T_3E

T_BE =
[c1*c2*c3 - s1*s3, - c3*s1 - c1*c2*s3, c1*s2,   L2*c1*s2]
[c1*s3 + c2*c3*s1,   c1*c3 - c2*s1*s3, s1*s2,   L2*s1*s2]
[          -c3*s2,              s2*s3,    c2, L1 + L2*c2]
[               0,                  0,     0,          1]
*/