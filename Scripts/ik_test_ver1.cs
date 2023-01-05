using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ik_test_ver1 : MonoBehaviour
{
    public Transform left_target_hand, left_target_wrist, left_target_elbow, left_target_shoulder;
    public Transform left_shoulder_pitch, left_shoulder_roll, left_shoulder_yaw;
    public Transform left_elbow_pitch, left_elbow_yaw;
    public Transform left_wrist_pitch, left_wrist_roll;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        좌표축 변환
        unity   real
        x       y
        y       z
        z       x

        */

        /*
        유니티의 오일러 각 회전 순서: z -> x -> y
        이를 실제 축으로 환산하면 x -> y -> z 가 된다.

        */

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //case 1
        /*
        px = L1*s2
        py = -L1*c1*c2
        pz = L1*s1*c2
        */
        float L1=(float)Vector3.Distance(left_target_shoulder.transform.position,left_target_elbow.transform.position);  //어깨 ~ 팔꿈치 길이

        float ax_pre=(int)((left_target_elbow.transform.position.x-left_target_shoulder.position.x)*10000);
        float ay_pre=(int)((left_target_elbow.transform.position.y-left_target_shoulder.position.y)*10000);
        float az_pre=(int)((left_target_elbow.transform.position.z-left_target_shoulder.position.z)*10000);  //어깨를 기준으로 본 팔꿈치 좌표 (소수점 변환 준비)
        float ax=ax_pre/10000;
        float ay=ay_pre/10000;
        float az=az_pre/10000;  //어깨를 기준으로 본 팔꿈치 좌표 (소수점 변환 완료)
        //Debug.Log("ax: "+ax+" "+"ay: "+ay+" "+"az: "+az);
        /*
        float s2 = ax/L1;
        float c2 = Mathf.Sqrt(ay*ay+az*az)/L1;

        float c1 = -ay/(L1*c2);
        float s1 = az/(L1*c2);

        float calcu_theta1 = Mathf.Atan2(s1, c1)*Mathf.Rad2Deg;
        float calcu_theta2 = Mathf.Atan2(s2, c2)*Mathf.Rad2Deg;
        */
        float s2 = ax/L1;
        float c2 = ax >= 0 ? Mathf.Sqrt(1-s2*s2) : -Mathf.Sqrt(1-s2*s2);
        //Debug.Log("c2: "+c2+" "+"s2: "+s2);
        float c1 = ay/(L1*c2);
        float s1 = az/(L1*c2);
        Debug.Log("c1: "+c1+" "+"s1: "+s1);

        float calcu_theta1 = Mathf.Atan2(s1, c1)*Mathf.Rad2Deg;
        //float calcu_theta1 = Mathf.Atan2(az, -ay)*Mathf.Rad2Deg;
        float calcu_theta2 = Mathf.Atan2(s2, c2)*Mathf.Rad2Deg;
        //Debug.Log("calcu_theta1: "+calcu_theta1+" "+"calcu_theta2: "+calcu_theta2);
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
         //계산된 theta를 로봇 3d 모델에 맞춰주기
        float theta1 = -(calcu_theta1-180);
        float theta2 = -(calcu_theta2+180);
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //특이점 예외 처리
        //옆으로 팔 핀 상태에서 발생하는 특이점(+-5도)
        if(theta2>=-95 && theta2<-90) theta2 = -95;
        else if(theta2>=-90 && theta2 < -85) theta2 = -85;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //최종적으로 계산된 theta만큼 오브젝토 각도 변경
        if (!float.IsNaN(theta1)){
            left_shoulder_pitch.transform.localEulerAngles = new Vector3(theta1, 0, 0);
            }
        if (!float.IsNaN(theta2)){
            left_shoulder_roll.transform.localEulerAngles = new Vector3(0, 0, theta2);
            }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        Debug.Log("theta 1: "+theta1+"  "
        +"theta 2: "+theta2+"  "
        );
        */
    }

    float constrain(float x, float min, float max)
    {
        if(x<=min){
            return min;
        }
        else if(x>=max){
            return max;
        }
        else{
            return x;
        }
    }
}


        /*
        +"theta3: "+theta3+"  "
        +"theta4: "+theta4
        */


/* case 1
syms theta1 theta2
syms l1
syms c1 c2
syms s1 s2

T_B1=kinematics_T(-90, 0, 0, 90)
T_12=kinematics_T(theta1, 0, 0, -90)
T_24=kinematics_T(theta2, 0, l1, 0)

T_12=[c1,0,-s1,0;s1,0,c1,0;0,-1,0,0;0,0,0,1]
T_24=[c2,-s2,0,l1*c2;s2,c2,0,l1*s2;0,0,1,0;0,0,0,1]

T_B4=T_B1*T_12*T_24
*/
