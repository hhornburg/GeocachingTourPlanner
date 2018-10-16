using GeocachingTourPlanner.Types;

namespace GeocachingTourPlanner.Routing
{
    /// <summary>
    /// Child class of WaypointRoutingInformation, provides information specific for Geocaches
    /// </summary>
    public class GeocacheRoutingInformation : WaypointRoutingInformation
    {
        /// <summary>
        /// Returns the Waypoint casted to a Geocache
        /// </summary>
        public Geocache Geocache { get { return (Geocache)Waypoint; } }

        /// <summary>
        /// Creates a new GeocacheRoutingInformation to the given Geocache
        /// </summary>
        /// <param name="GC"></param>
        public GeocacheRoutingInformation(Geocache GC) : base(GC) { }

        /// <summary>
        /// Rating how good it is to add the Geocache to the Route. (Best is many points in short distancce). raturns -1 if it doesn't know the distance
        /// </summary>
        public float GetRoutingRating(Waypoint Target)
        {
            if (Target != null && RoutesToWaypoints.ContainsKey(Target))
            {
                return ((Geocache)Waypoint).Rating / RoutesToWaypoints[Target].TotalDistance;
            }
            else
            {
                return -1;
            }
        }
    }
}
