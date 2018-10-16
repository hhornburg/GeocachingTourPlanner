using GeocachingTourPlanner.Types;
using Itinero;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GeocachingTourPlanner.Routing
{
    /// <summary>
    /// Holds Information to speed up Routing
    /// </summary>
    public class WaypointRoutingInformation
    {
        /// <summary>
        /// The Waypoint of this WaypointRoutingInformation
        /// </summary>
        public Waypoint Waypoint { get; protected set; }
        /// <summary>
        /// Saves the Routerpint of this Waypoint
        /// </summary>
        public RouterPoint ResolvedCoordinates { get; set; }
        /// <summary>
        /// Holds all Routes Calculated from this Waypoint
        /// </summary>
        public ConcurrentDictionary<Waypoint, Route> RoutesToWaypoints {get;set;}

        /// <summary>
        /// Creates a new Instance of WaypointRoutingInformation for the given Waypoint
        /// </summary>
        /// <param name="WP"></param>
        public WaypointRoutingInformation(Waypoint WP)
        {
            Waypoint = WP;
            RoutesToWaypoints = new ConcurrentDictionary<Waypoint, Route>();
        }
        
    }
}
