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

namespace ItemPassives
{
    public class ItemID_3022 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private bool Casted = false;
        IChampion _owner;
        public void OnActivate(IObjAiBase owner)
        {
            _owner = owner as IChampion;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
        }

        private void TargetExecute(IAttackableUnit Unit, bool crit)
        {
            AddParticle(Unit, Unit, "Item_TrueIce_Freeze_Slow.troy", Unit.Position);
            AddBuff("RylaisSlow", 1.5f, 1, _owner.GetSpell(0), Unit, _owner);
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}