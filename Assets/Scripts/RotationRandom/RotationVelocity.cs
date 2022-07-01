using Unity.Entities;
using Unity.Mathematics;

namespace RotationRandom
{
    public struct RotationVelocity : IComponentData
    {
        public float3 Angular;
    }
}
