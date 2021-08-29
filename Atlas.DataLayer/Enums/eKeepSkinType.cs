using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.DataLayer
{
	/// <summary>
	/// Skin types to use for this keep
	/// 0 = any, 1 = old, 2 = new
	/// </summary>
	public enum eKeepSkinType : byte
	{
		/// <summary>
		/// Use server proerty to determine skin
		/// </summary>
		Any = 0,
		/// <summary>
		/// Use old skins
		/// </summary>
		Old = 1,
		/// <summary>
		/// Use new skins
		/// </summary>
		New = 2,
	}
}
