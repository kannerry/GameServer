using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class MissFortuneBulletTime : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            ChannelDuration = 2f,
            //IsDamagingSpell = true,
            //NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

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

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);
            PlayAnimation(owner, "Spell4", 2f);

            //DamageSector = spell.CreateSpellSector(new SectorParameters
            //{
            //    Length = 1400f,
            //    Tickrate = 4,
            //    ConeAngle = 15.0f,
            //    CanHitSameTargetConsecutively = true,
            //    Type = SectorType.Cone,
            //    Lifetime = 2.0f
            //});
            //AddParticle(owner, null, "MissFortune_Base_W_buf.troy", GetPointFromUnit(owner, 1450f), lifetime: 5.0f);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.125f;
            var ad = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.125f;
            if (spell.CastInfo.SpellLevel == 1)
            {
                var damage = 50 + ap + ad;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }
            else
            {
                var damage = 75 + (50 * (spell.CastInfo.SpellLevel - 2)) + ap + ad;
                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            }

            // AddBuff("MissFortuneR", 0.5f, 1, spell, target, owner);
        }

            //for (int arrowCount = -15; arrowCount< 15; arrowCount = arrowCount + 5)
            //{
            //    SpellCast(owner, 2, SpellSlotType.ExtraSlots, GetPointFromUnit(owner, 1000, arrowCount), GetPointFromUnit(owner, 1000, arrowCount), true, Vector2.Zero);
            //}

    public void OnSpellChannel(ISpell spell)
        {
            LogDebug("OnSpellChannel");
            AddBuff("MissFortuneR", 2.0f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
            LogDebug("OnSpellChannelCancel");
            StopAnimation(spell.CastInfo.Owner, "Spell4");
            var Owner = spell.CastInfo.Owner;
            RemoveBuff(Owner, "MissFortuneR");
            //if(pList.Count < 1)
            //{
            //    foreach(var particle in pList)
            //    {
            //        particle.SetToRemove();
            //        pList.Remove(particle);
            //    }
            //}
        }

        public void OnSpellPostChannel(ISpell spell)
        {
            LogDebug("OnSpellChannelCancel");
            StopAnimation(spell.CastInfo.Owner, "Spell4", fade: true);
            var Owner = spell.CastInfo.Owner;
            RemoveBuff(Owner, "MissFortuneR");
        }

        public void OnUpdate(float diff)
        {
        }
    }

    public class MissFortuneBullets : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
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
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (missile is ISpellCircleMissile skillshot)
            {
                var owner = spell.CastInfo.Owner;

                //target.TakeDamage(owner, 50, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_RAW, true);
                float ap = owner.Stats.AbilityPower.Total * 0.20f;

                float damage = ap + 50 + 25 * spell.CastInfo.SpellLevel - 1;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

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