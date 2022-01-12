using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class BlindedGravesW : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_DEHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle p;
        float visionrad;
        IChampion champ;
        static internal List<IObjAiBase> invisibleObjList = new List<IObjAiBase>();
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "katarina_daggered.troy", unit, buff.Duration);
            if (unit is IChampion)
            {
                champ = unit as IChampion;
            }
            var units = GetUnitsInRange(unit.Position, 50000, true);
            LogDebug("yo");
            foreach (var u in units)
            {
                if (!Extensions.IsVectorWithinRange(u.Position, unit.Position, 300))
                {
                    if (champ != null)
                    {
                        if (u is IObjAiBase)
                        {
                            if (!(u is IBaseTurret))
                            {
                                var uc = u as IObjAiBase;
                                champ.SetInvisible((int)champ.GetPlayerId(), uc, 0f, 0.0f);
                                champ.SetHealthbarVisibility((int)champ.GetPlayerId(), uc, false);
                                invisibleObjList.Add(uc);
                            }
                        }
                    }
                }
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            LogDebug("yo2");
            //unit.VisionRadius = visionrad;
            //RemoveParticle(p);
            foreach (var obj in invisibleObjList)
            {
                champ.SetInvisible((int)champ.GetPlayerId(), obj, 1f, 0.1f);
                champ.SetHealthbarVisibility((int)champ.GetPlayerId(), obj, true);
                CreateTimer(0.1f, () => { invisibleObjList.Remove(obj); });
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