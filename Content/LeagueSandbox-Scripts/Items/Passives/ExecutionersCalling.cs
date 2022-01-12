﻿using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemPassives
{
    public class ItemID_3123 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase _owner;

        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            AddBuff("HealCheck", 6.0f, 1, _owner.GetSpell(0), Unit, _owner);
            var x = AddParticleTarget(_owner, Unit, "global_grievousWound_tar.troy", Unit, bone: "C_BUFFBONE_GLB_HEAD_LOC");
            LogDebug("applied");
        }


        public void OnUpdate(float diff)
        {
        }

        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, owner);
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {

        }

        public void OnSpellCast(ISpell spell)
        {

        }

        public void OnSpellPostCast(ISpell spell)
        {

        }

        public void OnSpellChannel(ISpell spell)
        {

        }

        public void OnSpellChannelCancel(ISpell spell)
        {

        }

        public void OnSpellPostChannel(ISpell spell)
        {

        }
    }
}