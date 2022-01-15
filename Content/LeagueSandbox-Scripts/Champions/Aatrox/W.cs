using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class AatroxW : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        private IObjAiBase _owner;
        private ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _spell = spell;
            _owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, ThirdHit, false);
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, ApplyWPassive, true);
        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AttackDamage.Total * 0.25f;
            float healthGain = 15 + (spell.CastInfo.SpellLevel * 45) + ap;
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
        }

        private int i = 0;

        public void ThirdHit(IAttackableUnit unit, bool crit)
        {
            if(_owner.GetSpell(1).CastInfo.SpellLevel > 0)
            {
                i++;

                if (i == 2)
                {
                    if (toggled == false)
                    {
                        AddBuff("AatroxWONHLifeBuff", float.MaxValue, 1, _spell, _owner, _owner, true);
                    }
                    if (toggled == true)
                    {
                        AddBuff("AatroxWONHPowerBuff", float.MaxValue, 1, _spell, _owner, _owner, true);
                    }
                }

                if (i == 3)
                {
                    _owner.RemoveBuffsWithName("AatroxWONHLifeBuff");
                    _owner.RemoveBuffsWithName("AatroxWONHPowerBuff");
                    if (toggled == false)
                    {
                        PerformHeal(_owner, _spell, _owner);
                    }
                    if (toggled == true)
                    {
                        _owner.Stats.CurrentMana += _owner.Stats.ManaPoints.Total * 0.1f;
                        _owner.TakeDamage(_owner, _owner.Stats.CurrentHealth * 0.1f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                        unit.TakeDamage(_owner, _owner.Stats.CurrentHealth * 0.1f, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    }
                    i = 0;
                }
            }
        }

        public void ApplyWPassive(ISpell spell)
        {
            AddBuff("AatroxWLife", float.MaxValue, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner, true);
            //AddBuff("AatroxWONHLifeBuff", float.MaxValue, 1, _spell, _owner, _owner, true);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        private bool toggled = false;

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            toggled = !toggled;
            if (toggled == false)
            {
                RemoveBuff(owner, "AatroxWPower");
                AddBuff("AatroxWLife", float.MaxValue, 1, spell, target, owner, true);
                LogDebug("toggled off");
                CreateTimer(0.1f, () =>
                {
                    _owner.RemoveBuffsWithName("AatroxWONHLifeBuff");
                    _owner.RemoveBuffsWithName("AatroxWONHPowerBuff");
                });
                i = 0;
            }
            if (toggled == true)
            {
                RemoveBuff(owner, "AatroxWLife");
                AddBuff("AatroxWPower", float.MaxValue, 1, spell, target, owner, true);
                LogDebug("toggled on");
                CreateTimer(0.1f, () =>
                {
                    _owner.RemoveBuffsWithName("AatroxWONHLifeBuff");
                    _owner.RemoveBuffsWithName("AatroxWONHPowerBuff");
                });
                i = 0;
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
}