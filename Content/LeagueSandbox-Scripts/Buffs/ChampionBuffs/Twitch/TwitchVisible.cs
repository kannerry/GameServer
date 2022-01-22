using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class TwitchVisible : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_DEHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var Champs = GetAllChampionsInRange(owner.Position, 50000);
            owner.SetStatus(StatusFlags.Targetable, true);
            //owner.SetStatus(StatusFlags.NoRender, false);

            foreach (IChampion player in Champs)
            {
                if (!(player.Team.Equals(owner.Team)))
                {
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, true);
                }
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            LogDebug("deactivate");
            var owner = ownerSpell.CastInfo.Owner;
            var Champs = GetAllChampionsInRange(unit.Position, 50000);
            owner.SetStatus(StatusFlags.Targetable, false);
            //owner.SetStatus(StatusFlags.NoRender, true);

            foreach (IChampion player in Champs)
            {
                if (!(player.Team.Equals(owner.Team)))
                {
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                    owner.SetHealthbarVisibility((int)player.GetPlayerId(), owner, false);
                }
            }
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}