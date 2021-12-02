using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class CurseoftheSadMummy : ISpellScript
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
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            PlayAnimation(owner, "SPELL4");
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var ownerr = spell.CastInfo.Owner as IChampion;
            var spellLevel = ownerr.GetSpell("CurseoftheSadMummy").CastInfo.SpellLevel;
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.8f;
            var damage = 50 + spellLevel * 100 + ap;

            AddParticleTarget(ownerr, null, "CurseBandages_cas1.troy", ownerr, 1f);

            foreach (var enemy in GetUnitsInRange(ownerr.Position, 550, true))
            {
                if (enemy.Team != ownerr.Team)
                {
                    enemy.TakeDamage(ownerr, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    AddBuff("Stun", 1.5f, 1, spell, enemy, ownerr);
                    AddParticleTarget(enemy, enemy, "Amumu_Sadrobot_Ultwrap.troy", enemy, 1f);
                }
            }
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