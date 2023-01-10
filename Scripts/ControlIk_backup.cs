using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlIk_backup : MonoBehaviour
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
        //Debug.Log("px: "+px+" "+ "py: " + py + " " + "pz: " + pz + " ");

        float L1=(float)Vector3.Distance(target_joint_1.transform.position,target_joint_2.transform.position);  //d1
        float L2=(float)Vector3.Distance(target_joint_2.transform.position,target_joint_3.transform.position);  //a1
        float L3=(float)Vector3.Distance(target_joint_3.transform.position,target_end.transform.position);  //a2
        //Debug.Log("L1: "+L1+" "+ "L2: " + L2 + " " + "L3: " + L3 + " ");

        float theta1=Mathf.Atan2(pz,-px)*Mathf.Rad2Deg;
        //Debug.Log("s2: "+s2+" "+ "c2: " + c2+"theta2: "+theta2);
        //Debug.Log("theta 1: "+theta1+" "+ "theta 2: " + theta2 );

        float ax=(target_end.transform.position.x-target_joint_3.position.x);
        float ay=(target_end.transform.position.y-target_joint_3.position.y);
        float az=(target_end.transform.position.z-target_joint_3.position.z);  //어깨를 기준으로 본 팔꿈치 좌표
        //Debug.Log("ax: "+ax+" "+ "ay: " + ay + " " + "az: " + az + " ");
        float s1_c = Mathf.Sin((theta1)*Mathf.Deg2Rad);
        float c1_c = Mathf.Cos((theta1)*Mathf.Deg2Rad);

        float bx_pre = (int)((c1_c*ax - az*s1_c)*10000);
        float by_pre = (int)(ay*10000);
        float bz_pre = (int)((c1_c*az + ax*s1_c)*10000);
        float bx=bx_pre/10000;
        float by=by_pre/10000;
        float bz=bz_pre/10000;
        Debug.Log("bx: "+bx+" "+"by: "+by+" "+"bz: "+bz);

        float s2=(py-L1)/L3;
        //float c2 = Mathf.Sqrt(1-s2*s2);
        float c2 = (bx> 0) ? Mathf.Sqrt(1-s2*s2) : -Mathf.Sqrt(1-s2*s2);

        float theta2=Mathf.Atan2(s2,c2)*Mathf.Rad2Deg;

        if (!float.IsNaN(theta1))
            servo_1.transform.localEulerAngles = new Vector3(0, theta1,0);
        if (!float.IsNaN(theta2))
            servo_2.transform.localEulerAngles = new Vector3(0, 0, theta2+180);
    }
}
