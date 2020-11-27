using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MenuManager.Instance.canSave = true;
        MenuManager.Instance.savePointPos = transform.position;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MenuManager.Instance.canSave = false;
        print(MenuManager.Instance.canSave);
    }
}
