using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class YasuoEFIX : IBuffGameScript
    {
        public BuffType BuffType => BuffType.INTERNAL;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => true;

        public IStatsModifier StatsModifier { get; private set; }

        private readonly IAttackableUnit target = Spells.YasuoDashWrapper._target;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var time = 0.6f - ownerSpell.CastInfo.SpellLevel * 0.1f;
            var damage = 50f + ownerSpell.CastInfo.SpellLevel * 20f + unit.Stats.AbilityPower.Total * 0.6f;
            owner.SetTargetUnit(null);
            AddParticleTarget(owner, unit, "Yasuo_Base_E_Dash.troy", unit);
            AddParticleTarget(owner, target, "Yasuo_Base_E_dash_hit.troy", target);
            var to = Vector2.Normalize(target.Position - unit.Position);
            unit.SetStatus(StatusFlags.Ghosted, true);

            var xy = unit as IObjAiBase;
            xy.SetTargetUnit(null);
            FaceDirection(target.Position, owner);
            ForceMovement(unit, "Spell3", GetPointFromUnit(owner, 475), 750f + unit.Stats.MoveSpeed.Total * 0.6f, 0, 0, 0);
            CancelDash(unit);
            owner.PlayAnimation("Spell3", 0.45f, 0, 1);
            CreateTimer(2.5f, () => { unit.SetStatus(StatusFlags.Ghosted, false); });
            target.TakeDamage(unit, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            CancelDash(unit);
        }

        public void OnUpdate(float diff)
        {
            //empty
        }
    }
}