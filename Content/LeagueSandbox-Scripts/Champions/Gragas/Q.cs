using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore;
using System;

namespace Spells
{
    public class GragasQ : ISpellScript
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

            ApiEventManager.OnSpellCast.AddListener(this, spell, PassiveHeal);

            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public ISpellSector DamageSector;

        //ApiEventManager.OnSpellCast.AddListener(this, spell, PassiveHeal);
        public void PassiveHeal(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;

            if (owner.HasBuff("GragasCanPassive"))
            {
                owner.RemoveBuffsWithName("GragasCanPassive");
                LogDebug("yo1");
                AddBuff("GragasPassiveCooldown", 8.0f, 1, spell, owner, owner);
                PerformHeal(owner, spell, owner);
            }

        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total * spell.SpellData.MagicDamageCoefficient;
            float healthGain = 15 + (spell.CastInfo.SpellLevel * 45) + ap;
            if (target.HasBuff("HealCheck"))
            {
                healthGain *= 0.5f;
            }
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        static internal bool reprocced = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var targetPos = GetPointFromUnit(owner, 850.0f);
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, spellpos, spellpos, false, Vector2.Zero);

            CreateTimer(0.1f, () =>
            {
                owner.SetSpell("GragasQToggle", 0, true);
                owner.GetSpell(0).SetCooldown(0);
            });
            CreateTimer(0.11f, () => { SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true); });

        }

        public void OnSpellChannel(ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
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

    public class GragasQMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            //ApiEventManager.OnSpellMissileEnd.AddListener(this, p, CastKeg, false);
            //ApiEventManager.OnCreateSector.AddListener(owner, spell.CastInfo.Owner, CastKeg);
            //ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        static internal ISpellSector DamageSector;
        IObjAiBase Owner;
        static internal ISpell daspell;
        IAttackableUnit datarget;

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
            daspell = spell;
            datarget = target;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, CastKeg, true);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {


        }
        static internal IParticle ally;
        static internal IParticle enemy;
        static internal Vector2 spellpos;
        public void CastKeg(ISpellMissile missile)
        {
            var owner = daspell.CastInfo.Owner;
            var targetPos = GetPointFromUnit(owner, 850.0f);
            spellpos = new Vector2(daspell.CastInfo.TargetPositionEnd.X, daspell.CastInfo.TargetPositionEnd.Z);

            SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);

            AddBuff("GragasQTimer", 4.0f, 1, daspell, owner, owner);

            enemy = AddParticle(owner, null, "Gragas_Base_Q_Enemy.troy", spellpos, lifetime: 4f, reqVision: false);
            ally = AddParticle(owner, null, "Gragas_Base_Q_Ally.troy", spellpos, lifetime: 4f, reqVision: false, teamOnly: owner.Team);
            //AddParticle(owner, null, "Gragas_Base_Q_Mis.troy", spellpos, lifetime: 0.5f , reqVision: false);
            ApiEventManager.OnSpellHit.AddListener(this, daspell, TargetExecute, true);
        }

        public void OnSpellChannel(ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.7f;
            var damage = 40 + spell.CastInfo.SpellLevel * 40 + ap;
            LogDebug("yo");
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("GragasQ", 2.5f, 1, spell, target, owner);
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

    public class GragasQToggle : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        static internal bool recast = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var spellpos = new Vector2(GragasQMissile.daspell.CastInfo.TargetPositionEnd.X, GragasQMissile.daspell.CastInfo.TargetPositionEnd.Z);
            AddParticle(owner, null, "gragas_barrelboom.troy", spellpos, lifetime: 4f, reqVision: false);
            //GragasQMissile.DamageSector = GragasQMissile.daspell.CreateSpellSector(new SectorParameters
            //{
            //    Length = 250f,
            //    SingleTick = true,
            //    CanHitSameTargetConsecutively = false,
            //    OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
            //    Type = SectorType.Area,
            //    Lifetime = 1f
            //});
            GragasQMissile.ally.SetToRemove();
            GragasQMissile.enemy.SetToRemove();
            GragasQ.reprocced = true;
            AddBuff("reprocGragasBarrel", 5.0f, 1, spell, owner, owner);

            AddParticle(owner, null, "gragas_barrelboom.troy", GragasQMissile.spellpos, lifetime: 4f, reqVision: false);
            var DamageSector = GragasQMissile.daspell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                Tickrate = 100,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 1f
            });

            owner.SetSpell("GragasQ", 0, true);
            var x = (new float[] { 10f, 9f, 8f, 7f, 6f }[spell.CastInfo.SpellLevel - 1]);
            CreateTimer(0.1f, () => { owner.GetSpell(0).SetCooldown(x); });
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

        public void OnUpdate(float diff)
        {
        }
    }

}