using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class spriteRenderOrder : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 500;
    [SerializeField]
    public int offset = 0;
    [SerializeField]
    private bool runOnlyOnce = true;

    public bool isPlayer = false;
    private float timer;
    private float timerMax = .5f;
    private Renderer myRenderer;

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }
    private void LateUpdate()
    {
        if (isPlayer == true)
        {
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        }
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }
    }
}
