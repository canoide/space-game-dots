using Unity.Entities;
using Unity.Physics.Stateful;

namespace SpaceArcade.Destroy
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(StatefulTriggerEventBufferSystem))]
    public partial class DestroyByTriggerSystem : SystemBase
    {
        //private BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
        //private EndFixedStepSimulationEntityCommandBufferSystem m_CommandBufferSystem;
        private EndSimulationEntityCommandBufferSystem m_EndCommandBufferSystem;
        //EntityQueryMask m_NonTriggerMask;

        protected override void OnCreate()
        {
            //m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            //m_CommandBufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
            m_EndCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            /*m_NonTriggerMask = EntityManager.GetEntityQueryMask(
                GetEntityQuery(new EntityQueryDesc
                {
                    None = new ComponentType[]
                    {
                        typeof(StatefulTriggerEvent)
                    }
                })
            );*/
            RequireForUpdate(GetEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[] { typeof(DestroyTag) },
                All = new ComponentType[] { typeof(DestroyByTrigger) }
            }));
        }

        // protected override void OnUpdate()
        // {
        //     //var entityCommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();
        //     var commandBuffer = m_CommandBufferSystem.CreateCommandBuffer();
        //     //var nonTriggerMask = m_NonTriggerMask;
        //     Entities
        //         .WithoutBurst()
        //         .ForEach((Entity entity, ref DynamicBuffer<StatefulTriggerEvent> triggerEventBuffer,
        //             ref DestroyByContact triggerDamage) =>
        //         {
        //             for (int i = 0; i < triggerEventBuffer.Length; i++)
        //             {
        //                 var triggerEvent = triggerEventBuffer[i];
        //                 //var otherEntity = triggerEvent.GetOtherEntity(entity);
        //                 /*if (triggerEvent.State == StatefulEventState.Stay || !nonTriggerMask.Matches(otherEntity))
        //                     continue;*/
        //                 if (triggerEvent.State == StatefulEventState.Enter)
        //                 {
        //                     commandBuffer.DestroyEntity(entity);
        //                     var otherEntity = triggerEvent.GetOtherEntity(entity);
        //                     commandBuffer.DestroyEntity(otherEntity);
        //                 }
        //             }
        //         })
        //         .Run();
        //     
        //     m_CommandBufferSystem.AddJobHandleForProducer(Dependency);
        // }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            Entities
                //.WithBurst()
                .WithoutBurst()
                .ForEach((Entity entity, int entityInQueryIndex,
                    ref DynamicBuffer<StatefulTriggerEvent> triggerEventBuffer,
                    ref DestroyByTrigger triggerDamage) =>
                {
                    for (int i = 0; i < triggerEventBuffer.Length; i++)
                    {
                        var triggerEvent = triggerEventBuffer[i];
                        if (triggerEvent.State == StatefulEventState.Enter)
                        {
                            commandBuffer.AddComponent(entityInQueryIndex, entity,
                                new DestroyTag { Type = DestroyType.Trigger });
                            var otherEntity = triggerEvent.GetOtherEntity(entity);
                            commandBuffer.AddComponent(entityInQueryIndex, otherEntity,
                                new DestroyTag { Type = DestroyType.Trigger });
                        }
                    }
                })
                .ScheduleParallel();
            m_EndCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}