using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ZhonyasHourglassBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_OVERLAPS;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        static internal IParticle p;
        static internal IParticle p2;
        static internal float RealBaseArmor;
        static internal float RealBaseMagicResist;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            p = AddParticleTarget(owner, unit, "zhonyas_ring_activate.troy", unit, buff.Duration, size: 2);
            p2 = AddParticleTarget(owner, unit, "zhonya_ring_self_skin.troy", unit, buff.Duration, size: 2);
            RealBaseArmor = owner.Stats.Armor.BaseValue;
            RealBaseMagicResist = owner.Stats.MagicResist.BaseValue;
            owner.SetStatus(StatusFlags.Targetable, false);
            owner.SetStatus(StatusFlags.CanMove, false);
            owner.Stats.Armor.BaseValue = 20000;
            owner.Stats.MagicResist.BaseValue = 20000;
            owner.StopMovement();

            int i = -1;
            while (i++ < 3)
                SealSpellSlot(owner, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, true);

            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, true);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            RemoveParticle(p);
            owner.SetStatus(StatusFlags.Targetable, true);
            owner.SetStatus(StatusFlags.CanMove, true);
            owner.Stats.Armor.BaseValue = RealBaseArmor;
            owner.Stats.MagicResist.BaseValue = RealBaseMagicResist;


            int i = -1;
            while (i++ < 3)
                SealSpellSlot(owner, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, false);

            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, false);

        }

        public void OnUpdate(float diff)
        {
        }
    }
}