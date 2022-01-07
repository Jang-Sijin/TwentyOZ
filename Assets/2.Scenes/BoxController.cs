using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//     비례항 : 현재 상태에서의 오차값의 크기에 비례한 제어작용을 한다.
//     적분항 : 정상상태(steady-state) 오차를 없애는 작용을 한다.
//     미분항 : 출력값의 급격한 변화에 제동을 걸어 오버슛(overshoot)을 줄이고 안정성(stability)을 향상시킨다.

public class BoxController : MonoBehaviour
{
    public Vector3 ObjectPosition;
    public Vector3 targetPosition;
    public float addForceY; // integral
    public float integral;

    private float _lastError;

    // [SerializeField] [Range(-10, 10)] private float _xAxisP, _xAxisI, _xAxisD;
    
    private Rigidbody _rigidbody;
    // private PIDController _AxisPidController;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        //  _AxisPidController = new PIDController(_xAxisP, _xAxisI, _xAxisD);
    }
    
    // Rigidbody를 다루는 경우에, Update대신 FixedUpdate를 사용해야 합니다. 예를 들어 리지드바디에 힘을 가하는 경우에,
    // FixedUpdate안에서 매 고정된 프레임마다 힘을 적용해야 합니다. 이 경우 Upate에서 매 프레임마다 힘을 적용하지 않도록 합니다.
    void FixedUpdate()
    {
        // 오브젝트 위치 갱신
        ObjectPosition = _rigidbody.position;
        
        Vector3 distancePosition_error = targetPosition - _rigidbody.position;
        print($"{distancePosition_error}");

        // integral += distancePosition_error.y * Time.deltaTime;
        // float derivative = (distancePosition_error.y - _lastError) / Time.deltaTime;
        // _lastError = distancePosition_error.y;

        _rigidbody.AddRelativeForce(Vector3.up * addForceY * distancePosition_error.y);
    }
}