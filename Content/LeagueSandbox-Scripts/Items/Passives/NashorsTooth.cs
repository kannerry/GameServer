using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;

namespace ItemPassives
{
    public class ItemID_3115 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, TiamatExecute, false);
            StatsModifier.CooldownReduction.FlatBonus = 0.2f;
            owner.AddStatModifier(StatsModifier);
        }

        private void TiamatExecute(ISpell spell)
        {
            var unitx = spell.CastInfo.Targets[0].Unit;
            var owner = spell.CastInfo.Owner;
            var dmg = 15 + owner.Stats.AbilityPower.Total * 0.15f;
            unitx.TakeDamage(owner, dmg, GameServerCore.Enums.DamageType.DAMAGE_TYPE_MAGICAL, GameServerCore.Enums.DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}