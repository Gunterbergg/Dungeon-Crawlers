using UnityEngine;

namespace DungeonCrawlers.Systems
{
    public class DestroySystem : MonoBehaviour
    {
        public void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}