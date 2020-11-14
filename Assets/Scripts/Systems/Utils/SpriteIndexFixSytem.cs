using UnityEngine;

namespace DungeonCrawlers.Systems 
{
    public class SpriteIndexFixSytem : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) Destroy(this);
        }

        void Update() => spriteRenderer.sortingOrder = (int)(transform.position.y * -100f);
    }
}