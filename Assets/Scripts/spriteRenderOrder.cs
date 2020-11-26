using UnityEngine;
using yaSingleton;

[CreateAssetMenu(fileName ="SpriteRenderOrderSystem", menuName ="Systems/SpriteRenderOrderSystem")]
public class spriteRenderOrder : Singleton<spriteRenderOrder>
{

   public override void OnUpdate()
    {
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();

        foreach(SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
        }
    }
}
