using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("CuttableObject"))
            Destroy(collision.gameObject);
    }
}
