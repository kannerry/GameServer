using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class IreliaTranscendentBlades : IBuffGameScript
    {
        public BuffType BuffType => BuffType.INTERNAL;
        public BuffAddType BuffAddType => BuffAddType.REPLACE_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => true;

        public IStatsModifier StatsModifier { get; private set; }

        int counter = 0;
        IObjAiBase Unit;
        IBuff Buff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //Buff = buff;
            if (unit is IObjAiBase obj)
            {
                Unit = obj;
                obj.SetSpell("IreliaTranscendentBladesSpell", 3, true);
                //ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("IreliaTranscendentBladesSpell"));
            }
        }

        public void OnSpellCast(ISpell spell)
        {
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}