using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float countdownTimer;

    private void Start()
    {
        StartCoroutine(selfDestructButton());
    }

    IEnumerator selfDestructButton()
    {
        yield return new WaitForSeconds(countdownTimer);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
