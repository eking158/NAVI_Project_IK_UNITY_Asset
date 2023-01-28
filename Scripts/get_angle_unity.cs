using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class get_angle_unity : MonoBehaviour
{
    public Transform target_a, target_b;
    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log(target_a.transform.localEulerAngles);
        //Debug.Log(target_a.transform.eulerAngles);
        //Debug.Log(target_a.rotation.eulerAngles);
        //Debug.Log(target_a.transform.localEulerAngles);
        Debug.Log(target_a.transform.rotation);
    }
}
