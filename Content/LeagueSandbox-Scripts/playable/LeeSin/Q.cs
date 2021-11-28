using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{   
    public class BlindMonkQOne : ISpellScript
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
        static internal IAttackableUnit targ;
        static internal IParticle partic;
        static internal bool procced = false;
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            targ = target;
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 80 + ((spell.CastInfo.SpellLevel - 1) * 55) + ap;
            partic = AddParticleTarget(owner, target, "blindMonk_Q_tar_indicator.troy", owner, lifetime: 3.0f);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            owner.SetSpell("BlindMonkQTwo", 0, true);
            missile.SetToRemove();

            CreateTimer(3.0f, () => {
                if(procced == false)
                {
                    owner.SetSpell("BlindMonkQOne", 0, true);
                    owner.Spells[0].SetCooldown(spell.SpellData.Cooldown[0]);
                }
            });

            CreateTimer(0.1f, () => {
                if (target.IsDead)
                {
                    owner.SetSpell("BlindMonkQOne", 0, true);
                    owner.Spells[0].SetCooldown(spell.SpellData.Cooldown[0]);
                }
            });
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
    public class BlindMonkQTwo : ISpellScript
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
            ForceMovement(spell.CastInfo.Owner, "RUN_GLIDE", BlindMonkQOne.targ.Position, 1800f, 0, 2.0f, 0, movementOrdersType: ForceMovementOrdersType.CANCEL_ORDER);
            spell.CastInfo.Owner.SetSpell("BlindMonkQOne", 0, true);
            spell.CastInfo.Owner.Spells[0].SetCooldown(spell.SpellData.Cooldown[0]);
            BlindMonkQOne.partic.SetToRemove();
            BlindMonkQOne.procced = true;
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

