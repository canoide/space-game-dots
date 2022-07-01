using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RotationRandom
{
    public partial class RotationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate(GetEntityQuery(typeof(Rotation), typeof(RotationVelocity)));
        }

        protected override void OnUpdate()
        {
            float speed = 5.0f;
            float deltaTime = Time.DeltaTime * speed;

            Entities
                .WithBurst()
                .ForEach((ref Rotation rotation, in RotationVelocity rotationComponent) =>
                {
                    rotation.Value = math.mul(rotation.Value,
                        quaternion.EulerXYZ(rotationComponent.Angular * deltaTime));
                })
                .ScheduleParallel();
        }
    }
}
