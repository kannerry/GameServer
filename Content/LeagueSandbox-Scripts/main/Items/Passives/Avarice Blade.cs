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
    public class ItemID_3093 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IObjAiBase itemOwner;
        public void OnActivate(IObjAiBase owner)
        {
            itemOwner = owner;
            ApiEventManager.OnKillUnit.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(IDeathData obj)
        {
            itemOwner.Stats.Gold += 2;
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnKillUnit.RemoveListener(this);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}
