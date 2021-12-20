using UnityEngine;

public class CuttableObject : MonoBehaviour
{
    public float fallOffForce = 1f;
    private Rigidbody rb;
    private bool falling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // went too far from table
        if(transform.position.x < -1f)
        {
            if (!falling)
            {
                FallOff();
            }
        }
    }

    private void FallOff()
    {
        falling = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        Vector3 forceVector = Vector3.left * fallOffForce;
        rb.AddForceAtPosition(forceVector, transform.position, ForceMode.Impulse);
    }
}
