using UnityEngine;
using System.Collections;

public class BonusJumpController : MonoBehaviour {

    void Update()
    {
        Ray rayDown = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (!Physics.Raycast(rayDown, out hit, 2f))
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = Vector3.down * 15f;
        }
    }


    // Use this for initialization
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
