using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class KarthusPassive : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; }

        private float timer = 0f;
        private float tickCount = 0f;
        private IChampion champion;
        private IBuff thisBuff;
        float atk;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnDeath.AddListener(this, unit, OnDeath, true);
            champion = unit as IChampion;
            thisBuff = buff;
            champion.SetStatus(StatusFlags.CanMove, false);
            //These Particles aren't working
            //p = AddParticleTarget(unit, null, "Sion_Skin01_Passive_Skin.troy", unit, buff.Duration);
            //p2 = AddParticleTarget(unit, null, "Sion_Skin01_Passive_Ax.troy", unit, buff.Duration);

            for (byte i = 0; i < 4; i++)
            {
                if (champion != null)
                {
                    //SealSpellSlot(champion, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, true);
                }
            };
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //string[] originalAbilities = new string[] { "KogMawQ", "KogMawBioArcaneBarrage", "KogMawVoidOoze", "KogMawLivingArtillery" };
            champion.SetStatus(StatusFlags.CanMove, true);
            for (byte i = 0; i < 4; i++)
            {
                if (champion != null)
                {
                    //SealSpellSlot(champion, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, false);
                }
            }

            unit.TakeDamage(unit, 100000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);

            RemoveBuff(unit, "KarthusPassiveDelay");
            RemoveBuff(unit, "KarthusPassive");
            //RemoveParticle(p);
            //RemoveParticle(p2);
        }

        public void OnDeath(IDeathData deathData)
        {
            if (thisBuff != null && !thisBuff.Elapsed())
            {
                thisBuff.DeactivateBuff();
            }
        }

        public void OnUpdate(float diff)
        {
            timer += diff;
            if (timer > 250f && champion != null)
            {
                champion.TakeDamage(champion, 1 + champion.Stats.Level + tickCount * (0.7f * (champion.Stats.Level * 0.7f)) * 2, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                timer = 0;
                tickCount++;
            }
        }
    }
}