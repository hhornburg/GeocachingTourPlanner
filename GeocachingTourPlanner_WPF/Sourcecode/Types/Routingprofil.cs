using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeocachingTourPlanner.Types
{

	public class Routingprofile
	{
		public string Name { get; set; }

		public override string ToString()
		{
			return Name;
		}
		/// <summary>
		/// maximal distace of trip in km
		/// </summary>
		public int MaxDistance { get; set; }
		public int PenaltyPerExtraKM { get; set; }
		/// <summary>
		/// Maximal available time in minutes
		/// </summary>
		public int MaxTime { get; set; }
		public int PenaltyPerExtra10min { get; set; }
		public int TimePerGeocache { get; set; }

		public SerializableItineroProfile ItineroProfile { get; set; }

		[XmlIgnore]
		public int RoutesOfthisType = 0;
	}
}
