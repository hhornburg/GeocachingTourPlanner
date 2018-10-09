using Itinero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner.Routing
{
    public class PartialRoute
    {
        public Route partialRoute { get; set; }
        /// <summary>
        /// geocaches that are in reach from this partial route. NOT THOSE ON THE ROUTE
        /// </summary>
        public List<GeocacheRoutingInformation> GeocachesInReach { get; set; }

        public PartialRoute(Route partialRoute, List<GeocacheRoutingInformation> GeocachesInReach)
        {
            this.partialRoute = partialRoute;
            this.GeocachesInReach = GeocachesInReach;
        }

        public PartialRoute(Route partialRoute)
        {
            this.partialRoute = partialRoute;
        }

        public PartialRoute()
        {
            GeocachesInReach = new List<GeocacheRoutingInformation>();
        }
        public PartialRoute DeepCopy()
        {
            PartialRoute _DeepCopy = new PartialRoute();
            _DeepCopy.partialRoute = partialRoute;//No deeper copy needed since partial route won't be changed
            foreach (GeocacheRoutingInformation geocacheRoutingInfo in GeocachesInReach)
            {
                _DeepCopy.GeocachesInReach.Add(new GeocacheRoutingInformation(geocacheRoutingInfo));
            }
            return _DeepCopy;
        }
    }
}
