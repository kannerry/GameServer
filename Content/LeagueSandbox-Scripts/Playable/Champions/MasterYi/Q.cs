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

            var Champs = GetChampionsInRange(owner.Position, 50000, true);
            foreach (IChampion player in Champs)
            {
                owner.SetStatus(StatusFlags.Targetable, false);
                if (player.Team.Equals(owner.Team))
                {
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.0f);
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
            }
        };

        private IAttackableUnit firstTarget;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            // Skip the first target because we already hit it in SivirWAttack
            //if (firstTarget == target)
            //{
            //    return;
            //}
            var owner = spell.CastInfo.Owner;

            AddBuff("MasterYiQ", 0.1f, 1, spell, owner, owner);
            AddParticleTarget(owner, target, "MasterYi_Base_Q_Cas.troy", target);
            var damage = owner.Stats.AttackDamage.Total * (0.8f + (0.3f * (owner.GetSpell(0).CastInfo.SpellLevel - 1)));
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            CreateTimer(0.3f, () =>
            {
                if (!owner.HasBuff("MasterYiQ"))
                {
                    var to = Vector2.Normalize(firstTarget.Position - owner.Position);
                    TeleportTo(owner, firstTarget.Position.X - to.X * 100f, firstTarget.Position.Y - to.Y * 100f);
                    var Champs = GetChampionsInRange(owner.Position, 50000, true);
                    foreach (IChampion player in Champs)
                    {
                        owner.SetStatus(StatusFlags.Targetable, true);
                        if (player.Team.Equals(owner.Team))
                        {
                            owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f);
                        }
                        if (!(player.Team.Equals(owner.Team)))
                        {
                            owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f);
                            owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true);
                        }
                    }
                }
            });
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            firstTarget = target;
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