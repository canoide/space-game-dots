using Unity.Entities;
using UnityEngine;

namespace SpaceArcade.Destroy
{
    public class DestroyByTimeAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float DestroyTime;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new DestroyByTime
            {
                DestroyTimer = DestroyTime
            });
        }
    }
}