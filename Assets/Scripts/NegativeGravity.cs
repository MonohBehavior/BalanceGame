using UnityEngine;

namespace BalanceGame
{
    public class NegativeGravity : MonoBehaviour
    {
        [Tooltip("mulitiplier for the angular velocity for the torque to apply.")]
        public float Friction = 0.4f;

        [SerializeField] private CharacterJoint _joint;
        [SerializeField] private Rigidbody _stage;
        [SerializeField] private Rigidbody _cube;

        private void FixedUpdate()
        {
            var slowDownFactor = 0.9f;
            var vel = _stage.velocity * slowDownFactor;
            _stage.velocity = vel;
            _stage.angularVelocity *= slowDownFactor;
        }
    }
}