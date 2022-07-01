using Unity.Entities;

namespace SpaceArcade.Destroy
{
    public struct DestroyByTime : IComponentData
    {
        public float DestroyTimer;
    }
}