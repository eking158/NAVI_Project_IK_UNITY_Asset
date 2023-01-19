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
        Debug.Log("rx: "+rx+"  "+ "ry: " + ry + "  " + "rz: " + rz + "  ");

        float cx = Mathf.Cos((rx)*Mathf.Deg2Rad);
        float cy = Mathf.Cos((ry)*Mathf.Deg2Rad);
        float cz = Mathf.Cos((rz)*Mathf.Deg2Rad);
        float sx = Mathf.Sin((rx)*Mathf.Deg2Rad);
        float sy = Mathf.Sin((ry)*Mathf.Deg2Rad);
        float sz = Mathf.Sin((rz)*Mathf.Deg2Rad);

        float r11 = cy*cz;
        float r12 = sx*sy*cz - cx*sz;
        float r13 = cx*sy*cz + sx*sz;

        float r21 = cy*sz;
        float r22 = sx*sy*sz + cx*cz;
        float r23 = cx*sy*sz - sx*cz;

        float r31 = -sy;
        float r32 = sx*cy;
        float r33 = cx*cy;
        

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


        float theta1 = ay>0 ? Mathf.Atan2(r23, r13)*Mathf.Rad2Deg : Mathf.Atan2(-r23, -r13)*Mathf.Rad2Deg;
        float theta2 = ay>0 ? -Mathf.Atan2(Mathf.Sqrt(r13*r13 + r23*r23), r33)*Mathf.Rad2Deg : -Mathf.Atan2(-Mathf.Sqrt(r13*r13 + r23*r23), r33)*Mathf.Rad2Deg;
        //Debug.Log("theta 1: "+theta1+" "+ "theta 2: " + theta2 );
        //Debug.Log(model_theta1);


        //Debug.Log(target_end.transform.rotation);

        
        if (!float.IsNaN(theta1))
            servo_1.transform.localEulerAngles = new Vector3(0, theta1);
        if (!float.IsNaN(theta2))
            servo_2.transform.localEulerAngles = new Vector3(0, 0, theta2);
    }
}
