using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using Buffs;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class ZedShuriken : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
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
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        Vector2 pos;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            pos = end;
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var targetPos = pos;
            FaceDirection(targetPos, owner);
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
            if(ZedShadowDashMissile.WShadow != null)
            {
                FaceDirection(targetPos, ZedShadowDashMissile.WShadow);
                ZedShadowDashMissile.WShadow.PlayAnimation("Spell1");
                SpellCast(owner, 1, SpellSlotType.ExtraSlots, targetPos, targetPos, true, ZedShadowDashMissile.WShadow.Position);
            }
            if (ZedShadowDashMissile.RShadow != null)
            {
                FaceDirection(targetPos, ZedShadowDashMissile.RShadow);
                ZedShadowDashMissile.RShadow.PlayAnimation("Spell1");
                SpellCast(owner, 1, SpellSlotType.ExtraSlots, targetPos, targetPos, true, ZedShadowDashMissile.RShadow.Position);
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

    public class ZedShurikenMisOne : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;
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

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            float ad = owner.Stats.AttackDamage.Total;
            float damage = 75 + (spell.CastInfo.SpellLevel - 1) * 40 + ad;
            if (missile is ISpellCircleMissile circleMissle && circleMissle.ObjectsHit.Count > 1)
            {
                damage *= 0.6f;
            }

            if(ZedMarker.startdmgtrack == true)
            {
                ZedMarker.bonusdmg += damage * 0.25f;
            }

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "Zed_Q_tar.troy", target);
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