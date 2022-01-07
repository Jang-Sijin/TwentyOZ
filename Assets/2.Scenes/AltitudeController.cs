using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeController : MonoBehaviour
{
    private float _pidThrottle;
    private float _currentAltitude;
    public float verticalSpeed { get; private set; }
    public float force;
    public PID pid;
    public float integralLimit;
    public float ascendMaxSpeed; // 올라가는 속도
    public float descendMaxSpeed;
    
    public float setMaxheightY_altitude;
    
    private Rigidbody _rigidbody;
    
    public GameObject target; // 타겟
    

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        //  _AxisPidController = new PIDController(_xAxisP, _xAxisI, _xAxisD);
    }
    
    void FixedUpdate()
    {
        // verticalSpeed = gameObject.GetComponent<Rigidbody>().velocity.y;
        // _currentAltitude = gameObject.transform.position.y;
        // _pidThrottle = pid.Update(setMaxheightY_altitude, _currentAltitude, Time.fixedDeltaTime);
        //
        // if (setMaxheightY_altitude - _currentAltitude < -5) //Engage Descend Speed Limiter if altitude difference is greater than 5
        // {
        //     _pidThrottle = DescendSpeedLimiter(_pidThrottle, verticalSpeed, descendMaxSpeed);
        // }
        //
        // //Anti Integral Windup Start
        // if (verticalSpeed < -2f) //Descending
        // {
        //     pid.LimitIntegral(0);
        // }
        // pid.LimitIntegral(integralLimit); // Ascending
        // //Anti Integral Windup End
        //
        // if (verticalSpeed > ascendMaxSpeed) //Ascend Speed Limiter
        // {
        //     _pidThrottle = 0;
        // }
        //
        // _rigidbody.AddRelativeForce(Vector3.up * _pidThrottle);
        
        // // 타겟 방향으로 회전함
        // Vector3 direction = pid.Update(target.transform.position, transform.position,Time.fixedDeltaTime);
        // // 어떤 방향을 제시하면 그 방향을 본다.
        // Quaternion targetRotation = Quaternion.LookRotation(direction);
        // transform.rotation = targetRotation;
        
        Vector3 direction = pid.Update(target.transform.position, transform.position,Time.fixedDeltaTime);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = lookRotation;
    }

    float DescendSpeedLimiter(float throttle, float verticalspeed, float maxspeed)
    {
        float newthrottle;
        float mass = gameObject.GetComponent<Rigidbody>().mass;
        float weight = -1f * mass * Physics2D.gravity.y;
        if (verticalspeed < -maxspeed)
        {
            newthrottle = weight / force;
            return newthrottle;
        }
        else
            return throttle;
    }

}