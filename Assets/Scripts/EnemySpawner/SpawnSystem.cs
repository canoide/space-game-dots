using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace EnemySpawner
{
    public partial class SpawnSystem : SystemBase
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            RequireForUpdate(GetEntityQuery(typeof(SpawnSettings), typeof(LocalToWorld), typeof(Translation)));
        }

        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var random = new Random((uint)Environment.TickCount);
            Entities
                .WithBurst()
                .ForEach((Entity entity, int entityInQueryIndex, ref SpawnSettings spawnSettings,
                    in LocalToWorld location) =>
                {
                    spawnSettings.Timer -= dt;
                    if (spawnSettings.Timer > 0) return;
                    spawnSettings.Timer = random.NextFloat(spawnSettings.Interval.x, spawnSettings.Interval.y);
                    var instance = commandBuffer.Instantiate(entityInQueryIndex, spawnSettings.Prefab);
                    var position = location.Position +
                                   new float3(random.NextFloat(spawnSettings.Range.x, spawnSettings.Range.y), 0, 0);
                    commandBuffer.SetComponent(entityInQueryIndex, instance, new Translation { Value = position });
                })
                .ScheduleParallel();

            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}