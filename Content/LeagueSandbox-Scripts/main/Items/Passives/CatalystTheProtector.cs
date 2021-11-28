using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using Buffs;
using GameServerCore.Enums;
using GameServerCore.Domain;
using System;

namespace ItemPassives
{
    public class ItemID_3010 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(IObjAiBase owner)
        {
            ApiEventManager.OnLevelUp.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(IAttackableUnit obj)
        {
            obj.Stats.CurrentHealth += 150;
            obj.Stats.CurrentMana += 200;
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnLevelUp.RemoveListener(this);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}
