using UnityEngine;

public class MotoController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] wheelPoint;
    [SerializeField] private Rigidbody bikeRigidbody;
    [SerializeField] private LayerMask drivableLayer;


    [Header("Constants")]
    [SerializeField] private float springDamping;

    void Start()
    {
        if (bikeRigidbody == null)
        {
            //BikeRigidbody wasn't assigned
            Debug.Log("RigidBody wasn't assigned");
            return;
        }

    }

    private void FixedUpdate()
    {

        foreach (Transform wheelTransform in wheelPoint)
        {
            if (Physics.Raycast(wheelTransform.position, -Vector3.up, out RaycastHit hit))
            {
                Debug.Log("Found an object - distance: " + hit.distance);
                Debug.Log(wheelTransform.name + " : " + wheelTransform.position.ToString());
            }
        }
    }
}
