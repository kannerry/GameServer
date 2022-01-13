using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using Spells;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Passives
{
    public class YasuoPassive : ICharScript
    {

        IObjAiBase yasuo = null;
        float stanceTime = 500;
        float stillTime = 0;
        bool beginStance = false;
        bool stance = false;

        private ISpell originspell;
        private IObjAiBase ownermain;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            yasuo = owner;
            yasuo.Stats.CurrentMana = 0;
            ApiEventManager.OnTakeDamage.AddListener(this, owner, FlowProc, false);
        }
        bool tookdmg = false;
        public void FlowProc(IAttackableUnit unit1, IAttackableUnit unit2)
        {
            if(!(unit2 is ILaneMinion))
            {
                if(!(unit2 is IBaseTurret))
                {
                    tookdmg = true;
                    CreateTimer(0.1f, () => { tookdmg = false; });
                    if (max == true)
                    {
                        var unit1champ = unit1 as IChampion;
                        var x = (new float[] { 5f, 5f, 5f, 5f, 5f, 10f, 10f, 10f, 15f, 15f, 15f, 30f, 30f, 35f, 40f, 50f, 60f, 70f }[unit1.Stats.Level]);
                        var flowamt = 110 + x;
                        unit1champ.ApplyShield(unit1, flowamt, true, true, false);
                        CreateTimer(1.0f, () => { unit1champ.ApplyShield(unit1, -flowamt, true, true, false); });
                        unit1.Stats.CurrentMana = 0;
                        max = false;
                    }
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        bool max = false;

        public void OnUpdate(float diff)
        {

            //YASUO ULT RANGE
            //FOR SOME REASON
            //OnUpdate is CRINGE!!!

            if(YasuoQ3W.v != null)
            {
                if(!Extensions.IsVectorWithinRange(YasuoQ3W.v.Position, yasuo.Position, 1150))
                {
                    YasuoQ3W.v.SetToRemove();
                }
            }


            if (yasuo != null)
            {
                // moving
                if (!(yasuo.CurrentWaypoint.Value == yasuo.Position && !beginStance))
                {
                    if(yasuo.Stats.CurrentMana != yasuo.Stats.ManaPoints.Total)
                    {
                        yasuo.Stats.CurrentMana += 0.1f;
                    }
                    else
                    {
                        if(max != true)
                        {
                            max = true;
                        }
                    }
                }
                else
                {
                    stillTime = 0;
                    beginStance = false;
                    stance = false;
                }
            }
        }
    }
}