using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [Min(0)]
    public float delay;

    void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    private IEnumerator DestroyAfter()
	{
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
	}
}