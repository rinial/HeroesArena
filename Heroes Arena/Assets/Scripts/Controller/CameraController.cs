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
        
        // Camera velocity.
        private Vector2 _currentVelocity = Vector2.zero;

        // Updates camera position every frame.
        private void Update()
        {
            // If Target is set, follows it.
            if (Target)
            {
                Vector3 newPos = Vector2.SmoothDamp(transform.position, Target.position, ref _currentVelocity, Damping, MaxSpeed, Time.deltaTime);
                transform.position = new Vector3(newPos.x, newPos.y, -ZDistance);
            }
            else
            {
            	// TODO behaviour with no target to follow.
            }
        }
    }
}
