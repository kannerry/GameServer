using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs

{
    internal class RedBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; }

        private IParticle p;
        private IParticle p2;
        private IObjAiBase _owner;
        private ISpell spell;
        private IBuff Buff;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;
            spell = ownerSpell;
            Buff = buff;

            p = AddParticleTarget(_owner, unit, "NeutralMonster_buf_red_offense_big.troy", unit, buff.Duration);
            p2 = AddParticleTarget(_owner, unit, "SRU_JungleBuff_Redbuff_Health.troy", unit);

            if (unit is IChampion)
            {
                ApiEventManager.OnHitUnit.AddListener(_owner, _owner, OnHit, false);
            }

            ApiEventManager.OnDeath.AddListener(this, unit, OnDeath, true);
        }

        public void OnDeath(IDeathData deathData)
        {
            if (_owner is IMonster monster)
            {
                if (deathData.Killer is IChampion killer)
                {
                    RemoveBuff(Buff);
                    AddBuff("RedBuff", 60f, 1, spell, killer, monster);
                }
            }
            if (_owner is IChampion champion)
            {
                if (deathData.Killer is IChampion killer)
                {
                    RemoveBuff(Buff);
                    AddBuff("RedBuff", Buff.Duration - Buff.TimeElapsed, 1, spell, killer, champion);
                }
            }
        }

        private void OnHit(IAttackableUnit unit, bool isCrit)
        {
            AddBuff("RedBuffBurn", 5f, 1, spell, unit, _owner);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p2);
            ApiEventManager.OnDeath.RemoveListener(this);
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}