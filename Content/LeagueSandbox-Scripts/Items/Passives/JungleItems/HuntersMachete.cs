using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace ItemPassives
{
    public class ItemID_1039 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase itemOwner;

        public void OnActivate(IObjAiBase owner)
        {
            itemOwner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(IAttackableUnit unit, bool crit)
        {
            LogDebug(unit.GetType().ToString());
            if (unit is IMonster)
            {
                unit.TakeDamage(itemOwner, 15, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                CreateTimer(1.0f, () => { unit.TakeDamage(itemOwner, 7, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false); });
                itemOwner.Stats.CurrentHealth += 7;
                itemOwner.Stats.CurrentMana += 3;
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}