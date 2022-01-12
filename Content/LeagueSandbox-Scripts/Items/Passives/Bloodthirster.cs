using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
namespace ItemPassives
{
    public class ItemID_3072 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase ownermain;
        public void OnActivate(IObjAiBase owner)
        {
            ownermain = owner;
            StatsModifier.LifeSteal.PercentBonus = 0.2f;
            owner.AddStatModifier(StatsModifier);
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }
        float shield = 0;
        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            if(ownermain.Stats.CurrentHealth == ownermain.Stats.HealthPoints.Total)
            {
                if(shield < 350)
                {
                    var savedAD = ownermain.Stats.AttackDamage.Total;
                    shield += ownermain.Stats.AttackDamage.Total;
                    ownermain.ApplyShield(ownermain, ownermain.Stats.AttackDamage.Total, true, true, false);
                    CreateTimer(25.0f, () => { ownermain.ApplyShield(ownermain, -savedAD, true, true, false); });
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, owner);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
