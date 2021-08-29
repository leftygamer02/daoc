using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.DataLayer.Models
{
    public partial class Appeal
    {
		[NotMapped]
		public string SeverityToName
		{
			get
			{
				switch (Severity)
				{
					case 1:
						return "Low";
					case 2:
						return "Medium";
					case 3:
						return "High";
					case 4:
						return "Critical";
					default:
						return "none";
				}
			}
			set { }
		}
	}
}
