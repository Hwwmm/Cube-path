using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

    public Color[] colors;

    Material material;


    void Start()
    {
        material = GetComponent<Renderer>().material;

        StartCoroutine(ChangeColor());
    }

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

    IEnumerator ChangeColor()
    {
        while (true)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                material.SetColor("_Color", colors[i]);
                yield return new WaitForSeconds(0.7f);
            }
        }

    }


    // Use this for initialization
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
