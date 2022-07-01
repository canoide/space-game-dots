using Unity.Entities;

namespace SpaceArcade.Destroy
{
    public struct DestroyTag : IComponentData
    {
        public DestroyType Type;
    }
}