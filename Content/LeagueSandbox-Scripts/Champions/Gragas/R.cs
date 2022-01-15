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
    public class GragasR : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };
        static internal ISpell _spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            ApiEventManager.OnSpellCast.AddListener(this, spell, PassiveHeal);
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }


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

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            FaceDirection(pos, target);
            var pos2 = GetPointFromUnit(target, -500);

            var xy = target as IObjAiBase;
            xy.SetTargetUnit(null);
            ForceMovement(target, "run", pos2, 1000, 0, 0, 0);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private Vector2 pos;
        public ISpellSector DamageSector;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            pos = end;
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, end, false, Vector2.Zero);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
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

    public class GragasRBoom : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        IObjAiBase _owner;
        static internal ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        Vector2 end_;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {

            end_ = end;

            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(ISpellMissile missile)
        {
            LogDebug("yo");
            AddParticle(_owner, null, "Gragas_Base_R_End.troy", end_, lifetime: 4.0f, reqVision: false);
            var DamageSector = _owner.GetSpell(3).CreateSpellSector(new SectorParameters
            {
                Tickrate = 100,
                Length = 350f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                SingleTick = true
            });

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