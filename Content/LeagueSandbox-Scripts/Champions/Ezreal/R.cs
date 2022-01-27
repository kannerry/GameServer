using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class EzrealTrueshotBarrage : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            }
            // TODO
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
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddParticleTarget(owner, owner, "Ezreal_bow_huge.troy", owner, bone: "L_HAND");
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (missile is ISpellCircleMissile skillshot)
            {
                AddBuff("EzrealRisingSpellForce", 6.0f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
                var reduc = Math.Min(skillshot.ObjectsHit.Count, 7);
                var bonusAd = owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue;
                var ap = owner.Stats.AbilityPower.Total * 0.9f;
                var damage = 200f + (150f * (spell.CastInfo.SpellLevel - 1)) + bonusAd + ap;
                target.TakeDamage(owner, damage * (1f - (reduc - 1f) / 10f), DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
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