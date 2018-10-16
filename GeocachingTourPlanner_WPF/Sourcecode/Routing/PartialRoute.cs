using GeocachingTourPlanner.Types;
using Itinero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner.Routing
{
    /// <summary>
    /// Wraps around a actual Route to provide Reachable Geocaches
    /// </summary>
    public class PartialRoute
    {
        /// <summary>
        /// The actual Route of the partialRoute
        /// </summary>
        public Route Route { get; }
        /// <summary>
        /// geocaches that are in reach from this partial route. NOT THOSE ON THE ROUTE
        /// </summary>
        public List<Geocache> ReachableGeocaches { get; set; }
        /// <summary>
        /// Startpoint of partialRoute
        /// </summary>
        public Waypoint From { get; }
        /// <summary>
        /// Endpoint of partialRoute
        /// </summary>
        public Waypoint To { get; }

        /// <summary>
        /// Creates a partialRoute
        /// </summary>
        /// <param name="partialRoute"></param>
        /// <param name="From"></param>
        /// <param name="To"></param>
        public PartialRoute(Route partialRoute, Waypoint From, Waypoint To)
        {
            this.Route = partialRoute;
            this.From = From;
            this.To = To;
            ReachableGeocaches = new List<Geocache>();
        }

        /// <summary>
        /// Returns a copy, that created an independent List of reachable Geocaches
        /// </summary>
        /// <returns></returns>
        public PartialRoute DeepCopy()
        {
            PartialRoute partialRoute = new PartialRoute(this.Route, From, To);
            foreach(Geocache GC in ReachableGeocaches)
            {
                partialRoute.ReachableGeocaches.Add(GC);
            }
            return partialRoute;
        }
    }
}
