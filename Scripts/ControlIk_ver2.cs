using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlIk_ver2 : MonoBehaviour
{
    public Transform target_joint_1, target_joint_2, target_joint_3, target_end;
    public Transform servo_1, servo_2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        좌표축 변환
        unity   real  calcu
        x       y     x
        y       z     z
        z       x     y

        */

        /*
        유니티의 오일러 각 회전 순서: z -> x -> y
        이를 실제 축으로 환산하면 x -> y -> z 가 된다.

        */
        float px=(float)target_end.transform.position.x;
        float py=(float)target_end.transform.position.z;
        float pz=(float)target_end.transform.position.y;
        //Debug.Log("px: "+px+" "+ "py: " + py + " " + "pz: " + pz + " ");

        float model_x = target_end.transform.rotation.x;
        //if(model_x>-0.70711 && model_x<0.70711)
        float rx = 0;
        float ry = model_x>-0.70711 && model_x<0.70711 ? -target_end.transform.eulerAngles.z : -target_end.transform.eulerAngles.z-180; 
        ry = target_end.transform.eulerAngles.z==0 ? 0.001f : ry;
        float rz = model_x>-0.70711 && model_x<0.70711 ? target_end.transform.eulerAngles.y : target_end.transform.eulerAngles.y-180;
        //Debug.Log("rx: "+rx+"  "+ "ry: " + ry + "  " + "rz: " + rz + "  ");

        float cx = Mathf.Cos((rx)*Mathf.Deg2Rad);
        float cy = Mathf.Cos((ry)*Mathf.Deg2Rad);
        float cz = Mathf.Cos((rz)*Mathf.Deg2Rad);
        float sx = Mathf.Sin((rx)*Mathf.Deg2Rad);
        float sy = Mathf.Sin((ry)*Mathf.Deg2Rad);
        float sz = Mathf.Sin((rz)*Mathf.Deg2Rad);

        float quat_x = target_end.transform.rotation.x;
        float quat_y = target_end.transform.rotation.y;
        float quat_z = target_end.transform.rotation.z;
        float quat_w = target_end.transform.rotation.w;
        Debug.Log(target_end.transform.rotation);

        float r11 = 1-(2*quat_y*quat_y)-(2*quat_z*quat_z);
        float r12 = (2*quat_x*quat_y)-(2*quat_w*quat_z);
        float r13 = (2*quat_x*quat_z)+(2*quat_w*quat_y);

        float r21 = (2*quat_x*quat_y)+(2*quat_w*quat_z);
        float r22 = 1-(2*quat_x*quat_x)-(2*quat_z*quat_z);
        float r23 = (2*quat_y*quat_z)-(2*quat_w*quat_x);

        float r31 = (2*quat_x*quat_z)-(2*quat_w*quat_y);
        float r32 = (2*quat_y*quat_z)+(2*quat_w*quat_x);
        float r33 = 1-(2*quat_x*quat_x)-(2*quat_y*quat_y);

        /*
        float r11 = cy*cz;
        float r12 = sx*sy*cz - cx*sz;
        float r13 = cx*sy*cz + sx*sz;

        float r21 = cy*sz;
        float r22 = sx*sy*sz + cx*cz;
        float r23 = cx*sy*sz - sx*cz;

        float r31 = -sy;
        float r32 = sx*cy;
        float r33 = cx*cy;
        */
        

        float L1=(float)Vector3.Distance(target_joint_1.transform.position,target_joint_2.transform.position);  //d1
        float L2=(float)Vector3.Distance(target_joint_2.transform.position,target_joint_3.transform.position);  //a1
        float L3=(float)Vector3.Distance(target_joint_3.transform.position,target_end.transform.position);  //a2
        //Debug.Log("L1: "+L1+" "+ "L2: " + L2 + " " + "L3: " + L3 + " ");

        float ax=(target_end.transform.position.x-target_joint_3.position.x);
        float ay=(target_end.transform.position.y-target_joint_3.position.y);
        float az=(target_end.transform.position.z-target_joint_3.position.z);  //어깨를 기준으로 본 팔꿈치 좌표
        //Debug.Log("ax: "+ax+" "+ "ay: " + ay + " " + "az: " + az + " ");




        float s2=(py-L1)/L3;
        float c2 = (ax> 0) ? Mathf.Sqrt(1-s2*s2) : -Mathf.Sqrt(1-s2*s2);
        float c1 = px/(L2+L3*c2);
        float s1 = pz/(L2+L3*c2);
        //Debug.Log("s2: "+s2+" "+ "c2: " + c2);


        float theta1 = ay>0 ? Mathf.Atan2(-r33, r13)*Mathf.Rad2Deg : Mathf.Atan2(r33, -r13)*Mathf.Rad2Deg;
        float theta2 = ay>0 ? -Mathf.Atan2(Mathf.Sqrt(r13*r13 + r33*r33), r23)*Mathf.Rad2Deg : -Mathf.Atan2(-Mathf.Sqrt(r13*r13 + r33*r33), r23)*Mathf.Rad2Deg; 
        //Debug.Log("theta 1: "+theta1+" "+ "theta 2: " + theta2 );
        //Debug.Log(model_theta1);


        //Debug.Log(target_end.transform.rotation);

        
        if (!float.IsNaN(theta1))
            servo_1.transform.localEulerAngles = new Vector3(0, theta1);
        if (!float.IsNaN(theta2))
            servo_2.transform.localEulerAngles = new Vector3(0, 0, theta2);
    }
}


/*
syms theta1 theta2 theta3
syms L1 L2 L3
syms c1 c2 c3
syms s1 s2 s3

T_WB = kinematics_T(0, 0, 0, -90);
T_B1 = kinematics_T(0, L1, 0, 0);
T_12 = kinematics_T(theta1, 0, 0, -90);
T_23 = kinematics_T(theta2, 0, 0, 90);
T_3E = kinematics_T(theta3, L2, 0, 0);

T_12 = [c1, 0, -s1, 0;s1, 0, c1, 0;0, -1, 0, 0;0, 0, 0, 1];
T_23 = [c2, 0, s2, 0;s2, 0, -c2, 0;0, 1, 0, 0;0, 0, 0, 1];
T_3E = [c3, -s3, 0, 0;s3, c3, 0, 0;0, 0, 1, L2;0,0,0,1];

T_WE = T_WB*T_B1*T_12*T_23*T_3E
-----------------------------------------------------------------------
T_WE =
[  c1*c2*c3 - s1*s3, - c3*s1 - c1*c2*s3,  c1*s2,   L2*c1*s2]
[            -c3*s2,              s2*s3,     c2, L1 + L2*c2]
[- c1*s3 - c2*c3*s1,   c2*s1*s3 - c1*c3, -s1*s2,  -L2*s1*s2]
[                 0,                  0,      0,          1]
-----------------------------------------------------------------------
*/
