using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace ItemPassives
{
    public class ItemID_3715 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase itemOwner;

        public void OnActivate(IObjAiBase owner)
        {
            itemOwner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        // S5_SummonerSmitePlayerGanker - Blue Smite
        // S5_SummonerSmiteDuel - Red Smite
        // S5_SummonerSmiteQuick - White Smite - INVADE SMITE +20 GOLD +HALF COOLDOWN +175% MS OVER  2S (all on enemy camp)

        private void TargetExecute(IAttackableUnit unit, bool crit)
        {
            LogDebug(unit.GetType().ToString());
            if (unit is IMonster)
            {
                unit.TakeDamage(itemOwner, 22, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                CreateTimer(1.0f, () => { unit.TakeDamage(itemOwner, 11, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false); });
                itemOwner.Stats.CurrentHealth += 10;
                itemOwner.Stats.CurrentMana += 5;
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