using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    [RequireComponent(typeof(SphereCollider))]
    public class Sensor : MonoBehaviour
    {
        [SerializeField] float _detectionRadius = 5f;
        [SerializeField] float _timerInterval = 1f;

        private SphereCollider _detectionRange;

        public event Action OnTargetChanged = delegate { };

        public Vector3 TargetPosition => _target? _target.transform.position : Vector3.zero;
        public bool IsTargetInRange => TargetPosition != Vector3.zero;

        private GameObject _target;
        private Vector3 _lastKnownPosition;
        private CountdownTimer _timer;

        private void Awake()
        {
            _detectionRange = GetComponent<SphereCollider>();
            _detectionRange.isTrigger = true;
            _detectionRange.radius= _detectionRadius;
        }
        private void Start()
        {
            _timer = new CountdownTimer(_timerInterval);
            _timer.OnTimerStop += () =>
            {
                UpdateTargetPosition(_target.OrNull());
                _timer.Start();
            };
            _timer.Start();
        }
        private void Update()
        {
            _timer.Tick(Time.deltaTime);
        }
        private void UpdateTargetPosition(GameObject target=null)
        {
            _target = target;
            if (IsTargetInRange && (_lastKnownPosition!=TargetPosition || _lastKnownPosition != Vector3.zero))
            {
                _lastKnownPosition = TargetPosition;
                OnTargetChanged.Invoke();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            UpdateTargetPosition(other.gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            UpdateTargetPosition();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = IsTargetInRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}
