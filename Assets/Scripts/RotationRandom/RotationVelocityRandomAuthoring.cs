using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RotationRandom
{
    public class RotationVelocityRandomAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new RotationVelocity()
            {
                Angular = new float3(0.0f, 0.0f, Random.Range(-1f, 1f))
            });
        }
    }
}