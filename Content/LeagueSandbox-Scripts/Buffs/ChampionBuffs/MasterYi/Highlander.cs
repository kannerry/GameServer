using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class Highlander : IBuffGameScript
    {

        IObjAiBase yi = null;
        float stanceTime = 500;
        float stillTime = 0;
        bool beginStance = false;
        bool stance = false;

        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle highlander;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            yi = owner;
            highlander = AddParticleTarget(owner, unit, "Highlander_buf.troy", unit);

            owner.StopMovement();

            StatsModifier.MoveSpeed.PercentBonus = StatsModifier.MoveSpeed.PercentBonus + (15f + ownerSpell.CastInfo.SpellLevel * 10) / 100f;
            StatsModifier.AttackSpeed.PercentBonus = StatsModifier.AttackSpeed.PercentBonus + (5f + ownerSpell.CastInfo.SpellLevel * 25) / 100f;
            unit.AddStatModifier(StatsModifier);
            // TODO: add immunity to slows
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(highlander);
        }

        public void OnUpdate(float diff)
        {
            if (yi != null)
            {
                // moving
                if (!(yi.CurrentWaypoint.Value == yi.Position && !beginStance))
                {
                    PlayAnimation(yi, "RUN_HASTE", 1f, flags: AnimationFlags.UniqueOverride);
                    beginStance = true;
                    if (stillTime >= stanceTime && !stance)
                    {
                        beginStance = false;
                        stance = true;
                        //PlayAnimation(diana, "Attack1", flags: GameServerCore.Enums.AnimationFlags.Lock);
                    }
                    else
                    {
                        stillTime += diff;
                    }
                }
                else
                {
                    stillTime = 0;
                    beginStance = false;
                    stance = false;
                }
            }
        }
    }
}