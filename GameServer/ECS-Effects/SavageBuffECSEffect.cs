using DOL.GS.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOL.GS.PacketHandler;

namespace DOL.GS
{
    public class SavageBuffECSGameEffect : StatBuffECSEffect
    {
        public SavageBuffECSGameEffect(ECSGameEffectInitParams initParams)
            : base(initParams) { }

        public override void OnStartEffect()
        {
            if (!IsBuffActive && !IsDisabled)
            {
                ApplyBonus(Owner, (SpellHandler as AbstractSavageBuff).BonusCategory1, (SpellHandler as AbstractSavageBuff).Property1, SpellHandler.Spell.Value, Effectiveness, false);
                // "You parry with extra skill!"
                // "{0} begins parrying faster!"
                OnEffectStartsMsg(Owner, true, false, true);
                //(SpellHandler as AbstractSavageBuff).MessageToCaster(eChatType.CT_Spell, SpellHandler.Spell.Message1);
                //Message.SystemToArea(Owner, Util.MakeSentence(SpellHandler.Spell.Message2, Owner.GetName(0, true)), eChatType.CT_Spell, Owner);
            }
            else
                OnHealthCost();
        }

        public override void OnStopEffect()
        {
            ApplyBonus(Owner, (SpellHandler as AbstractSavageBuff).BonusCategory1, (SpellHandler as AbstractSavageBuff).Property1, SpellHandler.Spell.Value, Effectiveness, true);
            OnHealthCost();           
        }

        private void OnHealthCost()
        {
            if (SpellHandler.Spell.Power != 0)
            {
                int cost = 0;
                if (SpellHandler.Spell.Power < 0)
                    cost = (int)(SpellHandler.Caster.MaxHealth * Math.Abs(SpellHandler.Spell.Power) * 0.01);
                else
                    cost = SpellHandler.Spell.Power;
                if (Owner.Health > cost)
                    Owner.ChangeHealth(Owner, eHealthChangeType.Spell, -cost);
            }
        }
    }
}
