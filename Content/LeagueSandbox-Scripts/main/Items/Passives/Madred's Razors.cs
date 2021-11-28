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
using System;
using GameServerCore.Enums;

namespace ItemPassives
{
    public class ItemID_3106 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        public void TargetExecute(IAttackableUnit unit, bool arg2)
        {
            unit.TakeDamage(unit, 50, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
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
