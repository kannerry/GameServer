using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class YasuoQ3W : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastTime = 0.5f
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            yasuo = owner;
            _spell = spell;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        ISpell _spell;

        static internal IObjAiBase yasuo;
        static internal ISpellMissile v;
        static internal IMinion mushroom;

        public void EQ3(IAttackableUnit unit)
        {
            var owner = unit as IChampion;
            owner.PlayAnimation("Spell3A", 0.5f, 0, 1);
            AddParticleTarget(owner, owner, "Yasuo_Base_EQ_cas.troy", owner);
            var sector = _spell.CreateSpellSector(new SectorParameters
            {
                BindObject = _spell.CastInfo.Owner,
                Length = 215f,
                SingleTick = true,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
            owner.SetSpell("YasuoQW", 0, true);
            owner.RemoveBuffsWithName("YasuoQ02");
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            ISpellScriptMetadata s = new SpellScriptMetadata()
            {
                TriggersSpellCasts = true,
                MissileParameters = new MissileParameters
                {
                    Type = MissileType.Circle,
                }
            };

            if (owner.HasBuff("YasuoEFIX"))
            {
                ApiEventManager.OnFinishDash.AddListener(this, owner, EQ3, true);
            }
            else
            {
                v = spell.CreateSpellMissile(s.MissileParameters);
                mushroom = AddMinion(owner, "TeemoMushroom", "TeemoMushroom", owner.Position);
                mushroom.SetStatus(StatusFlags.Ghosted, true);
                var Champs = GetAllChampionsInRange(owner.Position, 50000);
                var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
                FaceDirection(spellPos, owner, true);
                owner.StopMovement();
                CreateTimer(0.1f, () => { FaceDirection(spellPos, owner, true); });
                owner.PlayAnimation("Spell1C", 0.5f, 0, 1);
                owner.SetSpell("YasuoQW", 0, true);
                CreateTimer(0.01f, () => { ((IObjAiBase)owner).GetSpell(0).SetCooldown(1.33f); });
                owner.RemoveBuffsWithName("YasuoQ02");
                var x = AddParticle(owner, mushroom, "Yasuo_Base_Q_wind_mis.troy", owner.Position);
                float inte = 0.0f;
                float intinc = 0.01f;
                while (inte < 1.0f)
                {
                    CreateTimer(inte, () => { mushroom.TeleportTo(v.Position.X, v.Position.Y); });
                    inte += intinc;
                }
                CreateTimer(1.5f, () => { mushroom.TakeDamage(owner, 20000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false); });
                foreach (IChampion player in Champs)
                {
                    mushroom.SetInvisible((int)player.GetPlayerId(), mushroom, 0f, 0f);
                    mushroom.SetHealthbarVisibility((int)player.GetPlayerId(), mushroom, false);
                }
                mushroom.SetCollisionRadius(0.0f);
                mushroom.SetStatus(StatusFlags.Targetable, false);
            }
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {

            var atkspeed = spell.CastInfo.Owner.Stats.AttackSpeedFlat * spell.CastInfo.Owner.Stats.AttackSpeedMultiplier.Total;
            var bonus = atkspeed - spell.CastInfo.Owner.Stats.AttackSpeedFlat;
            var cd = 4 * (1 - bonus);
            float cd1 = (float)Math.Max(cd, 1.33);

            CreateTimer(0.01f, () => { ((IObjAiBase)spell.CastInfo.Owner).GetSpell(0).SetCooldown(cd1, true); });

        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target is Champion)
            {
                SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
                CreateTimer(1.0f, () => { SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true); });
            }
            AddParticleTarget(spell.CastInfo.Owner, target, "Yasuo_Base_Q_hit_tar.troy", target);
            var owner = spell.CastInfo.Owner;
            var APratio = owner.Stats.AttackDamage.Total;
            var spelllvl = (spell.CastInfo.SpellLevel * 20);
            target.TakeDamage(owner, APratio + spelllvl + 2, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            var xy = target as IObjAiBase;
            xy.SetTargetUnit(null);
            ForceMovement(target, "RUN", new Vector2(target.Position.X + 5f, target.Position.Y + 5f), 13f, 0, 16.5f, 0);
            if (target is Champion)
            {
                AddBuff("Pulverize", 1.0f, 1, spell, target, spell.CastInfo.Owner);
            }

            int i = 0;
            if (HasBuff(spell.CastInfo.Owner, "StaticField"))
            {
                var ItemOwner = owner;
                ItemOwner.RemoveBuffsWithName("StaticField");
                AddBuff("StaticFieldCooldown", 7.0f, 1, ItemOwner.GetSpell(0), ItemOwner, ItemOwner);
                var x = GetUnitsInRange(target.Position, 600, true);
                foreach (var unit in x)
                {
                    if (unit.Team != ItemOwner.Team)
                    {
                        if (i < 4)
                        {
                            i++;
                            AddParticle(ItemOwner, unit, "volibear_R_chain_lighting_01.troy", target.Position);
                            unit.TakeDamage(ItemOwner, 100, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        }
                        CreateTimer(0.01f, () => { i = 0; });
                    }
                }
            }

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