using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject savePoint;
    void Start()
    {
        savePoint = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MenuManager.instance.canSave = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MenuManager.instance.canSave = false;
    }
}
