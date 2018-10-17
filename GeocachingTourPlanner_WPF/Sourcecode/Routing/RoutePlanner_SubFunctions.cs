using GeocachingTourPlanner.Types;
using Itinero;
using Itinero.LocalGeo;
using System;
using System.Windows;

namespace GeocachingTourPlanner.Routing
{
    /// <summary>
    /// Provides Methods for Route Calculation
    /// </summary>
    public partial class RoutePlanner
    {
        #region Route Operations

        private Result<PartialRoute> CalculateRoute(Waypoint From, Waypoint To)
        {
            RouterPoint From_ResolvedCoordinates;
            RouterPoint To_ResolvedCoordinates;

            if (RoutingCache.ContainsKey(From) && RoutingCache[From].RoutesToWaypoints.ContainsKey(To))
            {
                return new Result<PartialRoute>(new PartialRoute(RoutingCache[From].RoutesToWaypoints[To], From, To));
            }
            else
            {
                if (RoutingCache.ContainsKey(From) && RoutingCache[From].ResolvedCoordinates != null)
                {
                    From_ResolvedCoordinates = RoutingCache[From].ResolvedCoordinates;
                }
                else
                {
                    Result<RouterPoint> ResolveResult = router.TryResolve(CompleteRouteData.Profile.ItineroProfile.profile, new Coordinate(From.lat, From.lon), SearchDistanceInMeters);
                    if (ResolveResult.IsError)
                    {
                        CompleteRouteData.RemoveWaypoint(From);
                        return new Result<PartialRoute>("Resolving failed on:" + From.ToString() + "\nRemoved waypoint.");
                    }
                    else
                    {
                        From_ResolvedCoordinates = ResolveResult.Value;
                        if (RoutingCache.ContainsKey(From))
                        {
                            RoutingCache[From].ResolvedCoordinates = From_ResolvedCoordinates;
                        }
                        else
                        {
                            RoutingCache[From] = new WaypointRoutingInformation(From) { ResolvedCoordinates = From_ResolvedCoordinates };
                        }
                    }
                }

                if (RoutingCache.ContainsKey(To) && RoutingCache[To].ResolvedCoordinates != null)
                {
                    To_ResolvedCoordinates = RoutingCache[To].ResolvedCoordinates;
                }
                else
                {
                    Result<RouterPoint> ResolveResult = router.TryResolve(CompleteRouteData.Profile.ItineroProfile.profile, new Coordinate(To.lat, To.lon),SearchDistanceInMeters);
                    if (ResolveResult.IsError)
                    {
                        CompleteRouteData.RemoveWaypoint(To);
                        return new Result<PartialRoute>("Resolving failed on:" + To.ToString() + "\nRemoved waypoint.");
                    }
                    else
                    {
                        To_ResolvedCoordinates = ResolveResult.Value;
                        if (RoutingCache.ContainsKey(To))
                        {
                            RoutingCache[To].ResolvedCoordinates = To_ResolvedCoordinates;
                        }
                        else
                        {
                            RoutingCache[To] = new WaypointRoutingInformation(From) { ResolvedCoordinates = To_ResolvedCoordinates };
                        }
                    }
                }

                Result<Route> RouteCalculationResult1 = router.TryCalculate(CompleteRouteData.Profile.ItineroProfile.profile, From_ResolvedCoordinates, To_ResolvedCoordinates);
                if (RouteCalculationResult1.IsError)
                {
                    CompleteRouteData.RemoveWaypoint(To); //Since navigating to the start worked in all cases except if it is the first one
                    FailedRouteCalculations++;
                    return new Result<PartialRoute>("Route calculation failed between " + From.ToString() + " and " + To.ToString() + "\nRemoved waypoint at the end.");
                }
                else
                {
                    Route route = RouteCalculationResult1.Value;
                    //No checking needed if Key exists, since it either existed befre or has been added after resolving
                    RoutingCache[From].RoutesToWaypoints[To] = route;
                    return new Result<PartialRoute>(new PartialRoute(RoutingCache[From].RoutesToWaypoints[To], From, To));
                }
            }
        }

        private bool CheckIfIsland()
        {
            if (FailedRouteCalculations >= FailedRouteCalculationsLimit)
            {
                MessageBox.Show("Couldn't properly calculate route due to a probable island. Please select different Start/Endpoints", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
