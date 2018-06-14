using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner.Routing
{
	class Waypoint
	{
		public float lat { get; set; }
		public float lon { get; set; }

		public Waypoint(float lat, float lon)
		{
			this.lat = lat;
			this.lon = lon;
		}
	}
}
