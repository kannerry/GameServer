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
    public class slashCast : ISpellScript
    {
        private IObjAiBase Owner;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            //MissileParameters = new MissileParameters
            //{
            //    Type = MissileType.Circle
            //},
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
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

            PlayAnimation(owner, "Spell3");

            if (Extensions.IsVectorWithinRange(owner.Position, coords, 700))
            {
                ForceMovement(owner, "Spell3", coords, 1200, 0, 0, 0);
            }
            else
            {
                ForceMovement(owner, "Spell3", GetPointFromUnit(owner, 700), 1200, 0, 0, 0);
            }

            var DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = owner,
                Length = 225f,
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
            var Adratio = owner.Stats.AttackDamage.Total * 0.9f;
            var damage = 30 * (spell.CastInfo.SpellLevel) + Adratio;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
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