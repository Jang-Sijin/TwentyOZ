using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// https://ssabi.tistory.com/24
    [RequireComponent(typeof (Rigidbody))]
    public class TestController : MonoBehaviour
    {
        // 타겟의 Transform
        [SerializeField]
        protected Transform m_target = null;

        // pid 조절
        [System.Serializable]
        public class Gain
        {
            public float p = 1.0f;

            public float i = 0.0f;

            public float d = 1.0f;
        }

        // 인스펙터 위치 조절
        [SerializeField]
        protected Gain m_posGain = new Gain();
        
        // 인스펙터 방향 조절
        [SerializeField]
        protected Gain m_rotGain = new Gain();

        // 중력 적용
        [SerializeField]
        protected bool m_gravityCompensation = true;

        // 오브젝트 rigidbody 저장
        private Rigidbody _rigidbody;

        // 오브젝트 transform 저장
        private Transform _transform;

        // Position(위치) error
        private Vector3 error_ = Vector3.zero;
        private Vector3 prevError_ = Vector3.zero;
        private Vector3 diffError_ = Vector3.zero;
        private Vector3 intError_ = Vector3.zero;

        // Rotation(각도) error
        private Quaternion rotError_ = Quaternion.identity;
        private Quaternion prevRotError_ = Quaternion.identity;
        private Quaternion diffRotError_ = Quaternion.identity;
        private Quaternion intRotError_ = Quaternion.identity;

        private float angleError_ = 0f;
        private Vector3 errorAxis_;
        private Vector3 diffErrorAxis_;
        private Vector3 intErrorAxis_;

        // private float prevAngleError_ = 0f;
        private float diffAngleError_ = 0f;
        private float intAngleError_ = 0f;

        private Vector3 force_ = Vector3.zero;
            
        // target transform
        public Transform target
        {
            get
            {
                return m_target;
            }
        }
        
        // Position gains
        public Gain positionGains
        {
            get
            {
                return m_posGain;
            }
        }
        
        // Rotation gains
        public Gain rotationGains
        {
            get
            {
                return m_rotGain;
            }
        }
        
        // rotation axis
        Vector3 RotAxis(Quaternion q)
        {
            var n = new Vector3(q.x, q.y, q.z);
            return n.normalized;
        }
        
        protected Quaternion RotOptimize(Quaternion q)
        {
            if (q.w < 0.0f)
            {
                q.x *= -1.0f;
                q.y *= -1.0f;
                q.z *= -1.0f;
                q.w *= -1.0f;

                Debug.Log($"x:{q.x}, y:{q.y}, z:{q.z}, w:{q.w}");
            }
            Debug.Log($"x:{q.x}, y:{q.y}, z:{q.z}, w:{q.w}");

            return q;
        }
        
        public Vector3 posError
        {
            get
            {
                return error_;
            }
        }

        public void SetTarget(Transform target)
        {
            m_target = target;
        }

        public void SetPositionGains(float p, float i, float d)
        {
            m_posGain.p = p;
            m_posGain.i = i;
            m_posGain.d = d;
        }

        public void SetRotationGains(float p, float i, float d)
        {
            m_rotGain.p = p;
            m_rotGain.i = i;
            m_rotGain.d = d;
        }

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
        }

        void FixedUpdate()
        {
            if (m_target == null)
            {
                return;
            }

            /// position ///
            error_ = m_target.position - _transform.position;
            intError_ += error_ * Time.deltaTime; // integral
            diffError_ = (error_ - prevError_) / Time.deltaTime;
            prevError_ = error_;

            force_ =
                m_posGain.p * error_ +
                m_posGain.i * intError_ +
                m_posGain.d * diffError_;

            if (m_gravityCompensation && _rigidbody.useGravity)
            {
                force_ += -_rigidbody.mass * Physics.gravity;
            }

            /// rotation ///
            rotError_ = RotOptimize(m_target.rotation * Quaternion.Inverse(_transform.rotation));
            // RotOptimize

             diffRotError_ = rotError_ * Quaternion.Inverse(prevRotError_);
            intRotError_ = intRotError_ * rotError_;
            rotError_.ToAngleAxis(out angleError_, out errorAxis_);

             diffRotError_.ToAngleAxis(out diffAngleError_, out diffErrorAxis_);
             intRotError_.ToAngleAxis(out intAngleError_, out intErrorAxis_);
             var trq = errorAxis_ * (m_rotGain.p*angleError_) +diffErrorAxis_*(m_rotGain.i*diffAngleError_) + intErrorAxis_* (m_rotGain.d*intAngleError_);
            angleError_ *= Mathf.Deg2Rad; // deg to rad
            var angVel_ = m_rotGain.p * angleError_ * errorAxis_;

            if (error_.sqrMagnitude > 0.1f)
            {
                _rigidbody.AddForce (force_);
            }
            else if (m_gravityCompensation && _rigidbody.useGravity)
            {
                _rigidbody.AddForce(-_rigidbody.mass * Physics.gravity);
            }

            if (angleError_ * angleError_ > 0.01f)
            {
                _rigidbody.angularVelocity = angVel_;
            }
            prevRotError_ = rotError_;
            
            transform.LookAt(m_target);
        }

        // [Button]
        public void TeleportToTarget()
        {
            if (!_transform)
            {
                _transform = transform;
            }
            if (m_target)
            {
                _transform.SetPositionAndRotation(m_target.position, m_target.rotation);
            }
        }
    }