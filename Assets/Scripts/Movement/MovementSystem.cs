using Unity.Entities;
using Unity.Physics;

namespace SpaceArcade.Movement
{
    public partial class MovementSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(PhysicsVelocity), 
                ComponentType.ReadOnly<MovementDirection>(), 
                ComponentType.ReadOnly<MovementSpeed>()));
        }

        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            Entities
                .WithBurst()
                .ForEach((ref PhysicsVelocity physicsVelocity, in MovementDirection direction,
                        in MovementSpeed speed) =>
                    physicsVelocity.Linear = direction.Value * speed.Value)
                .Schedule();
        }
    }
}