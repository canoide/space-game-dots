using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace EnemySpawner
{
    [GenerateAuthoringComponent]
    public struct SpawnSettings : IComponentData
    {
        public Entity Prefab;
        public float2 Range;
        public float2 Interval;
        public float Timer;
    }
}