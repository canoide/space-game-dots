using Unity.Entities;
using Unity.Mathematics;

namespace SpaceArcade.Movement
{
    [GenerateAuthoringComponent]
    public struct MovementDirection : IComponentData
    {
        public float3 Value;
    }
}
