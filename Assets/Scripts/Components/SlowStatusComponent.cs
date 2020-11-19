using System;

namespace DungeonCrawlers
{
    public class SlowStatusComponent : StatusComponent {
        public float duration = 1f;
        public float speedModifier = 0.5f;

        public override Status Status { get => new SlowStatus(duration, speedModifier); }
    }

    [Serializable]
    public class SlowStatus : Status
    {
        public float speedModifier;

        public SlowStatus(float duration, float speedModifier)
        {
            this.duration = duration;
            this.speedModifier = speedModifier;
        }

        public override Status Accumulate(Status other)
        {
            SlowStatus otherSlowStatus = other as SlowStatus;
            if (otherSlowStatus == null) return this;
            return this.speedModifier > otherSlowStatus.speedModifier ? this : otherSlowStatus;
        }
    }
}