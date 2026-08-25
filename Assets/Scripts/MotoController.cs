using UnityEngine;

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

    void Start()
    {
        if (bikeRigidbody == null)
        {
            Debug.Log("RigidBody wasn't assigned");
            return;
        }
    }

    private void FixedUpdate()
    {
        foreach (Transform wheelTransform in wheelPoint)
        {
            float maxLength = restLength + springTravel;

            if (Physics.Raycast(wheelTransform.position, -wheelTransform.up, out RaycastHit hit, maxLength + tireRadius, drivableLayer))
            {
                Vector3 springDir = wheelTransform.up;

                Vector3 tireWorldVel = bikeRigidbody.GetPointVelocity(wheelTransform.position);

                // Calculate offset from the raycast
                float offset = restLength - (hit.distance - tireRadius);

                // springDir is a unit vector, so this returns the magnitude of
                // tireWorldVel projected onto springDir
                float vel = Vector3.Dot(springDir, tireWorldVel);

                float force = (offset * springStiffness) - (vel * damperStiffness);

                bikeRigidbody.AddForceAtPosition(springDir * force, wheelTransform.position);

                Debug.DrawLine(wheelTransform.position, hit.point, Color.red);
            }
            else
            {
                Debug.DrawRay(wheelTransform.position, -wheelTransform.up * (tireRadius + maxLength), Color.green);
            }
        }
    }
}