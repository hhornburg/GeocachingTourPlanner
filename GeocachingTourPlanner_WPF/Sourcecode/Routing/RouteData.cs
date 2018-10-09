using GeocachingTourPlanner.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GeocachingTourPlanner.Routing
{
    //TODO proper encapsulating with possibility to XmlSerialize.
    /// <summary>
    /// Holds Routing Data
    /// </summary>
    public class RouteData
    {
        /// <summary>
        /// Occurs when the List of Partial Routes changes
        /// </summary>
        public event EventHandler PartialRoutesChangedEvent;
        /// <summary>
        /// Occurs when the List of Waypoints changes
        /// </summary>
        public event EventHandler WaypointsChangedEvent;

        /// <summary>
        /// All Geocaches that can somehow be reached in the Distancce limit from the Route.
        /// </summary>
        public List<GeocacheRoutingInformation> ReachableGeocaches { get; set; }
        /// <summary>
        /// The Active Routingprofile for this Route
        /// </summary>
        public Routingprofile Profile { get; set; }
        /// <summary>
        /// in meters
        /// </summary>
        public float TotalDistance { get; set; }
        /// <summary>
        /// in seconds, with geocaches
        /// </summary>
        public float TotalTime { get; set; }
        /// <summary>
        /// sum of all the ratings of the geocaches on the route
        /// </summary>
        public float TotalPoints { get; set; }

        #region PartialRoute
        /// <summary>
        /// Returns a readonly List of partial Routes
        /// </summary>
        public List<PartialRoute> PartialRoutes { get; set; }
        /// <summary>
        /// Adds a partial Route to the List of Partial Routes
        /// </summary>
        /// <param name="PR"></param>
        public void AddPartialRoute(PartialRoute PR)
        {
            TotalDistance += PR.partialRoute.TotalDistance;
            TotalTime += PR.partialRoute.TotalTime;
            PartialRoutes.Add(PR);
            PartialRoutesChangedEvent(this, null);
        }
        /// <summary>
        /// Replaces a Route with a List of routes
        /// </summary>
        /// <param name="partialRouteToReplace"></param>
        /// <param name="PartialRoutesToInsert"></param>
        public void ReplaceRoute(PartialRoute partialRouteToReplace, List<PartialRoute> PartialRoutesToInsert)
        {
            TotalDistance -= partialRouteToReplace.partialRoute.TotalDistance;
            TotalTime -= partialRouteToReplace.partialRoute.TotalTime;
            foreach(PartialRoute PR in PartialRoutesToInsert)
            {
                TotalDistance += PR.partialRoute.TotalDistance;
                TotalTime += PR.partialRoute.TotalTime;
            }
            PartialRoutes.InsertRange(PartialRoutes.IndexOf(partialRouteToReplace), PartialRoutesToInsert);
            PartialRoutes.Remove(partialRouteToReplace);
            PartialRoutesChangedEvent(this, null);
        }
        /// <summary>
        /// Adds a partial Route to the List of Partial Routes
        /// </summary>
        /// <param name="PR"></param>
        public void RemovePartialRoute(PartialRoute PR)
        {
            TotalDistance -= PR.partialRoute.TotalDistance;
            TotalTime -= PR.partialRoute.TotalTime;
            PartialRoutes.Remove(PR);
            PartialRoutesChangedEvent(this, null);
        }

        /// <summary>
        /// Removes all partial routes
        /// </summary>
        public void ClearPartialRoutes()
        {
            TotalDistance = 0;
            PartialRoutes.Clear();
            PartialRoutesChangedEvent(this, null);
        }
        #endregion

        #region Waypoints
        /// <summary>
        /// All Waypoints on the Route. ONLY USE METHODS TO CHANGE LIST!
        /// </summary>
        public List<Waypoint> Waypoints { get; set; }
        //TODO reevaluate if necessary, since Geocaches are contained in Waypoints 
        /// <summary>
        /// All Geocaches on the Route ONLY USE METHODS TO CHANGE LIST! TODO reevaluate if necessary, since contained in Waypoints 
        /// </summary>
        public List<Geocache> GeocachesOnRoute { get; set; }
        /// <summary>
        /// Adds a Waypoint to the list of Waypoints and to the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void AddWaypointToEnd(Waypoint WP)
        {
            Waypoints.Remove(WP);
            Waypoints.Add(WP);
            if (WP.GetType() == typeof(Geocache))
            {
                GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints += ((Geocache)WP).Rating;
                TotalTime += Profile.TimePerGeocache;
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Adds a Waypoint to the beginning of the list of Waypoints and to the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void AddWaypointToBeginning(Waypoint WP)
        {
            Waypoints.Remove(WP);
            Waypoints.Insert(0,WP);
            if (WP.GetType() == typeof(Geocache))
            {
                GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints += ((Geocache)WP).Rating;
                TotalTime += Profile.TimePerGeocache;
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Removes a Waypoint from the list of Waypoints and from the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void RemoveWaypoint(Waypoint WP)
        {
            Waypoints.Add(WP);
            if (WP.GetType() == typeof(Geocache))
            {
                GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints += ((Geocache)WP).Rating;
                TotalTime += Profile.TimePerGeocache;
            }
            WaypointsChangedEvent(this, null);
        }
        #endregion

        /// <summary>
        /// Structure that holds all routing Data
        /// </summary>
        public RouteData()
        {
            PartialRoutes = new List<PartialRoute>();
            //This Way they only get bound once
            PartialRoutesChangedEvent += App.mainWindow.Map_RenewCurrentRoute;
            PartialRoutesChangedEvent += App.mainWindow.RenewRouteInfo;
            GeocachesOnRoute = new List<Geocache>();
            Waypoints = new List<Waypoint>();
            //This Way they only get bound once
            WaypointsChangedEvent += App.mainWindow.Waypoints_ListChanged;
            ReachableGeocaches = new List<GeocacheRoutingInformation>();
            foreach (Geocache GC in RemoveGeocachesWithNegativePoints(App.Geocaches.ToList()))
            {
                GeocacheRoutingInformation GCRI = new GeocacheRoutingInformation();
                GCRI.geocache = GC;
                ReachableGeocaches.Add(GCRI);
            }
        }

        /// <summary>
        /// Empties PartialRoutes, GeocachesOnRoute, Waypoints and sets statistics to 0
        /// </summary>
        public void ResetRouteData()
        {
            PartialRoutes.Clear();
            GeocachesOnRoute.Clear();
            Waypoints.Clear();
            TotalDistance = 0;
            TotalPoints = 0;
            TotalTime = 0; ;
        }

        /// <summary>
        /// Creates a copy as deep as needed to have modify the copy without modifing the original route. Does not deep copy the geocaches for example, since this is unnecessary
        /// </summary>
        /// <returns></returns>
        public RouteData DeepCopy()
        {
            RouteData _DeepCopy = new RouteData();
            foreach (PartialRoute partialRoute in PartialRoutes)
            {
                _DeepCopy.PartialRoutes.Add(partialRoute.DeepCopy());
            }
            foreach (Geocache geocache in GeocachesOnRoute)
            {
                _DeepCopy.AddWaypointToEnd(geocache);//No deeper copy needed, as geocaches won't be changed
            }
            _DeepCopy.Profile = Profile;//No deeper copy needed, as profile won't be changed
            _DeepCopy.TotalDistance = TotalDistance;
            _DeepCopy.TotalPoints = TotalPoints;
            _DeepCopy.TotalTime = TotalTime;
            return _DeepCopy;
        }

        /// <summary>
		/// Returns new list with all geocaches that don't have a negtive rating
		/// </summary>
		/// <param name="AllGeocaches"></param>
		/// <returns></returns>
		private static List<Geocache> RemoveGeocachesWithNegativePoints(List<Geocache> AllGeocaches)
        {
            List<Geocache> UsableGeocaches = new List<Geocache>();
            Parallel.ForEach(AllGeocaches, GC =>
            {
                if (GC.Rating > 0)
                {
                    lock (UsableGeocaches)
                    {
                        UsableGeocaches.Add(GC);
                    }
                }
            });
            return UsableGeocaches;
        }
    }
}
