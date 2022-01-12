using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace ItemSpells
{
    public class ItemMorellosBane : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
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
            SetStatus(owner, GameServerCore.Enums.StatusFlags.CanMove, true);
            SetStatus(owner, GameServerCore.Enums.StatusFlags.CanCast, true);
            SetStatus(owner, GameServerCore.Enums.StatusFlags.CanAttack, true);
            SetStatus(owner, GameServerCore.Enums.StatusFlags.Targetable, true);
            if (owner.HasBuff("ZhonyasHourglassBuff"))
            {
                Buffs.ZhonyasHourglassBuff.p.SetToRemove();
                Buffs.ZhonyasHourglassBuff.p2.SetToRemove();
                owner.Stats.Armor.BaseValue = Buffs.ZhonyasHourglassBuff.RealBaseArmor;
                owner.Stats.MagicResist.BaseValue = Buffs.ZhonyasHourglassBuff.RealBaseMagicResist;
            }
            PerformHeal(owner, spell, target);
        }

        private void PerformHeal(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var ap = target.Stats.HealthPoints.Total * 0.10f;
            float healthGain = 150 + ap;
            if (target.HasBuff("HealCheck"))
            {
                healthGain *= 0.5f;
            }
            var newHealth = target.Stats.CurrentHealth + healthGain;
            target.Stats.CurrentHealth = Math.Min(newHealth, target.Stats.HealthPoints.Total);
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