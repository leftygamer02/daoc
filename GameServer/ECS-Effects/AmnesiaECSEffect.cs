using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOL.GS
{
    public class AmnesiaECSEffect : ECSGameSpellEffect
    {
        public AmnesiaECSEffect(ECSGameEffectInitParams initParams)
            : base(initParams) { }

        public override void OnStartEffect()
        {
            // "Your mind goes blank and you forget what you were doing!"
            // "{0} forgets what they were doing!"
            OnEffectStartsMsg(Owner, true, true, true);
        }

        public override void OnStopEffect()
        {
            // "Your energy field dissipates."
            // "{0}'s energy field dissipates."
            //OnEffectExpiresMsg(Owner, false, false, false);
        }
    }
}
