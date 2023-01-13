/*
-특이점 및 예외 처리 : limit_angle.cs와 ik_calculate_ver3에서 처리
-ik 해석 : ik_calculate_ver3에서 처리
-아바타의 관절 구조 따라하기 : ik_imitate_test.cs로 처리
-NAVI_model (get position) 각 관절 위치 생성 : Change_Position.cs로 처리

-ybot: 메인 아바타 -> 사람의 동작을 그대로 따라함 (신체 비율 맞추는 용도)
-NAVI_model: 메인 아바타의 각 관절로부터 각도를 받아와서 따로 구현 (로봇 모델과 아바타를 이어주는 과정)
-NAVI_model (get position) : NAVI_model의 관절 위치를 그대로 구현함 (상속 관계가 존재하지 않음)
-NAVI_model(robot)(pitch->roll->yaw) : 실제 로봇의 3d 모델. -> NAVI_model의 각 관절 위치를 통해 역기구학을 계산함
*/

/* 역기구학 해석 순서
<case 1>
-shoulder에서 본 elbow 좌표를 통해 shoulder_pitch, shoulder_roll 해석

<case 2>
-shoulder_pitch와 shoulder_roll은 고정된 상태에서 shoulder에서 본 wrist 좌표를 통해 shoulder_yaw, elbow_pitch 해석
-상대 좌표 : shoulder_yaw와 elbow_pitch만 영향 받음
-> [ax2, ay2, az2] : shoulder에서 본 wrist 좌표 (좌표축: base)
-> [bx, by, bz] = R_p(theta1) * [ax, ay, az] : shoulder에서 본 wrist 좌표 (좌표축: base에서 pich로 theta1만큼 회전)
-> [cx, cy, cz] = R_r(theta2) * [bx, by, bz] : shoulder에서 본 wrist 좌표 (좌표축: base에서 pich로 theta1만큼 회전 -> roll로 theta2만큼 회전)

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ik_calculate_ver3 : MonoBehaviour
{
    public Transform left_target_hand, left_target_wrist, left_target_elbow, left_target_shoulder;
    public Transform left_target_elbow_pitch;
    public Transform left_shoulder_pitch, left_shoulder_roll, left_shoulder_yaw;
    public Transform left_elbow_pitch, left_elbow_yaw;
    public Transform left_wrist_pitch, left_wrist_roll;
    public Transform left_vr;

    //public TextMeshProUGUI angle;

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
        float L1=(float)Vector3.Distance(left_target_shoulder.transform.position,left_target_elbow.transform.position);  //어깨 ~ 팔꿈치 길이
        float L2=(float)Vector3.Distance(left_target_elbow.transform.position,left_target_wrist.transform.position);  //팔꿈치 ~ 손목 길이
        float L3=(float)Vector3.Distance(left_target_wrist.transform.position,left_target_hand.transform.position);  //손목 ~ 손 길이
        //Debug.Log("L1: "+L1+" "+"L2: "+L2);

        
        float ax_pre=(int)((left_target_elbow.transform.position.x-left_target_shoulder.position.x)*10000);
        float ay_pre=(int)((left_target_elbow.transform.position.y-left_target_shoulder.position.y)*10000);
        float az_pre=(int)((left_target_elbow.transform.position.z-left_target_shoulder.position.z)*10000);  //어깨를 기준으로 본 팔꿈치 좌표 (소수점 변환 준비)
        float ax=ax_pre/10000;
        float ay=ay_pre/10000;
        float az=az_pre/10000;  //어깨를 기준으로 본 팔꿈치 좌표 (소수점 변환 완료)
        //Debug.Log("ax: "+ax+" "+"ay: "+ay+" "+"az: "+az);
        

        float ay2=(float)left_target_wrist.transform.position.x-left_target_shoulder.position.x;
        float az2=(float)left_target_wrist.transform.position.y-left_target_shoulder.position.y;
        float ax2=(float)left_target_wrist.transform.position.z-left_target_shoulder.position.z;  //어깨를 기준으로 본 손목 좌표
        //Debug.Log("ax2: "+ax2+" "+"ay2: "+ay2+" "+"az2: "+az2);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        float c2 = -ax/L1;
        float s2 = ax > 0 ? Mathf.Sqrt(1-c2*c2) : -Mathf.Sqrt(1-c2*c2);
        float c1 = -ay/(L1*s2);
        float s1 = az/(L1*s2);
        */
        float s2 = ax/L1;
        float c2 = ay > 0 ? Mathf.Sqrt(1-s2*s2) : -Mathf.Sqrt(1-s2*s2);
        float c1 = -ay/(L1*c2);
        float s1 = az/(L1*c2);
        

        //theta 1 ~ 4까지 계산
        //float object_theta1 = (ay>0) ? (left_target_shoulder.transform.eulerAngles.z-360) : (-left_target_shoulder.transform.eulerAngles.z+180);
        float object_theta1 = (left_target_shoulder.transform.eulerAngles.z);
        if(object_theta1>180 && object_theta1<=360) object_theta1 = object_theta1-360;
        //float calcu_theta1 = 90-object_theta1;

        float calcu_theta1 = Mathf.Atan2(s1, c1)*Mathf.Rad2Deg;
        float calcu_theta2 = Mathf.Atan2(s2, c2)*Mathf.Rad2Deg;
        Debug.Log("calcu_theta1: "+calcu_theta1+" "+"c2: "+c2+" "+"s2: "+s2+" "+"c1: "+c1+" "+"s1: "+s1+" "+"L1: "+L1);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        float c1_c = Mathf.Cos((-calcu_theta1)*Mathf.Deg2Rad);
        float c2_c = Mathf.Cos((calcu_theta2-90)*Mathf.Deg2Rad);
        float s1_c = Mathf.Sin((-calcu_theta1)*Mathf.Deg2Rad);
        float s2_c = Mathf.Sin((calcu_theta2-90)*Mathf.Deg2Rad);

        float bx = c1_c*ax2 - az2*s1_c;
        float by = ay2;
        float bz = c1_c*az2 + ax2*s1_c;
        //Debug.Log("bx: "+bx+" "+"by: "+by+" "+"bz: "+bz);

        float cx = bx;
        float cy = c2_c*by + bz*s2_c;
        float cz = c2_c*bz - by*s2_c;
        //Debug.Log("cx: "+cx+" "+"cy: "+cy+" "+"cz: "+cz);

        float get_theta4 = left_target_elbow_pitch.transform.localEulerAngles.y;
        float s4 = (L1+cz)/L2;
        float c4 = (L1+cz) < 0 ? Mathf.Sqrt(1-s4*s4) : -Mathf.Sqrt(1-s4*s4);
        float c3 = -cy/(L2*c4);
        float s3 = -cx/(L2*c4);
        //Debug.Log("c4: "+c4+" "+"s4: "+s4);
        
        float calcu_theta3 = Mathf.Atan2(s3, c3)*Mathf.Rad2Deg;
        float calcu_theta4 = Mathf.Atan2(s4, c4)*Mathf.Rad2Deg;
        //Debug.Log("calcu_theta 3: "+calcu_theta3+" "+"calcu_theta 4: "+calcu_theta4);


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        float calcu_theta5 =left_vr.transform.localEulerAngles.y;
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
         //계산된 theta를 로봇 3d 모델에 맞춰주기
        float theta1 = -(calcu_theta1+180);
        float theta2 = -(calcu_theta2+180);
        float theta3 = -(calcu_theta3-90);
        float theta4 = -(calcu_theta4+90);
        float theta5 = (calcu_theta5);
        //float theta2 = constrain(calcu_theta2-90, -90, 90);
        //float theta3 = constrain(-(calcu_theta3+90),-89,89);
        //float theta4 = constrain(-calcu_theta4-90,-179,-5);

        //특이점 처리 구간////////////////////////////////////////////////////////////////////////////////////////////////////////
        float theta2_sig = 3;  //theta2 특이점 방지 오차 각도
        float theta4_sig = -2;  //theta4 특이점 방지 오차 각도

        //theta2 각도가 -90일때 발생하는 theta1에 대한 특이점 방지
        /*
        if(theta2>=-90-theta2_sig && theta2<-90) theta2 = -90-theta2_sig;
        else if(theta2>-90 && theta2<-90+theta2_sig) theta2 = -90+theta2_sig;
        */

        //theta4가 반대로 굽히는 상황에서는 theta3가 180도 돌아감
        if(theta4 > theta4_sig){
            theta4 = theta4_sig;
            theta3 = 0;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //안전을 위한 예외 처리 구간////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        해당 코드 잠정 폐쇄 -> 처음부터 아바타의 이동 각도를 제한해주는 script 새로 생성
        if(cx<0){ //팔꿈치가 뒤로 걲이는 경우
            if(cy>0){
                theta3=89;
            }
            else if(cy<0){
                theta3=-89;
            }
            //theta4=-5;
        }
        if(theta2==-90){ //팔을 옆으로 쫙 핀 상태에서는 앞으로 회전 불가능
            theta1=0;
        }
        if(ay>0){  //팔을 옆으로 쫙 핀 상태에서 더 위로 움직이는 경우
            theta1 = 0;
            theta3 = 0;
            //theta2=theta2+180;
        }
        */

        //Debug.Log("theta 1: "+theta1+"  "+"theta 2: "+theta2+"  "+"theta3: "+theta3+"  "+"theta4: "+theta4);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        if (!float.IsNaN(theta1)){
            left_shoulder_pitch.transform.localEulerAngles = new Vector3(theta1, 0, 0);
            }
        if (!float.IsNaN(theta2)){
            left_shoulder_roll.transform.localEulerAngles = new Vector3(0, 0, theta2);
            }
        if (!float.IsNaN(theta3)){
            left_shoulder_yaw.transform.localEulerAngles = new Vector3(0, theta3, 0);
            }


        if (!float.IsNaN(theta4)){
            left_elbow_pitch.transform.localEulerAngles = new Vector3(theta4, 0, 0);
            }
            /*
        if (!float.IsNaN(theta5)){
            left_elbow_yaw.transform.localEulerAngles = new Vector3(0, theta5, 0);
            }


        if (!float.IsNaN(theta6)){
            left_wrist_pitch.transform.localEulerAngles = new Vector3(theta6, 0, 0);
            }
        if (!float.IsNaN(theta7)){
            left_wrist_roll.transform.localEulerAngles = new Vector3(0, 0, theta7);
            }
            */

            //각 theta값 화면에 출력
            //angle.text = "theta 1: "+(int)theta1+" "+"theta 2: "+(int)theta2+" "+"theta 3: "+(int)theta3+"\n"+"theta 4: "+(int)theta4+" "+"theta 5: "+(int)theta5+"\n"+"theta 6: "+(int)theta6+" "+"theta 7: "+(int)theta7;
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
matlab 코드 (dh 확인)

