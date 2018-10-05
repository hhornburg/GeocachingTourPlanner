using Itinero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeocachingTourPlanner.Types
{
	public class Waypoint
	{
		public float lat { get; set; }
		public float lon { get; set; }
		[XmlIgnore]
		public RouterPoint routerPoint { get; set; }

		public Waypoint(float lat, float lon)
		{
			this.lat = lat;
			this.lon = lon;
		}

		public Waypoint() { }
	}
}
