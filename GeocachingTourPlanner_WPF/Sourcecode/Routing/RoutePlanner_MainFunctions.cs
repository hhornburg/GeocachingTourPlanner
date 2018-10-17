using GeocachingTourPlanner.Types;
using Itinero;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace GeocachingTourPlanner.Routing
{
    public partial class RoutePlanner
    {
        /// <summary>
        /// Name of the Route
        /// </summary>
        public string Name;
        /// <summary>
        /// returns Routeplanner.Name
        /// </summary>
        /// <returns>RoutePlanner.Name</returns>
        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// For serialization only
        /// </summary>
        public RoutePlanner() { }
        /// <summary>
        /// Holds the actual route Data
        /// </summary>
        public RouteData CompleteRouteData = new RouteData();
        private ConcurrentDictionary<Waypoint, WaypointRoutingInformation> RoutingCache;

        /// <summary>
        /// Used to detect islands
        /// </summary>
        int FailedRouteCalculations = 0;

        #region hard coded constants
        /// <summary>
        /// Max number of allowed failed calculations
        /// </summary>
        int FailedRouteCalculationsLimit = 10;
        float SearchDistanceInMeters = 200F;
        #endregion

        Router router = null;
        Router Router2 = null;

        public RoutePlanner(string Name)
        {
            #region Create Routers
            this.Name = Name;
            if (App.RouterDB.IsEmpty)
            {
                MessageBox.Show("Import or set RouterDB before creating route!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //FIX App.mainWindow.UseWaitCursor = false;
                return;
            }
            //One for every thread
            router = new Router(App.RouterDB);
            Router2 = new Router(App.RouterDB);
            #endregion
        }

        /// <summary>
        /// Takes the Waypoints from the CompleteRouteData and calculates the route that goes through the Waypoints in their specified order.
        /// </summary>
        /// <returns></returns>
        public bool CalculateDirectRoute()
        {
            if (router == null || Router2 == null)
            {
                //One for every thread
                router = new Router(App.RouterDB);
                Router2 = new Router(App.RouterDB);
            }

            if (CompleteRouteData.Profile == null)
            {
                MessageBox.Show("Please select a Routingprofile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!App.RoutingCache.ContainsKey(CompleteRouteData.Profile.ItineroProfile.profile))
            {
                App.RoutingCache[CompleteRouteData.Profile.ItineroProfile.profile] = new ConcurrentDictionary<Waypoint, WaypointRoutingInformation>();
            }
            RoutingCache = App.RoutingCache[CompleteRouteData.Profile.ItineroProfile.profile];

            CompleteRouteData.ClearPartialRoutes();

            //TODO Parallel.FOR
            lock (CompleteRouteData.WaypointsLocker)
            {
                for (int i = 0; i < CompleteRouteData.Waypoints.Count - 1; i++)//minus one, since No route needs to be calculated from last point
                {
                    Result<PartialRoute> routeResult = CalculateRoute(CompleteRouteData.Waypoints[i], CompleteRouteData.Waypoints[i + 1]);
                    if (routeResult.IsError)
                    {
                        MessageBox.Show(routeResult.ErrorMessage);
                        return false;
                    }
                    else
                    {
                        CompleteRouteData.AddPartialRouteToEnd(routeResult.Value);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Adds Geocaches that lie directly on the Route, calls overloaded Method recursively for all partial Routes
        /// </summary>
        public void AddGeocachesDirectlyOnRoute()
        {
            App.mainWindow.UpdateStatus("Started adding Geocaches directly on Route");
            lock (CompleteRouteData.PartialRouteLocker)
            {
                foreach (PartialRoute PR in CompleteRouteData.PartialRoutes)
                {
                    new Thread(new ThreadStart(() =>
                    {
                        AddGeocachesDirectlyOnRoute(PR);
                    })).Start();
                }
            }
        }

        /// <summary>
        /// Adds geocaches directly to the route, recursively calls itself
        /// </summary>
        /// <param name="partialRoute"></param>
        public void AddGeocachesDirectlyOnRoute(PartialRoute partialRoute)
        {
            foreach (Geocache Geocache in partialRoute.ReachableGeocaches)
            {
                if (RouteData.GetMinDistanceToRoute(partialRoute.Route, Geocache) < App.DB.OnRouteDistanceLimit)
                {
                    Result<PartialRoute> RouteResult1 = CalculateRoute(partialRoute.From, Geocache);
                    if (RouteResult1.IsError)
                    {
                        return;
                    }
                    PartialRoute partialRoute1 = RouteResult1.Value;
                    Result<PartialRoute> RouteResult2 = CalculateRoute(Geocache, partialRoute.To);
                    if (RouteResult2.IsError)
                    {
                        return;
                    }
                    PartialRoute partialRoute2 = RouteResult2.Value;
                    if (partialRoute1.Route.TotalDistance + partialRoute2.Route.TotalDistance < partialRoute.Route.TotalDistance + App.DB.OnRouteDistanceLimit)
                    {
                        CompleteRouteData.ReplaceRoute(partialRoute, new List<PartialRoute>() { partialRoute1, partialRoute2 });
                        new Thread(new ThreadStart(() =>
                        {
                            AddGeocachesDirectlyOnRoute(partialRoute1);
                        })).Start();
                        new Thread(new ThreadStart(() =>
                        {
                            AddGeocachesDirectlyOnRoute(partialRoute2);
                        })).Start();
                        break;//Since the current partialRoute is removed
                    }
                }
            }
        }

        /// <summary>
        /// Takes the current Waypoints and uses itinero to optmize the resulting route
        /// </summary>
        /// <returns></returns>
        public bool OptimizeRoute()
        {
            //TODO Route optimization (travelling salesman problem)
            return false;
        }

        private void AddBestReachableGeocaches(List<Geocache> GeocachesToConsider)
        {

        }
    }
}
