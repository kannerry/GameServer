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
    public class ZedPBAOEDummy : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
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
            PlayAnimation(owner, "Spell3", 0.5f);
            AddParticleTarget(owner, null, "Zed_E_cas.troy", owner);
            spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                Type = SectorType.Area
            });

            if (ZedShadowDashMissile.WShadow != null)
            {
                ZedShadowDashMissile.WShadow.PlayAnimation("Spell3", 0.5f);
                AddParticleTarget(owner, null, "Zed_E_cas.troy", ZedShadowDashMissile.WShadow);
                spell.CreateSpellSector(new SectorParameters
                {
                    Length = 250f,
                    CanHitSameTarget = false,
                    Type = SectorType.Area,
                    BindObject = ZedShadowDashMissile.WShadow
                });
            }
            if (ZedShadowDashMissile.RShadow != null)
            {
                ZedShadowDashMissile.RShadow.PlayAnimation("Spell3", 0.5f);
                AddParticleTarget(owner, null, "Zed_E_cas.troy", ZedShadowDashMissile.RShadow);
                spell.CreateSpellSector(new SectorParameters
                {
                    Length = 250f,
                    CanHitSameTarget = false,
                    Type = SectorType.Area,
                    BindObject = ZedShadowDashMissile.RShadow
                });
            }

        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (owner != target)
            {
                var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
                var damage = 40 + spell.CastInfo.SpellLevel * 30 + AD;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, null, "Zed_E_tar.troy", target);
                owner.GetSpell(2).LowerCooldown(2);

                if (ZedMarker.startdmgtrack == true)
                {
                    ZedMarker.bonusdmg += damage * 0.25f;
                }

                if (!target.HasBuff("ZedSlow"))
                {
                    AddBuff("ZedSlow", 1.5f, 1, spell, target, owner);
                }
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