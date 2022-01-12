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
    public class ItemID_3116 : IItemScript
    {
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IObjAiBase baseOwner;
        private bool Casted = false;

        public void OnActivate(IObjAiBase owner)
        {
            baseOwner = owner;
            int i = 0;
            while (i++ < 65)
            {
                ApiEventManager.OnSpellHit.AddListener(this, owner.GetSpell((byte)(i)), RylaisSlow, false);
            }
        }

        public void RylaisSlow(ISpell spell, IAttackableUnit unit, ISpellMissile mis, ISpellSector sec)
        {
            if (!spell.SpellName.Contains("BasicAttack"))
            {
                if (spell.CastInfo.Targets[0].Unit != null)
                {
                    var unitspell = spell.CastInfo.Targets[0].Unit;
                    AddParticle(unitspell, unitspell, "Item_TrueIce_Freeze_Slow.troy", unitspell.Position);
                    AddBuff("RylaisSlow", 1.5f, 1, baseOwner.GetSpell(0), unitspell, baseOwner);
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner)
        {
            ApiEventManager.OnSpellHit.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}