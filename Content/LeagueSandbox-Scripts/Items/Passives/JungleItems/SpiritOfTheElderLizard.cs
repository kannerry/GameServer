using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace ItemPassives
{
    public class ItemID_3209 : IItemScript
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
            unit.TakeDamage(itemOwner, 18, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            CreateTimer(1.0f, () => { unit.TakeDamage(itemOwner, 8, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false); });
            CreateTimer(2.0f, () => { unit.TakeDamage(itemOwner, 8, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false); });
            itemOwner.Stats.CurrentHealth += itemOwner.Stats.AttackDamage.Total * 0.06f;
            itemOwner.Stats.CurrentMana += itemOwner.Stats.AttackDamage.Total * 0.03f;
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