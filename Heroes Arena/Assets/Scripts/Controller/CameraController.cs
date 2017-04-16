using UnityEngine;

namespace HeroesArena
{
    // Controlls camera.
    public class CameraController : MonoBehaviour
    {
        // Target to follow.
        public Transform Target;

        // The time it takes to move.
        public float Damping = 1;
        // Maximum camera speed.
        public float MaxSpeed = 100;
        // Distance from main plain. 
        public const float ZDistance = 10;
        // Offset on Y axis.
        public const float YOffset = 15;
        // Size parameters.
        public float MinSize = 30, MaxSize = 150, StartSize = 90;
        // Scale speed.
        public float ScaleSpeed = 10, ScaleAcceleration = 5;

        // Camera velocity.
        private Vector2 _currentVelocity = Vector2.zero;
        private Camera _camera;

        // Initializes camera.
        private void Start()
        {
            _camera = gameObject.GetComponent<Camera>();
            _camera.orthographicSize = StartSize;
        }

        // Updates camera position every frame.
        private void Update()
        {
            // Controlls camera size with mouse wheel.
            float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
            if (scrollAxis != 0)
            {
                float newSize = _camera.orthographicSize - (scrollAxis * ScaleSpeed * ((_camera.orthographicSize - MinSize) / (MaxSize - MinSize) * ScaleAcceleration + 1));
                newSize = newSize < MinSize ? MinSize : newSize > MaxSize ? MaxSize : newSize;

                _camera.orthographicSize = newSize;
            }

            // If Target is set, follows it.
            if (Target)
            {
                Vector3 newPos = Vector2.SmoothDamp(transform.position, new Vector2(Target.position.x, Target.position.y + YOffset * (MaxSize - _camera.orthographicSize) / (MaxSize - MinSize)), ref _currentVelocity, Damping, MaxSpeed, Time.deltaTime);
                transform.position = new Vector3(newPos.x, newPos.y, -ZDistance);
            }
            else
            {
                // TODO behaviour with no target to follow.
            }
        }
    }
}