<case (1)>
L(1) = Link([0 0 0 -pi/2 0], 'standard');
L(2) = Link([0 0 0 -pi/2 0], 'standard');
L(3) = Link([0 1 0 0 0], 'standard');
std3link = SerialLink(L, 'name', 'three link')
std3link.fkine([0 0 0], 'deg')
figure(1)
std3link.plot([0 0 0])
std3link.teach




*/


/*
matlab 코드 (방정식)

<case (1)>
syms theta1 theta2 theta3
syms l1
syms c1 c2 c3
syms s1 s2 s3

T_12=kinematics_T(theta1, 0, 0, -90)
T_23=kinematics_T(theta2, 0, 0, -90)
T_34=kinematics_T(theta3, l1, 0, 0)

T_12=[c1, 0, -s1, 0;s1, 0, c1, 0;0, -1, 0, 0;0, 0, 0, 1]
T_23=[c2, 0, -s2, 0;s2, 0, c2, 0;0, -1, 0, 0;0, 0, 0, 1]
T_34=[c3, -s3, 0, 0;s3, c3, 0, 0;0, 0, 1, -l1;0, 0, 0, 1]

T_14=T_12*T_23*T_34


<case (2)>
syms theta1 theta2 theta3 theta4
syms l1 l2
syms c1 c2 c3 c4
syms s1 s2 s3 s4

T_12=kinematics_T(theta1, 0, 0, -90)
T_23=kinematics_T(theta2, 0, 0, -90)
T_34=kinematics_T(theta3, l1, 0, -90)
T_46=kinematics_T(theta4, 0, l2, 0)

T_12=[c1, 0, -s1, 0;s1, 0, c1, 0;0, -1, 0, 0;0, 0, 0, 1]
T_23=[c2, 0, -s2, 0;s2, 0, c2, 0;0, -1, 0, 0;0, 0, 0, 1]
T_34=[c3, 0, -s3, 0;s3, 0, c3, 0;0, -1, 0, l1;0, 0, 0, 1]
T_46=[c4, -s4, 0, l2*c4;s4, c4, 0, l2*s4;0, 0, 1, 0;0 0 0 1]

T_13=T_12*T_23
T_36=T_34*T_46

T_16=T_12*T_23*T_34*T_46
*/



/*
float s4 = Mathf.Sin(get_theta4*Mathf.Deg2Rad);
float c4 = Mathf.Cos(get_theta4*Mathf.Deg2Rad);
float c3 = (ay2-L2*c2*s4+L1*c2)/(-L2*s2*c4);
float s3 = Mathf.Sqrt(1-c3*c3);
*/