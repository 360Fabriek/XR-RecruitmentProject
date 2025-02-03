using UnityEngine;

public class Ball : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Get the normal of the surface we hit
            Vector3 normal = collision.contacts[0].normal;
            // Reflect the velocity based on the normal
            GetComponent<Rigidbody>().linearVelocity = Vector3.Reflect(GetComponent<Rigidbody>().linearVelocity, normal);
        }
    }
}
