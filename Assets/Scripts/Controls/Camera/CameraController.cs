using UnityEngine;

namespace CyberEvolution.Controls.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraController : MonoBehaviour
    {
        private UnityEngine.Camera _camera;
        
        private Vector2 _direction;
        private Vector2 _originPosition;
        private Vector3 _velocity;
        private Vector3 _targetVelocity;
        private Vector3 _smoothedVelocity;
        private float _maxOrthographicSize = 20;
        private readonly float _maxDistanceOffset = 10;

        private readonly float _speed = 50;
        private readonly float _smoothTime = .2f;
        private readonly float _scrollStep = .75f;

        public void SetPosition(Vector2 position)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
            _originPosition = position;
        }

        public void SetOrthographicSize(float size)
        {
            _camera.orthographicSize = size;
            _maxOrthographicSize = size * 2;
        }

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void Update()
        {
            //todo: update logic when game states are implemented
            MoveCamera();
            ZoomCamera();
        }

        private void SubscribeToEvents()
        {
            InputService.Instance.OnJump += ResetPosition;
        }

        private void UnsubscribeFromEvents()
        {
            InputService.Instance.OnJump -= ResetPosition;
        }

        private void ResetPosition()
        {
            _velocity = Vector3.zero;
            transform.position = new Vector3(_originPosition.x, _originPosition.y, transform.position.z);
        }

        private void MoveCamera()
        {
            _direction = InputService.Instance.Direction;
            _targetVelocity = _direction * _speed;
            _velocity = Vector3.SmoothDamp(_velocity, _targetVelocity, ref _smoothedVelocity, _smoothTime);
            Vector3 targetPosition = transform.position + _velocity * Time.deltaTime;
            Vector3 clampedPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, _originPosition.x - _maxDistanceOffset,
                    _originPosition.x + _maxDistanceOffset),
                Mathf.Clamp(targetPosition.y, _originPosition.y - _maxDistanceOffset,
                    _originPosition.y + _maxDistanceOffset), transform.position.z);
            transform.position = clampedPosition;
        }

        private void ZoomCamera()
        {
            _camera.orthographicSize =
                Mathf.Clamp(_camera.orthographicSize - InputService.Instance.Scroll.y * _scrollStep,
                    1, _maxOrthographicSize);
        }
    }
}