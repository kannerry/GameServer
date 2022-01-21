using GameServerCore.Domain;
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
    public class JinxQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            ApiEventManager.OnKill.AddListener(this, owner, passive, false);
        }
        public void passive(IDeathData data)
        {
            var owner = data.Killer;
            var bonus = owner.Stats.MoveSpeed.Total * 1.75f;
            owner.Stats.MoveSpeed.FlatBonus += 500;
            CreateTimer(6.0f, () => { owner.Stats.MoveSpeed.FlatBonus -= 250; });
            //will turn into buff later
        }
        public void OnLaunchAttack(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;

            spell.CastInfo.Owner.TargetUnit.TakeDamage(spell.CastInfo.Owner, spell.CastInfo.Owner.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            if (toggled == true)
            {
                var x = GetUnitsInRange(spell.CastInfo.Owner.TargetUnit.Position, 250, true);
                foreach (var unit in x)
                {
                    var ad = owner.Stats.AttackDamage.Total;
                    unit.TakeDamage(owner, ad, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                }
            }

            if (toggled == true)
            {
                owner.Stats.CurrentMana -= 20;
                CreateTimer(0.01f, () => { SpellCast(owner, 1, SpellSlotType.ExtraSlots, spell.CastInfo.Targets[0].Unit.Position, spell.CastInfo.Targets[0].Unit.Position, false, Vector2.Zero); });
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        internal static bool toggled = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            toggled = !toggled;
            if (toggled == false)
            {
                LogDebug("toggled off");
            }
            if (toggled == true)
            {
                LogDebug("toggled on");
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

    public class JinxBasicAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        private void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector arg4)
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