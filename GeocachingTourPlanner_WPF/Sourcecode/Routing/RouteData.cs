using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// All Geocaches that can somehow be reached in the Distancce limit from the Route.
        /// </summary>
        public List<GeocacheRoutingInformation> ReachableGeocaches { get; set; }
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
        /// Returns a readonly List of partial Routes
        /// </summary>
        public IReadOnlyList<PartialRoute> PartialRoutes { get { return partialRoutes.AsReadOnly(); } }
        /// <summary>
        /// Adds a partial Route to the List of Partial Routes
        /// </summary>
        /// <param name="PR"></param>
        public void AddPartialRoute(PartialRoute PR)
        {
            TotalDistance += PR.partialRoute.TotalDistance;
            TotalTime += PR.partialRoute.TotalTime;
            partialRoutes.Add(PR);
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
            foreach (PartialRoute PR in PartialRoutesToInsert)
            {
                TotalDistance += PR.partialRoute.TotalDistance;
                TotalTime += PR.partialRoute.TotalTime;
            }
            partialRoutes.InsertRange(partialRoutes.IndexOf(partialRouteToReplace), PartialRoutesToInsert);
            partialRoutes.Remove(partialRouteToReplace);
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
            partialRoutes.Remove(PR);
            PartialRoutesChangedEvent(this, null);
        }

        /// <summary>
        /// Removes all partial routes
        /// </summary>
        public void ClearPartialRoutes()
        {
            TotalDistance = 0;
            partialRoutes.Clear();
            PartialRoutesChangedEvent(this, null);
        }
        #endregion

        #region Waypoints
        /// <summary>
        /// All Waypoints on the Route. ONLY USE METHODS TO CHANGE LIST!
        /// </summary>
        public IReadOnlyList<Waypoint> Waypoints { get { return waypoints.AsReadOnly(); } }

        //TODO reevaluate if necessary, since Geocaches are contained in Waypoints 
        /*/// <summary>
        /// All Geocaches on the Route ONLY USE METHODS TO CHANGE LIST! TODO reevaluate if necessary, since contained in Waypoints 
        /// </summary>
        public List<Geocache> GeocachesOnRoute { get; set; }
        */

        /// <summary>
        /// Adds a Waypoint to the list of Waypoints and to the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void AddWaypointToEnd(Waypoint WP)
        {
            waypoints.Remove(WP);
            waypoints.Add(WP);
            if (WP.GetType() == typeof(Geocache))
            {
                //GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints += ((Geocache)WP).Rating;
                if (Profile != null)
                {
                    TotalTime += Profile.TimePerGeocache;
                }
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Adds a Waypoint to the beginning of the list of Waypoints and to the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void AddWaypointToBeginning(Waypoint WP)
        {
            waypoints.Remove(WP);
            waypoints.Insert(0, WP);
            if (WP.GetType() == typeof(Geocache))
            {
                //GeocachesOnRoute.Add((Geocache)WP);
                TotalPoints += ((Geocache)WP).Rating;
                if (Profile != null)
                {
                    TotalTime += Profile.TimePerGeocache;
                }
            }
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Removes a Waypoint from the list of Waypoints and from the List of Geocaches if it is a Geocache. Takes care of statistics
        /// </summary>
        /// <param name="WP"></param>
        public void RemoveWaypoint(Waypoint WP)
        {
            waypoints.Remove(WP);
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
            int OldIndex = waypoints.IndexOf(WP);
            waypoints.RemoveAt(OldIndex);
            waypoints.Insert(OldIndex - 1, WP);
            WaypointsChangedEvent(this, null);
        }

        /// <summary>
        /// Moves Waypoint downwards in List
        /// </summary>
        /// <param name="WP"></param>
        public void MoveWaypointDown(Waypoint WP)
        {
            int OldIndex = waypoints.IndexOf(WP);
            waypoints.RemoveAt(OldIndex);
            waypoints.Insert(OldIndex + 1, WP);
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
                                if (App.Geocaches.Count(x => x.GCCODE == reader.GetAttribute("GCCODE"))>0)
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
            WaypointsChangedEvent += (s, e) => { Fileoperations.Backup(Databases.Routes); };

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
            partialRoutes.Clear();
            //GeocachesOnRoute.Clear();
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
            ResetRouteData();

            foreach(Geocache GC in waypoints.Where(x => x.GetType() == typeof(Geocache)))
            {
                TotalPoints += GC.Rating;
                if (Profile != null)
                {
                    TotalTime += Profile.TimePerGeocache;
                }
            }

            foreach(PartialRoute PR in partialRoutes)
            {
                TotalDistance += PR.partialRoute.TotalDistance;
                TotalTime += PR.partialRoute.TotalTime;
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
