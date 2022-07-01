using Unity.Entities;

namespace SpaceArcade.Weapon
{
    [GenerateAuthoringComponent]
    public struct Gun : IComponentData
    {
        public float Rate;
        public float Duration;
        public bool Fire;
        public Entity Projectile;
    }
}