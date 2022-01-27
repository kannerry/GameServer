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
    public class AlphaStrike : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            SpellCast(owner, 2, SpellSlotType.ExtraSlots, false, target, owner.Position);
            owner.SetTargetUnit(null);

            var Champs = GetAllChampionsInRange(owner.Position, 50000);
            foreach (IChampion player in Champs)
            {
                owner.SetStatus(StatusFlags.Targetable, false);
                if (player.Team.Equals(owner.Team))
                {
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.0f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
                }
                if (!(player.Team.Equals(owner.Team)))
                {
                    if (player.IsAttacking)
                    {
                        player.CancelAutoAttack(false);
                    }
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.0f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
                }
            }
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

    public class AlphaStrikeBounce : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = false,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Chained,
                // Sivir W bounces until all units in bounce range have been hit.
                MaximumHits = 4,
                CanHitSameTarget = true
                
            }
        };

        static internal IAttackableUnit firstTarget;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("MasterYiQ", 0.4f, 1, spell, owner, owner);
            AddParticleTarget(owner, target, "MasterYi_Base_Q_Cas.troy", target);
            AddParticleTarget(owner, target, "MasterYi_Base_Q_Tar.troy", target);

            //SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, target, owner.Position);

            var damage = owner.Stats.AttackDamage.Total * 1.03f;
            var basedmg = 20 + 35 * owner.GetSpell(0).CastInfo.SpellLevel;
            target.TakeDamage(owner, damage + basedmg, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            firstTarget = target;
            CreateTimer(0.1f, () =>
            {
                var x = GetObjectNET(spell.CastInfo.MissileNetID) as ISpellMissile;
                if(x != null)
                {
                    x.SetSpeed(1200f); // feels good, not be accurate to "MissileSpeed" from json
                }
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

    public class AlphaStrikeTeleport : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = false,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target,
                CanHitSameTarget = true

            }
        };

        private IAttackableUnit firstTarget;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
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