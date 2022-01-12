using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class PotionOfBrilliance : IBuffGameScript
    {
        public BuffType BuffType => BuffType.HEAL;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_CONTINUE;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle potion;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;
            switch (ownerSpell.CastInfo.Owner.Stats.Level)
            {
                case 1:
                    StatsModifier.AbilityPower.FlatBonus = 25f;
                    break;

                case 2:
                    StatsModifier.AbilityPower.FlatBonus = 25f;
                    break;

                case 3:
                    StatsModifier.AbilityPower.FlatBonus = 26f;
                    break;

                case 4:
                    StatsModifier.AbilityPower.FlatBonus = 27f;
                    break;

                case 5:
                    StatsModifier.AbilityPower.FlatBonus = 28f;
                    break;

                case 6:
                    StatsModifier.AbilityPower.FlatBonus = 29f;
                    break;

                case 7:
                    StatsModifier.AbilityPower.FlatBonus = 30f;
                    break;

                case 8:
                    StatsModifier.AbilityPower.FlatBonus = 31f;
                    break;

                case 9:
                    StatsModifier.AbilityPower.FlatBonus = 32f;
                    break;

                case 10:
                    StatsModifier.AbilityPower.FlatBonus = 32f;
                    break;

                case 11:
                    StatsModifier.AbilityPower.FlatBonus = 33f;
                    break;

                case 12:
                    StatsModifier.AbilityPower.FlatBonus = 34f;
                    break;

                case 13:
                    StatsModifier.AbilityPower.FlatBonus = 35f;
                    break;

                case 14:
                    StatsModifier.AbilityPower.FlatBonus = 36f;
                    break;

                case 15:
                    StatsModifier.AbilityPower.FlatBonus = 37f;
                    break;

                case 16:
                    StatsModifier.AbilityPower.FlatBonus = 38f;
                    break;

                case 17:
                    StatsModifier.AbilityPower.FlatBonus = 39f;
                    break;

                case 18:
                    StatsModifier.AbilityPower.FlatBonus = 40f;
                    break;

                default:
                    StatsModifier.AbilityPower.FlatBonus = 25f;
                    break;
            }
            unit.AddStatModifier(StatsModifier);
            potion = AddParticleTarget(caster, unit, "PotionofBrilliance_itm.troy", unit, buff.Duration, bone: "Buffbone_Glb_Ground_Loc");
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            potion.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
        }
    }
}