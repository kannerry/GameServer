using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class PotionOfGiantStrength : IBuffGameScript
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
                    StatsModifier.HealthPoints.FlatBonus = 120f;
                    break;

                case 2:
                    StatsModifier.HealthPoints.FlatBonus = 1126f;
                    break;

                case 3:
                    StatsModifier.HealthPoints.FlatBonus = 133f;
                    break;

                case 4:
                    StatsModifier.HealthPoints.FlatBonus = 140f;
                    break;

                case 5:
                    StatsModifier.HealthPoints.FlatBonus = 147f;
                    break;

                case 6:
                    StatsModifier.HealthPoints.FlatBonus = 153f;
                    break;

                case 7:
                    StatsModifier.HealthPoints.FlatBonus = 160f;
                    break;

                case 8:
                    StatsModifier.HealthPoints.FlatBonus = 167f;
                    break;

                case 9:
                    StatsModifier.HealthPoints.FlatBonus = 174f;
                    break;

                case 10:
                    StatsModifier.HealthPoints.FlatBonus = 180f;
                    break;

                case 11:
                    StatsModifier.HealthPoints.FlatBonus = 187f;
                    break;

                case 12:
                    StatsModifier.HealthPoints.FlatBonus = 194f;
                    break;

                case 13:
                    StatsModifier.HealthPoints.FlatBonus = 201f;
                    break;

                case 14:
                    StatsModifier.HealthPoints.FlatBonus = 207f;
                    break;

                case 15:
                    StatsModifier.HealthPoints.FlatBonus = 214f;
                    break;

                case 16:
                    StatsModifier.HealthPoints.FlatBonus = 221f;
                    break;

                case 17:
                    StatsModifier.HealthPoints.FlatBonus = 228f;
                    break;

                case 18:
                    StatsModifier.HealthPoints.FlatBonus = 235f;
                    break;

                default:
                    StatsModifier.HealthPoints.FlatBonus = 120f;
                    break;
            }
            StatsModifier.AttackDamage.FlatBonus = 15f;
            unit.AddStatModifier(StatsModifier);
            potion = AddParticleTarget(caster, unit, "PotionofGiantStrength_itm.troy", unit, buff.Duration, bone: "Buffbone_Glb_Ground_Loc");
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