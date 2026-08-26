using UnityEngine;
using UnityEngine.InputSystem;

public class MotoController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] wheelPoint;
    [SerializeField] private Rigidbody bikeRigidbody;
    [SerializeField] private LayerMask drivableLayer;

    [Header("Constants")]
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float tireRadius;

    [Header("Acceleration")]
    [SerializeField] private float driveForce;
    private float inputForce;

    void Start()
    {
        if (bikeRigidbody == null)
        {
            Debug.Log("RigidBody wasn't assigned");
            return;
        }
    }

    private void Update()
    {
        if (Keyboard.current.wKey.isPressed) inputForce = 1;
        else if (Keyboard.current.sKey.isPressed) inputForce = -1;
        else inputForce = 0;

    }

    private void FixedUpdate()
    {
        for (int i = 0; i < wheelPoint.Length; i++)
        {
            float maxLength = restLength + springTravel;

            if (Physics.Raycast(wheelPoint[i].position, -wheelPoint[i].up, out RaycastHit hit, maxLength + tireRadius, drivableLayer))
            {
                Vector3 springDir = wheelPoint[i].up;

                Vector3 tireWorldVel = bikeRigidbody.GetPointVelocity(wheelPoint[i].position);

                // Calculate offset from the raycast
                float offset = restLength - (hit.distance - tireRadius);

                // springDir is a unit vector, so this returns the magnitude of
                // tireWorldVel projected onto springDir
                float vel = Vector3.Dot(springDir, tireWorldVel);

                float force = (offset * springStiffness) - (vel * damperStiffness);

                bikeRigidbody.AddForceAtPosition(springDir * force, wheelPoint[i].position);
                // REAR WHEEL
                if (i == 1)
                {
                    bikeRigidbody.AddForceAtPosition(wheelPoint[i].forward * driveForce * inputForce, wheelPoint[i].position);
                }

                Debug.DrawLine(wheelPoint[i].position, hit.point, Color.red);
            }
            else
            {
                Debug.DrawRay(wheelPoint[i].position, -wheelPoint[i].up * (tireRadius + maxLength), Color.green);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (bikeRigidbody == null) return;

        Vector3 com = bikeRigidbody.worldCenterOfMass;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(com, 0.05f);

        // Optional: crosshair lines so it's visible at any zoom
        Gizmos.DrawLine(com - transform.right * 0.3f, com + transform.right * 0.3f);
        Gizmos.DrawLine(com - transform.up * 0.3f, com + transform.up * 0.3f);
        Gizmos.DrawLine(com - transform.forward * 0.3f, com + transform.forward * 0.3f);
    }
}