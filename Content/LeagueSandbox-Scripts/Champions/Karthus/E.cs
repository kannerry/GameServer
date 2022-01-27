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
    public class KarthusDefile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
            SpellToggleSlot = 4
        };

        private IBuff thisBuff;
        public ISpellSector DamageSector;
        public ISpellSector SlowSector;

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
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;

            if (owner.HasBuff("KarthusEBuff"))
            {
                owner.RemoveBuffsWithName("KarthusEBuff");
            }
            else
            {
                thisBuff = AddBuff("KarthusEBuff", 25000.0f, 1, spell, owner, owner);

                DamageSector = spell.CreateSpellSector(new SectorParameters
                {
                    BindObject = owner,
                    Length = 500f,
                    Tickrate = 1,
                    CanHitSameTargetConsecutively = true,
                    OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                    Type = SectorType.Area
                });
            }
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (!spell.CastInfo.Owner.HasBuff(thisBuff))
            {
                return;
            }
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.125f;
            var damage = 4;
            target.TakeDamage(spell.CastInfo.Owner, 1, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
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