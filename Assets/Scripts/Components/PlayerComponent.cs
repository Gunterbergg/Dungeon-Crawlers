using UnityEngine;

namespace DungeonCrawlers
{
    public class PlayerComponent : ObjectComponent
    {
        public int baseHealthPoints = 3;
        public int healthPoints = 3;
        public Vector2 movementDirection = Vector2.zero;
        public bool isWalking = false;

        public bool isAlive { get => healthPoints > 0; }
    }
}