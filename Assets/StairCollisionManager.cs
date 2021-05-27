using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCollisionManager : MonoBehaviour
{
    public GameObject Area;
    public GameObject TileLayer4;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            Area.GetComponent<EdgeCollider2D>().enabled = false;
            TileLayer4.GetComponent<EdgeCollider2D>().enabled = true;
            TileLayer4.transform.position = new Vector3(TileLayer4.transform.position.x,TileLayer4.transform.position.y, 0.2f);
        }
    }
}
