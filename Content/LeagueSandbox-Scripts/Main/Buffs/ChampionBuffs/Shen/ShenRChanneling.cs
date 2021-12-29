using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System;
using System.Collections.Generic;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs

{
    internal class ShenRChanneling : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase _sourceUnit;
        IAttackableUnit _shen;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _sourceUnit = buff.SourceUnit;
            _shen = unit;
            _shen.SetStatus(StatusFlags.CanMove, false);
            _shen.SetStatus(StatusFlags.CanCast, false);
            _shen.StopMovement();
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _shen.SetStatus(StatusFlags.CanMove, true);
            _shen.SetStatus(StatusFlags.CanCast, true);
            Vector2 to = Vector2.Normalize(_sourceUnit.Position - _shen.Position);
            Vector2 tpPos = new Vector2(_sourceUnit.Position.X + to.X * 100f, _sourceUnit.Position.Y + to.Y * 100f);
            _shen.TeleportTo(tpPos.X, tpPos.Y);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}