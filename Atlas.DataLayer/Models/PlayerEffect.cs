using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer.Models
{
    public class PlayerEffect : DataObjectBase
    {
        public bool IsHandler { get; set; }
        public int Var6 { get; set; }
        public int Var5 { get; set; }
        public int Var4 { get; set; }
        public int Var3 { get; set; }
        public int Var2 { get; set; }
        public int Var1 { get; set; }
        public int Duration { get; set; }
        public string EffectType { get; set; }
        public string SpellLine { get; set; }
        public int CharacterID { get; set; }

        public virtual Character Character { get; set; }
    }
}
