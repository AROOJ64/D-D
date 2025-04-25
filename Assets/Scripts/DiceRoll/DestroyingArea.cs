using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.CompareTag("DestroyingArea"))
        {
            Destroy(gameObject);
        }
    }
}
