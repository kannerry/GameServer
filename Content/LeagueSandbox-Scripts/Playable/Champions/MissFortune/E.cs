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
    public class MissFortuneScattershot : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        private IBuff thisBuff;
        public ISpellSector DamageSector;

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
            var targetPos = GetPointFromUnit(owner, 800.0f);
            //SpellCast(owner, 1, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);

            //AddParticle(owner, null, "MissFortune_Base_E_cas.troy", spellpos, lifetime: 2.0f, reqVision: false);
            AddParticle(owner, null, "MissFortune_Base_E_Unit_Tar.troy", spellpos, lifetime: 3.0f, reqVision: false);
            AddParticle(owner, null, "MissFortune_Base_E_Unit_Tar_green.troy", spellpos, lifetime: 3.0f, reqVision: false);
            AddParticle(owner, null, "MissFortune_Base_E_Unit_Tar_red.troy", spellpos, lifetime: 3.0f, reqVision: false);

            float i = 0.0f;
            while (i <= 2.5f)
            {
                CreateTimer((float)(i), () =>
                {
                    var randOffsetX = (float)new Random().Next(-200, 200);
                    var randOffsetY = (float)new Random().Next(-200, 200);

                    var randPoint = new Vector2(spellpos.X + randOffsetX, spellpos.Y + randOffsetY);

                    var particle1 = AddParticlePos(owner, "MissFortune_Base_E_cas.troy", randPoint, randPoint, lifetime: 1.0f);
                });
                CreateTimer((float)(i + 0.1), () =>
                {
                    var randOffsetX = (float)new Random().Next(-200, 200);
                    var randOffsetY = (float)new Random().Next(-200, 200);

                    var randPoint = new Vector2(spellpos.X + randOffsetX, spellpos.Y + randOffsetY);

                    var particle1 = AddParticlePos(owner, "MissFortune_Base_E_cas.troy", randPoint, randPoint, lifetime: 1.0f);
                });
                CreateTimer((float)(i + 0.25), () =>
                {
                    var randOffsetX = (float)new Random().Next(-200, 200);
                    var randOffsetY = (float)new Random().Next(-200, 200);

                    var randPoint = new Vector2(spellpos.X + randOffsetX, spellpos.Y + randOffsetY);

                    var particle1 = AddParticlePos(owner, "MissFortune_Base_E_cas.troy", randPoint, randPoint, lifetime: 1.0f);
                });
                LogDebug(i.ToString());
                i += 0.5f;
            }

            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 400f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 3.0f
            });
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            //var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.125f;
            // var damage = 5 + (5 * spell.CastInfo.SpellLevel ) + ap;

            // target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("MissFortuneE", 1f, 1, spell, target, owner);
        }

        public void OnSpellChannelCancel(ISpell spell)
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