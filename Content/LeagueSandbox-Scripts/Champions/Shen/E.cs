using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class ShenShadowDash : ISpellScript
    {
        private IObjAiBase Owner;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            ApiEventManager.OnFinishDash.AddListener(this, owner, StopDashAnim, false);
        }

        public void StopDashAnim(IAttackableUnit unit)
        {
            StopAnimation(unit, "RUN");
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            FaceDirection(end, owner);
            var trueCoords = GetPointFromUnit(owner, 750f);
            var coords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            owner.SetTargetUnit(null);

            PlayAnimation(owner, "Dash", flags: AnimationFlags.Override);

            if (Extensions.IsVectorWithinRange(owner.Position, coords, 600))
            {
                ForceMovement(owner, "Dash", coords, 800 + owner.Stats.MoveSpeed.Total, 0, 0, 0);
            }
            else
            {
                ForceMovement(owner, "Dash", GetPointFromUnit(owner, 600), 800 + owner.Stats.MoveSpeed.Total, 0, 0, 0);
            }

            var DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = owner,
                Length = 160f,
                Tickrate = 50,
                CanHitSameTarget = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 1.0f
            });
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var apratio = owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 15 + (35 * spell.CastInfo.SpellLevel) + apratio;
            LogDebug(damage.ToString());
            target.TakeDamage(owner, damage + 1, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("ShenTaunt", 1.5f, 1, spell, target, owner);
            owner.Stats.CurrentMana += 40;
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

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}