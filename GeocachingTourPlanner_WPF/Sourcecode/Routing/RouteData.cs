using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Types;
using Itinero;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace GeocachingTourPlanner.Routing
{
    //TODO proper encapsulating with possibility to XmlSerialize.
    /// <summary>
    /// Holds Routing Data
    /// </summary>
    public class RouteData : IXmlSerializable
    {
        private List<PartialRoute> partialRoutes = new List<PartialRoute>();
        private List<Waypoint> waypoints = new List<Waypoint>();
        private string routingprofile;

        /// <summary>
        /// Occurs when the List of Partial Routes changes
        /// </summary>
        public event EventHandler PartialRoutesChangedEvent;
        /// <summary>
        /// Occurs when the List of Waypoints changes
        /// </summary>
        public event EventHandler WaypointsChangedEvent;

        /// <summary>
        /// The Active Routingprofile for this Route
        /// </summary>
        public Routingprofile Profile { get => GetRoutingprofile(routingprofile); set => routingprofile = value.Name; }

        private Routingprofile GetRoutingprofile(string RP_Name)
        {
            if (App.Routingprofiles.Count(x => x.Name == RP_Name) > 0)
            {
                return App.Routingprofiles.First(x => x.Name == RP_Name);
            }
            else
            {
                return null;
            }
        }

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
        /// Lock for the List of PartialRoutes
        /// </summary>
        public readonly object PartialRouteLocker = new object();
        /// <summary>
        /// Returns a readonly List of partial Routes
        /// </summary>
        public IReadOnlyList<PartialRoute> PartialRoutes { get { return partialRoutes.AsReadOnly(); } }
        /// <summary>
        /// Adds a partial Route to the List of Partial Routes
        /// </summary>
        /// <param name="partialRoute"></param>
        public void AddPartialRouteToEnd(PartialRoute partialRoute)
        {
            TotalDistance += partialRoute.Route.TotalDistance;
            TotalTime += partialRoute.Route.TotalTime;
            lock (PartialRouteLocker)
            {
                partialRoutes.Add(partialRoute);

                //Syncing Route From/To and Waypoints
                int indexOfpartialRoute = partialRoutes.IndexOf(partialRoute);
                if (waypoints[indexOfpartialRoute] != partialRoute.From)
                {
                    throw new Exception("Waypoint in List and partialRoute.From don't match");
                }
                if (waypoints[indexOfpartialRoute + 1] != partialRoute.To)
                {
                    InsertWaypoint(indexOfpartialRoute + 1, partialRoute.To);
                }
            }
            UpdateReachableGeocaches(partialRoute);
            PartialRoutesChangedEvent(this, null);
        }
        /// <summary>
        /// Replaces a Route with a List of routes
        /// </summary>
        /// <param name="partialRouteToReplace"></param>
        /// <param name="PartialRoutesToInsert"></param>
        public void ReplaceRoute(PartialRoute partialRouteToReplace, List<PartialRoute> PartialRoutesToInsert)
        {
            TotalDistance -= partialRouteToReplace.Route.TotalDistance;
            TotalTime -= partialRouteToReplace.Route.TotalTime;
            foreach (PartialRoute PR in PartialRoutesToInsert)
            {
                TotalDistance += PR.Route.TotalDistance;
                TotalTime += PR.Route.TotalTime;
            }
            lock (PartialRouteLocker)
            {
                partialRoutes.InsertRange(partialRoutes.IndexOf(partialRouteToReplace), PartialRoutesToInsert);
                partialRoutes.Remove(partialRouteToReplace);
                foreach (PartialRoute partialRoute in PartialRoutesToInsert)
                {
                    int indexOfpartialRoute = partialRoutes.IndexOf(partialRoute);
                    if (waypoints[indexOfpartialRoute] != partialRoute.From)
                    {
                        throw new Exception("Waypoint in List and partialRoute.From don't match");
                    }
                    if (partialRoute != PartialRoutesToInsert.Last())
                    {
                        InsertWaypoint(indexOfpartialRoute + 1, partialRoute.To);
                    }
                }
            }
            foreach (PartialRoute partialRoute in PartialRoutesToInsert)
            {
                UpdateReachableGeocaches(partialRoute);
            }
            PartialRoutesChangedEvent(this, null);
        }
        /// <summary>
        /// Adds a partial Route to the List of Partial Routes
        /// </summary>
        /// <param name="PR"></param>
        public void RemovePartialRoute(PartialRoute PR)
        {
            TotalDistance -= PR.Route.TotalDistance;
            TotalTime -= PR.Route.TotalTime;
            lock (PartialRouteLocker)
            {
                partialRoutes.Remove(PR);
            }
            PartialRoutesChangedEvent(this, null);
        }

        /// <summary>
        /// Removes all partial routes
        /// </summary>
        public void ClearPartialRoutes()
        {
            partialRoutes.Clear();
            RecalculateRouteDataStatistics();
            PartialRoutesChangedEvent(this, null);
        }
        #endregion

        #region Waypoints
        /// <summary>
        /// Lock for the List of Waypoints
        /// </summary>
        public readonly object WaypointsLocker = new object();
        /// <summary>
        /// All Waypoints on the Route. ONLY USE METHODS TO CHANGE LIST!
        /// </summary>
        public IReadOnlyList<Waypoint> Waypoints { get { return waypoints.AsReadOnly(); } }

        /// <summary>
        /// Adds a Waypoint to the list of Waypoints and to the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void AddWaypointToEnd(Waypoint WP)
        {
            lock (WaypointsLocker)
            {
                waypoints.Add(WP);
            }
            if (WP.GetType() == typeof(Geocache))
            {
                //GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints += ((Geocache)WP).Rating;
                if (Profile != null)
                {
                    TotalTime += Profile.TimePerGeocache;
                }
            }
            if (WP.GetType() == typeof(Geocache))
            {
                RemoveGeocacheFromAllReachableGeocaches((Geocache)WP);
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Adds a Waypoint to the beginning of the list of Waypoints and to the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void AddWaypointToBeginning(Waypoint WP)
        {
            lock (WaypointsLocker)
            {
                waypoints.Insert(0, WP);
            }
            if (WP.GetType() == typeof(Geocache))
            {
                //GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints += ((Geocache)WP).Rating;
                if (Profile != null)
                {
                    TotalTime += Profile.TimePerGeocache;
                }
            }
            if (WP.GetType() == typeof(Geocache))
            {
                RemoveGeocacheFromAllReachableGeocaches((Geocache)WP);
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Removes a Waypoint from the list of Waypoints and from the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void RemoveWaypoint(Waypoint WP)
        {
            lock (WaypointsLocker)
            {
                waypoints.Remove(WP);
            }
            if (WP.GetType() == typeof(Geocache))
            {
                //GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints -= ((Geocache)WP).Rating;
                if (Profile != null)
                {
                    TotalTime -= Profile.TimePerGeocache;
                }
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Moves Waypoint upwards in List
        /// </summary>
        /// <param name="WP"></param>
        public void MoveWaypointUp(Waypoint WP)
        {
            lock (WaypointsLocker)
            {
                int OldIndex = waypoints.IndexOf(WP);
                waypoints.RemoveAt(OldIndex);
                waypoints.Insert(OldIndex - 1, WP);
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Moves Waypoint downwards in List
        /// </summary>
        /// <param name="WP"></param>
        public void MoveWaypointDown(Waypoint WP)
        {
            lock (WaypointsLocker)
            {
                int OldIndex = waypoints.IndexOf(WP);
                waypoints.RemoveAt(OldIndex);
                waypoints.Insert(OldIndex + 1, WP);
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Returns Index of waypoint
        /// </summary>
        /// <param name="WP"></param>
        /// <returns></returns>
        public int IndexOfWaypoint(Waypoint WP)
        {
            return waypoints.IndexOf(WP);
        }

        /// <summary>
        /// Inserts Waypoint at the given Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="WP"></param>
        public void InsertWaypoint(int index, Waypoint WP)
        {
            lock (WaypointsLocker)
            {
                waypoints.Insert(index, WP);
            }
            if (WP.GetType() == typeof(Geocache))
            {
                //GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints += ((Geocache)WP).Rating;
                if (Profile != null)
                {
                    TotalTime += Profile.TimePerGeocache;
                }
            }
            if (WP.GetType() == typeof(Geocache))
            {
                RemoveGeocacheFromAllReachableGeocaches((Geocache)WP);
            }
            WaypointsChangedEvent(this, null);
        }
        #endregion

        /// <summary>
        /// For serialization
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Only writes profile and waypoints. the rest has to be recalculated
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            if (Profile != null)
            {
                writer.WriteElementString("Profile", Profile.Name);
            }
            writer.WriteStartElement("Waypoints");
            foreach (Waypoint WP in waypoints)
            {
                if (WP.GetType() == typeof(Geocache))
                {
                    writer.WriteStartElement("Geocache");
                    writer.WriteAttributeString("GCCODE", ((Geocache)WP).GCCODE);
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteStartElement("Waypoint");
                    writer.WriteAttributeString("lat", WP.lat.ToString("G"));
                    writer.WriteAttributeString("lon", WP.lon.ToString("G"));
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            bool RouteDataIsEmptyElement = reader.IsEmptyElement; //
            reader.ReadStartElement();
            if (!RouteDataIsEmptyElement) // (1)
            {
                if (reader.LocalName == "Profile")
                {
                    routingprofile = reader.ReadElementString("Profile");
                }
                bool WaypointsIsEmptyElement = reader.IsEmptyElement;
                if (reader.LocalName == "Waypoints")
                {
                    reader.ReadStartElement();//Reads Waypoints tag
                    if (!WaypointsIsEmptyElement)
                    {
                        while (reader.MoveToContent() == XmlNodeType.Element && (reader.LocalName == "Geocache" || reader.LocalName == "Waypoint"))
                        {
                            if (reader.LocalName == "Geocache")
                            {
                                if (App.Geocaches.Count(x => x.GCCODE == reader.GetAttribute("GCCODE")) > 0)
                                {
                                    AddWaypointToEnd(App.Geocaches.First(x => x.GCCODE == reader.GetAttribute("GCCODE")));
                                }
                            }
                            else
                            {
                                AddWaypointToEnd(new Waypoint(float.Parse(reader.GetAttribute("lat")), float.Parse(reader.GetAttribute("lon"))));
                            }
                            reader.ReadStartElement();
                        }
                        reader.ReadEndElement();//Reads end of Waypoints tag
                    }
                }
                reader.ReadEndElement();//Only read it if RouteData is not empty
            }
        }

        /// <summary>
        /// Structure that holds all routing Data
        /// </summary>
        public RouteData()
        {
            //This Way they only get bound once
            PartialRoutesChangedEvent += App.mainWindow.Map_RenewCurrentRoute;
            PartialRoutesChangedEvent += App.mainWindow.RenewRouteInfo;
            WaypointsChangedEvent += App.mainWindow.Waypoints_ListChanged;
            WaypointsChangedEvent += App.mainWindow.RenewRouteInfo;
            WaypointsChangedEvent += App.mainWindow.Map_RenewWaypointLayer;
            WaypointsChangedEvent += (s, e) => { Fileoperations.Backup(Databases.Routes); };
        }

        /// <summary>
        /// Empties PartialRoutes, GeocachesOnRoute, Waypoints and sets statistics to 0
        /// </summary>
        public void ResetRouteData()
        {
            partialRoutes.Clear();
            waypoints.Clear();
            TotalDistance = 0;
            TotalPoints = 0;
            TotalTime = 0; ;
        }

        /// <summary>
        /// Recalculates TotalDistance, TotalTime and TotalPoints
        /// </summary>
        public void RecalculateRouteDataStatistics()
        {
            TotalDistance = 0;
            TotalPoints = 0;
            TotalTime = 0;

            foreach (Geocache GC in waypoints.Where(x => x.GetType() == typeof(Geocache)))
            {
                TotalPoints += GC.Rating;
                if (Profile != null)
                {
                    TotalTime += Profile.TimePerGeocache;
                }
            }

            foreach (PartialRoute PR in partialRoutes)
            {
                TotalDistance += PR.Route.TotalDistance;
                TotalTime += PR.Route.TotalTime;
            }
        }

        /// <summary>
        /// Creates a copy as deep as needed to have modify the copy without modifing the original route. Does not deep copy the geocaches for example, since this is unnecessary
        /// </summary>
        /// <returns></returns>
        public RouteData DeepCopy()
        {
            RouteData _DeepCopy = new RouteData();
            foreach (PartialRoute partialRoute in partialRoutes)
            {
                _DeepCopy.partialRoutes.Add(partialRoute.DeepCopy());
            }
            /*
            foreach (Geocache geocache in GeocachesOnRoute)
            {
                _DeepCopy.AddWaypointToEnd(geocache);//No deeper copy needed, as geocaches won't be changed
            }
            */
            _DeepCopy.Profile = Profile;//No deeper copy needed, as profile won't be changed
            _DeepCopy.TotalDistance = TotalDistance;
            _DeepCopy.TotalPoints = TotalPoints;
            _DeepCopy.TotalTime = TotalTime;
            return _DeepCopy;
        }

        /// <summary>
        /// Updates the reachable Geocaches in all partial routes
        /// </summary>
        public void UpdateReachableGeocaches()
        {
            List<Geocache> WellEnoughRatedGeocaches = App.Geocaches.Where(x => (x.Rating > App.DB.MinAllowedRating && x.Rating > 0)).ToList();

            foreach (PartialRoute partialRoute in partialRoutes)
            {
                UpdateReachableGeocaches(partialRoute);
            }
        }

        /// <summary>
        /// Updates the reachable Geocaches in this specific partial route.
        /// </summary>
        public void UpdateReachableGeocaches(PartialRoute partialRoute)
        {
            List<Geocache> WellEnoughRatedGeocaches = App.Geocaches.Where(x => (x.Rating > App.DB.MinAllowedRating && x.Rating > 0)).ToList();
            partialRoute.ReachableGeocaches.Clear();

            foreach (Geocache GC in WellEnoughRatedGeocaches)
            {
                if (!waypoints.Contains(GC))
                {
                    //First, all Geocaches are added, that are reachable from the End and Beginning when the Route is replaced.
                    if (Coordinate.DistanceEstimateInMeter(new Coordinate(partialRoute.From.lat, partialRoute.From.lon), new Coordinate(GC.lat, GC.lon)) < (Profile.MaxDistance - TotalDistance + partialRoute.Route.TotalDistance) / 2)
                    {
                        partialRoute.ReachableGeocaches.Add(GC);
                    }
                    else if (Coordinate.DistanceEstimateInMeter(new Coordinate(partialRoute.To.lat, partialRoute.To.lon), new Coordinate(GC.lat, GC.lon)) < (Profile.MaxDistance - TotalDistance + partialRoute.Route.TotalDistance) / 2)
                    {
                        partialRoute.ReachableGeocaches.Add(GC);
                    }
                    else
                    {
                        //In a second step, all Geocaches are added, that are reachable, when added to the route. Here only Geocaches should be added, when a large detour is made by the route
                        float MinDistance = GetMinDistanceToRoute(partialRoute.Route, GC);

                        if (MinDistance < (Profile.MaxDistance - TotalDistance) / 2)
                        {
                            partialRoute.ReachableGeocaches.Add(GC);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes the given Geocache from all ReachableGeocache Lists 
        /// </summary>
        /// <param name="geocache"></param>
        public void RemoveGeocacheFromAllReachableGeocaches(Geocache geocache)
        {
            lock (PartialRouteLocker)
            {
                foreach(PartialRoute partialRoute in partialRoutes)
                {
                    partialRoute.ReachableGeocaches.Remove(geocache);
                }
            }
        }
        /// <summary>
        /// Returns minimal Distance of the Waypoint to the any ShapePoint of the Route
        /// </summary>
        /// <param name="route"></param>
        /// <param name="waypoint"></param>
        /// <returns></returns>
        public static float GetMinDistanceToRoute(Route route, Waypoint waypoint)
        {
            float MinDistance = -1;//-1, so it is known if no "highscore" has been set
            int ClosestShapePoint = -1;

            //TODO Evaluate CPU cost
            for (int k = 0; k < route.Shape.Length; k++)
            {
                float Distance = Coordinate.DistanceEstimateInMeter(route.Shape[k], new Coordinate(waypoint.lat, waypoint.lon));
                if (MinDistance < 0 || Distance < MinDistance)
                {
                    MinDistance = Distance;
                    ClosestShapePoint = k;
                }
            }

            return MinDistance;
        }
    }
}
