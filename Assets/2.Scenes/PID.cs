using UnityEngine;

[System.Serializable]
public class PID
{
    public float pFactor, iFactor, dFactor;

    private float _integral;
    private float _lastError;
    
    private Vector3 _integralVector;
    private Vector3 _lastErrorVector;
    
    public PID(float pFactor, float iFactor, float dFactor)
    {
        this.pFactor = pFactor;
        this.iFactor = iFactor;
        this.dFactor = dFactor;
    }
    
    public float Update(float target, float current, float deltatime)
    {
        float error = target - current;
        _integral += error * deltatime;
        float derivative = (error - _lastError) / deltatime;
        _lastError = error;
        return error * pFactor + _integral * iFactor + derivative * dFactor;
    }
    
    public Vector3 Update(Vector3 target, Vector3 current, float deltatime)
    {
        Vector3 error = target - current;
        _integralVector += error * deltatime;
        Vector3 derivativeVector = (error - _lastErrorVector) / deltatime;
        _lastErrorVector = error;
        return error * pFactor + _integralVector * iFactor + derivativeVector * dFactor;
    }
    
    public void LimitIntegral(float value) // 최대, 최소 값 고정
    {
        if (_integral >= value)
        {
            _integral = value;
        }
        if (_integral <= -value)
        {
            _integral = -value;
        }
    }
}