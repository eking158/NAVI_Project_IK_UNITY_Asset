using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlIk : MonoBehaviour
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
        float px=(float)target_end.transform.position.x;
        float py=(float)target_end.transform.position.y;
        float pz=(float)target_end.transform.position.z;

        float px2=(float)target_joint_3.transform.position.x;
        float py2=(float)target_joint_3.transform.position.y;
        float pz2=(float)target_joint_3.transform.position.z;
        //Debug.Log("px: "+px+" "+ "py: " + py + " " + "pz: " + pz + " ");

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
        float model_theta1 = servo_1.transform.localEulerAngles.y-180;
        float theta2 = model_theta1>90&&model_theta1<180 || model_theta1>-1800&&model_theta1<=-90 ? Mathf.Atan2(s2,c2)*Mathf.Rad2Deg : Mathf.Atan2(s2,c2)*Mathf.Rad2Deg;
        //float theta1 = Mathf.Atan2(pz,px)*Mathf.Rad2Deg;
        float theta1 = theta2>90&&theta2<180 || theta2>-1800&&theta2<=-90 ? Mathf.Atan2(pz,-px)*Mathf.Rad2Deg : Mathf.Atan2(pz,-px)*Mathf.Rad2Deg-180;
        //float theta1 = theta2>90&&theta2<=180 || theta2>-1800&&theta2<=-90 ? Mathf.Atan2(s1,c1)*Mathf.Rad2Deg : Mathf.Atan2(s1,c1)*Mathf.Rad2Deg-180;
        Debug.Log("theta 1: "+theta1+" "+ "theta 2: " + theta2 );
        //Debug.Log(model_theta1);

        
        if (!float.IsNaN(theta1))
            servo_1.transform.localEulerAngles = new Vector3(0, theta1,0);
        if (!float.IsNaN(theta2))
            servo_2.transform.localEulerAngles = new Vector3(0, 0, theta2+180);
    }
}
