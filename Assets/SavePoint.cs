using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    void Start()
    {
        print(MenuManager.instance.canSave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MenuManager.instance.canSave = true;
        MenuManager.instance.savePointPos = transform.position;
        print(MenuManager.instance.canSave);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MenuManager.instance.canSave = false;
        print(MenuManager.instance.canSave);
    }
}
