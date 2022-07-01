using Unity.Entities;

namespace SpaceArcade.Movement
{
    [GenerateAuthoringComponent]
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }
}