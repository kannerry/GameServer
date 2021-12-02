using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Tantrum : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
            var ownerr = spell.CastInfo.Owner as IChampion;
            var ap = ownerr.Stats.AbilityPower.Total;
            var spellLevel = ownerr.GetSpell("Tantrum").CastInfo.SpellLevel;
            var damage = 50 + spellLevel * 25 + ap * 0.5f;
            AddParticleTarget(ownerr, target, "Amumu_Sadrobot_Tantrum_tar.troy", target);
            SpellCast(owner, 2, SpellSlotType.ExtraSlots, ownerr.Position, ownerr.Position, false, Vector2.Zero);
            foreach (var enemy in GetUnitsInRange(ownerr.Position, 350, true))
            {
                if (enemy.Team != owner.Team)
                {
                    enemy.TakeDamage(ownerr, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddParticleTarget(ownerr, enemy, "Amumu_Sadrobot_Tantrum_tar.troy", enemy);
                }
            }
            ApiEventManager.OnLevelUpSpell.AddListener(this, owner.GetSpell("Tantrum"), AddAmumuPassive, false);
        }

        public void AddAmumuPassive(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("Tantrum", 99999f, 1, spell, owner, owner, true);
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