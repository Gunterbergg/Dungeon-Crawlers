using System;
using UnityEngine;

namespace DungeonCrawlers 
{
    [Serializable]
    public class StatusComponent : MonoBehaviour { public virtual Status Status { get; set; } = null; }

    public abstract class Status
    {
        public float duration;
        public abstract Status Accumulate(Status other);
    }
}
