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
    public class WildCards : ISpellScript
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
        static internal Vector2 en;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            en = end;
            //PlayAnimation(owner, "Spell1");
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {

            //var owner = spell.CastInfo.Owner as IChampion;
            //for (int bladeCount = 0; bladeCount <= 2; bladeCount++)
            //{
            //    var targetPos = GetPointFromUnit(owner, 700f, (-25f + (bladeCount * 25f)));
            //    SpellCast(owner, 6, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
            //}

            if(spell.CastInfo.SpellSlot == 0)
            {
                var owner = spell.CastInfo.Owner as IChampion;
                for (int bladeCount = 0; bladeCount <= 2; bladeCount++)
                {
                    var targetPos = GetPointFromUnit(owner, 700f, (-25f + (bladeCount * 25f)));
                    SpellCast(owner, 6, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
                }
            }

            LogDebug(spell.CastInfo.SpellSlot.ToString());

            if (spell.CastInfo.SpellSlot == 3)
            {
                CreateTimer(0.02f, () => { ((IObjAiBase)spell.CastInfo.Owner).GetSpell(0).SetCooldown(0f); });
                AddParticle(spell.CastInfo.Owner, null, "SealFate_tar.troy", en, lifetime: 1.5f);
                PlayAnimation(spell.CastInfo.Owner, "Spell4");
                spell.CastInfo.Owner.StopMovement();
                spell.CastInfo.Owner.SetStatus(StatusFlags.CanMove, false);
                CreateTimer(1.5f, () => { spell.CastInfo.Owner.TeleportTo(en.X, en.Y); spell.CastInfo.Owner.SetStatus(StatusFlags.CanMove, true); spell.CastInfo.Owner.SetSpell("Destiny", 3, true); });
                Destiny.toggled = false;  
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

    public class SealFateMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };

        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        private void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile arg3, ISpellSector arg4)
        {
            var owner = spell.CastInfo.Owner;
            var damage = 5f + owner.Stats.AbilityPower.Total * 0.6f + owner.Spells[0].CastInfo.SpellLevel * 25f;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                //AddBuff("TalonWSlow", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, "UnderworldCardmasterStackAttack_tar.troy", target, 1f);
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