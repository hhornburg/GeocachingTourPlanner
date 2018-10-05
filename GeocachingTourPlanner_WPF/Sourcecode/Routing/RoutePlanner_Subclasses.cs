using GeocachingTourPlanner.Types;
using Itinero;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeocachingTourPlanner.Routing
{
    public partial class RoutePlanner
    {
		/// <summary>
		/// Structure that holds all routing Data
		/// </summary>
		public class RouteData
		{
			public SortableBindingList<PartialRoute> partialRoutes { get; set; } 
			public SortableBindingList<Waypoint> Waypoints { get; set; }
			/// <summary>
			/// Holds the geocaches that are on the route. To add a geocache ONLY use AddGeocacheOnRoute, since it adds the points of the geocache to the total points
			/// </summary>
			public List<Geocache> GeocachesOnRoute { get; set; }
			/// <summary>
			/// All Geocaches that can somehow be reached in the Distancce limit from the Route.
			/// </summary>
			public List<GeocacheRoutingInformation> ReachableGeocaches { get; set; }
			public Routingprofile Profile { get; set; }
			/// <summary>
			/// in meters
			/// </summary>
			public float TotalDistance { get; set; }
			/// <summary>
			/// in seconds, without geocaches
			/// </summary>
			public float TotalTime { get; set; }
			/// <summary>
			/// sum of all the ratings of the geocaches on the route
			/// </summary>
			public float TotalPoints { get; set; }

			public void AddGeocacheOnRoute(Geocache geocache)
			{
				GeocachesOnRoute.Add(geocache);
				Waypoints.Add(geocache);
				TotalPoints += geocache.Rating;
			}

			public RouteData()
			{
				partialRoutes = new SortableBindingList<PartialRoute>();
                partialRoutes.ListChanged += new ListChangedEventHandler(App.mainWindow.Map_RenewCurrentRoute);
                GeocachesOnRoute = new List<Geocache>();
				Waypoints = new SortableBindingList<Waypoint>();
				Waypoints.ListChanged += App.mainWindow.Waypoints_ListChanged;
				ReachableGeocaches = new List<GeocacheRoutingInformation>();
				foreach(Geocache GC in RemoveGeocachesWithNegativePoints(App.Geocaches.ToList()))
				{
					GeocacheRoutingInformation GCRI = new GeocacheRoutingInformation();
					GCRI.geocache = GC;
					ReachableGeocaches.Add(GCRI);
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
				foreach (Geocache geocache in GeocachesOnRoute)
				{
					_DeepCopy.AddGeocacheOnRoute(geocache);//No deeper copy needed, as geocaches won't be changed
				}
				_DeepCopy.Profile = Profile;//No deeper copy needed, as profile won't be changed
				_DeepCopy.TotalDistance = TotalDistance;
				_DeepCopy.TotalPoints = TotalPoints;
				_DeepCopy.TotalTime = TotalTime;
				return _DeepCopy;
			}
		}

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

		public class GeocacheRoutingInformation
		{
			/// <summary>
			/// Always use the position of the geocache for the shortest distance calculations
			/// </summary>
			public Geocache geocache { get; set; }

			/// <summary>
			/// In meters. Triangulation to the closest two Shapepoints
			/// </summary>
			public float EstimatedExtraDistance_InRoute
			{
				get { return DistanceToRoute_field; }
				set
				{
					DistanceToRoute_field = value;
					RoutingRating = geocache.Rating / (1 + Math.Min(EstimatedExtraDistance_NewRoute, EstimatedExtraDistance_InRoute));
				}
			}
			private float DistanceToRoute_field = -1;

			/// <summary>
			/// In meters. A guess how much longer the total route would be if this geocache was added
			/// </summary>
			public float EstimatedExtraDistance_NewRoute
			{
				get { return EstimatedExtraDistance_field; }
				set
				{
					EstimatedExtraDistance_field = value;
					RoutingRating = geocache.Rating / (1 + Math.Min(EstimatedExtraDistance_NewRoute, EstimatedExtraDistance_InRoute));
				}
			}
			private float EstimatedExtraDistance_field=-1;


			/// <summary>
			/// Rating how good it is to add the Geocache to the Route.
			/// </summary>
			/// <value>RoutingPoints = geocache.Rating / (1 + Min(EstimatedExtraDistance ,DistanceToRoute));</value>
			public float RoutingRating { get; set; }
			[XmlIgnore]
			public RouterPoint ResolvedCoordinates { get; set; }//Used so coordinates only have to be reoslved once

			public GeocacheRoutingInformation(Geocache geocache, float DistanceToRoute, float EstimatedExtraDistance, RouterPoint ResolvedCoordinates)
			{
				this.geocache = geocache;
				this.EstimatedExtraDistance_InRoute = DistanceToRoute;
				this.EstimatedExtraDistance_NewRoute = EstimatedExtraDistance;
				this.ResolvedCoordinates = ResolvedCoordinates;

				RoutingRating = geocache.Rating / (1 + EstimatedExtraDistance * DistanceToRoute);
			}

			public GeocacheRoutingInformation(Geocache geocache, float EstimatedExtraDistance, RouterPoint ResolvedCoordinates)
			{
				this.geocache = geocache;
				this.EstimatedExtraDistance_NewRoute = EstimatedExtraDistance;
				this.ResolvedCoordinates = ResolvedCoordinates;
			}
			/// <summary>
			/// Copies the attributes of the GeocacheRoutingInformation passed to a new GeocacheRoutingInformation
			/// </summary>
			/// <param name="ObjectToCopy"></param>
			public GeocacheRoutingInformation(GeocacheRoutingInformation ObjectToCopy)
			{
				geocache = ObjectToCopy.geocache;
				EstimatedExtraDistance_InRoute = ObjectToCopy.EstimatedExtraDistance_InRoute;
				EstimatedExtraDistance_NewRoute = ObjectToCopy.EstimatedExtraDistance_NewRoute;
				ResolvedCoordinates = ObjectToCopy.ResolvedCoordinates;
			}
			/// <summary>
			/// For serializtion only
			/// </summary>
			public GeocacheRoutingInformation() { }

			public override string ToString()
			{
				return geocache.ToString();
			}
		}
	}
}
