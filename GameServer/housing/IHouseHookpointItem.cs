using System;
using System.Collections.Generic;
using System.Text;
using Atlas.DataLayer.Models;

namespace DOL.GS.Housing
{
	/// <summary>
	/// House item interface.
	/// </summary>
	/// <author>Aredhel</author>
	public interface IHouseHookpointItem
	{
		bool Attach(House house, uint hookpointID, ushort heading);
		bool Attach(House house, Atlas.DataLayer.Models.HouseHookpointItem hookedItem);
		bool Detach(GamePlayer player);
		int Index { get; }
		int TemplateID { get; }
	}
}
