using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using Spells;
using System.Numerics;

namespace Buffs
{
    internal class MasterYiQ : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.STACKS_AND_RENEWS;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //StatsModifier.AttackSpeed.PercentBonus = StatsModifier.AttackSpeed.PercentBonus + (0.15f + 0.05f * ownerSpell.CastInfo.SpellLevel);

            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

            var owner = ownerSpell.CastInfo.Owner;

            if (!AlphaStrikeBounce.firstTarget.IsDead)
            {
                var to = Vector2.Normalize(AlphaStrikeBounce.firstTarget.Position - owner.Position);
                owner.TeleportTo(AlphaStrikeBounce.firstTarget.Position.X - to.X * 100f, AlphaStrikeBounce.firstTarget.Position.Y - to.Y * 100f);
            }
            else
            {

            }

            var Champs = GetChampionsInRange(owner.Position, 50000, true);
            foreach (IChampion player in Champs)
            {
                owner.SetStatus(StatusFlags.Targetable, false);
                if (player.Team.Equals(owner.Team))
                {
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.0f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
                }
                if (!(player.Team.Equals(owner.Team)))
                {
                    if (player.IsAttacking)
                    {
                        player.CancelAutoAttack(false);
                    }
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.0f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
                }
            }

        }

        public void OnUpdate(float diff)
        {
        }
    }
}