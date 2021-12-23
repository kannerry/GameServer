using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class RengarWBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            StatsModifier.Armor.BaseBonus += 5 + (5 * ownerSpell.CastInfo.SpellLevel);
            StatsModifier.MagicResist.BaseBonus += 5 + (5 * ownerSpell.CastInfo.SpellLevel);
            var swag = GetChampionsInRange(unit.Position, 450, true);
            foreach(var unitx in swag)
            {
                if(unitx.Team != unit.Team)
                {
                    StatsModifier.Armor.BaseBonus += StatsModifier.Armor.BaseBonus * 0.5f;
                    StatsModifier.MagicResist.BaseBonus += StatsModifier.Armor.BaseBonus * 0.5f;
                }
            }
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}