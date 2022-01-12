using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;

namespace Spells
{
    public class SionW : ISpellScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private ISpell thisSpell;

        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnKillUnit.AddListener(this, owner, OnKillMinion, false);
            ApiEventManager.OnKill.AddListener(this, owner, OnKillChampion, false);
            thisSpell = spell;
        }

        public void OnKillMinion(IDeathData deathData)
        {
            if (thisSpell.CastInfo.SpellLevel >= 1)
            {
                var owner = deathData.Killer;
                float extraHealth = 2f;

                StatsModifier.HealthPoints.FlatBonus = extraHealth;
                deathData.Killer.AddStatModifier(StatsModifier);
                owner.Stats.CurrentHealth += extraHealth;
            }
        }

        public void OnKillChampion(IDeathData deathData)
        {
            if (thisSpell.CastInfo.SpellLevel >= 1)
            {
                var owner = deathData.Killer;
                float extraHealth = 10f;

                StatsModifier.HealthPoints.FlatBonus = extraHealth;
                owner.AddStatModifier(StatsModifier);
                owner.Stats.CurrentHealth += extraHealth;
            }
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