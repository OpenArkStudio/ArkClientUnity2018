using GameFramework;
using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;
        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (Entity)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = ARKGameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }

        #region Custom Entity
        public static void ShowMyHero(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowEntity(typeof(MyHero), "Hero", data);
        }
        public static void ShowHero(this EntityComponent entityComponent, HeroData data)
        {
            entityComponent.ShowEntity(typeof(Hero), "Hero", data);
        }
        public static void ShowThruster(this EntityComponent entityComponent, ThrusterData data)
        {
            entityComponent.ShowEntity(typeof(ThrusterData), "Thruster",data);
        }
        public static void ShowBullet(this EntityComponent entityCompoennt, BulletData data)
        {
            entityCompoennt.ShowEntity(typeof(Bullet), "Bullet", data);
        }
        public static void ShowEffect(this EntityComponent entityComponent, EffectData data)
        {
            entityComponent.ShowEntity(typeof(Effect), "Effect", data);
        }
        #endregion

    }
}