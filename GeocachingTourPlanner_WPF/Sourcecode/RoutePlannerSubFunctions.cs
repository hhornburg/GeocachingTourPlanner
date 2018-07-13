using GeocachingTourPlanner.Types;
using Itinero;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GeocachingTourPlanner.Routing
{
    public partial class RoutePlanner
    {
		private bool ResolveAndAddGeocachesToPartialRoutes(List<Geocache> geocaches)
		{
			//This way the Geocache can be added to multiple partial routes, but no multiple times to the same one
			for (int CurrentPartialrouteIndex = 0; CurrentPartialrouteIndex < CompleteRouteData.partialRoutes.Count; CurrentPartialrouteIndex++)
			{
				Parallel.ForEach(geocaches, GC =>
				{
					float ExtraDistance = GetEstimatedExtraDistance_NewRoute(CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].partialRoute, new Coordinate(GC.lat, GC.lon));

					//Only exclude those that definitely can't be reached
					if (ExtraDistance < CompleteRouteData.Profile.MaxDistance * 1000 - CompleteRouteData.TotalDistance)
					{
						//TODO Cache Routerpoints to improve performance when geocaches get added to multiple routes
						RouterPoint RouterPointOfGeocache = null;
						Result<RouterPoint> ResolveResult = Router1.TryResolve(CompleteRouteData.Profile.ItineroProfile.profile, GC.lat, GC.lon, SearchDistanceInMeters);
						if (!ResolveResult.IsError)
						{
							float DistanceToRoute = ExtraDistance_InRoute(CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].partialRoute, new Coordinate(GC.lat, GC.lon));
							RouterPointOfGeocache = ResolveResult.Value;
							lock (CompleteRouteData.partialRoutes)
							{
								CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].GeocachesInReach.Add(new GeocacheRoutingInformation(GC, DistanceToRoute, ExtraDistance, RouterPointOfGeocache));//Push the resoved location on
							}
						}
					}
				});
			}
			return true;
		}

		/// <summary>
		/// Returns new list with all geocaches that don't have a negtive rating
		/// </summary>
		/// <param name="AllGeocaches"></param>
		/// <returns></returns>
		private List<Geocache> RemoveGeocachesWithNegativePoints(List<Geocache> AllGeocaches)
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

		/// <summary>
		/// Returns the shortest triangulated extra distance to the route if the coordinates are added in between the closest shapepoint and it's neighbor where the coordinate is closer to.
		/// </summary>
		/// <param name="route"></param>
		/// <param name="coord"></param>
		/// <returns></returns>
		private float ExtraDistance_InRoute(Route route, Coordinate coord)
		{
			float MinDistance = -1;//-1, so it is known if no "highscore" has been set
			int ClosestShapePoint = -1;

			for (int k = 0; k < route.Shape.Length; k++)
			{
				float Distance = Coordinate.DistanceEstimateInMeter(route.Shape[k], coord);
				if (MinDistance < 0 || Distance < MinDistance)
				{
					MinDistance = Distance;
					ClosestShapePoint = k;
				}
			}

			float DistanceToNeighbor1 = Coordinate.DistanceEstimateInMeter(route.Shape[ClosestShapePoint + 1], coord);
			float DistanceToNeighbor2 = Coordinate.DistanceEstimateInMeter(route.Shape[ClosestShapePoint - 1], coord);

			if (DistanceToNeighbor1 > DistanceToNeighbor2)
			{
				return DistanceToNeighbor2 - Coordinate.DistanceEstimateInMeter(route.Shape[ClosestShapePoint - 1], route.Shape[ClosestShapePoint]);
			}
			else
			{
				return DistanceToNeighbor1 - Coordinate.DistanceEstimateInMeter(route.Shape[ClosestShapePoint + 1], route.Shape[ClosestShapePoint]);
			}
		}

		/// <summary>
		/// Returmns an Estimate of the added distance, if route is newly routed with the coordinates added
		/// </summary>
		/// <param name="route"></param>
		/// <param name="coord"></param>
		/// <returns></returns>
		private float GetEstimatedExtraDistance_NewRoute(Route route, Coordinate coord)
		{
			float TriangleLengthWithGeocache = Coordinate.DistanceEstimateInMeter(route.Shape[0], coord) + Coordinate.DistanceEstimateInMeter(route.Shape[route.Count() - 1], coord);
			float MaxCurrentTriangleLength = -1;//-1, so it is known if no "highscore" has been set

			for (int k = 1; k < route.Shape.Length - 1; k++)//Since Start and endpoint don't need to be used
			{
				float Distance = Coordinate.DistanceEstimateInMeter(route.Shape[0], route.Shape[k]) + Coordinate.DistanceEstimateInMeter(route.Shape[route.Count() - 1], route.Shape[k]);
				if (MaxCurrentTriangleLength < 0 || Distance > MaxCurrentTriangleLength)
				{
					MaxCurrentTriangleLength = Distance;
				}
			}
			if (MaxCurrentTriangleLength > TriangleLengthWithGeocache)//Thus route seems to be shorter
			{
				return 0;
			}
			else
			{
				return route.TotalDistance * TriangleLengthWithGeocache / MaxCurrentTriangleLength - route.TotalDistance;
			}

		}

		/// <summary>
		/// Calculates minimal distance of geocaches in reach of the old route to the new routes. In a second steps it replaces the old route with the new one and updates the distances
		/// </summary>
		/// <param name="CompleteRouteData"></param>
		/// <param name="NewPart1"></param>
		/// <param name="NewPart2"></param>
		/// <param name="IndexOfRouteToReplace"></param>
		private RouteData ReplaceRoute(RouteData CompleteRouteData, Route NewPart1, Route NewPart2, int IndexOfRouteToReplace)
		{
			List<GeocacheRoutingInformation> NewPart1Geocaches = new List<GeocacheRoutingInformation>();
			List<GeocacheRoutingInformation> OldRouteGeocaches1 = new List<GeocacheRoutingInformation>(CompleteRouteData.partialRoutes[IndexOfRouteToReplace].GeocachesInReach);
			List<GeocacheRoutingInformation> NewPart2Geocaches = new List<GeocacheRoutingInformation>();
			List<GeocacheRoutingInformation> OldRouteGeocaches2 = new List<GeocacheRoutingInformation>(CompleteRouteData.partialRoutes[IndexOfRouteToReplace].GeocachesInReach);
			Coordinate Startingpoint = CompleteRouteData.partialRoutes[IndexOfRouteToReplace].partialRoute.Shape[0];
			Coordinate Endpoint = CompleteRouteData.partialRoutes[IndexOfRouteToReplace].partialRoute.Shape[0];

			Thread Thread1 = new Thread(new ThreadStart(() =>
			{
				foreach (GeocacheRoutingInformation GC_Info in new List<GeocacheRoutingInformation>(OldRouteGeocaches1))
				{
					float minDistance = ExtraDistance_InRoute(NewPart1, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					float estimatedDistance = GetEstimatedExtraDistance_NewRoute(NewPart1, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					NewPart1Geocaches.Add(new GeocacheRoutingInformation(GC_Info.geocache, minDistance, estimatedDistance, GC_Info.ResolvedCoordinates));//Push the resolved location on
				}
			}));

			Thread Thread2 = new Thread(new ThreadStart(() =>
			{
				foreach (GeocacheRoutingInformation GC_Info in new List<GeocacheRoutingInformation>(OldRouteGeocaches2))
				{
					float minDistance = ExtraDistance_InRoute(NewPart1, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					float estimatedDistance = GetEstimatedExtraDistance_NewRoute(NewPart2, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					NewPart2Geocaches.Add(new GeocacheRoutingInformation(GC_Info.geocache, minDistance, estimatedDistance, GC_Info.ResolvedCoordinates));//Push the resolved location on
				}

			}));

			Thread1.Start();
			Thread2.Start();

			Thread1.Join();
			Thread2.Join();

			//Put the new parts in place of the old part
			Route RouteToReplace = CompleteRouteData.partialRoutes[IndexOfRouteToReplace].partialRoute;
			CompleteRouteData.partialRoutes.RemoveAt(IndexOfRouteToReplace);
			CompleteRouteData.partialRoutes.InsertRange(IndexOfRouteToReplace, new List<PartialRoute>()
			{
				new PartialRoute(NewPart1, NewPart1Geocaches),
				new PartialRoute(NewPart2, NewPart2Geocaches)
			});

			CompleteRouteData.TotalDistance -= RouteToReplace.TotalDistance;
			CompleteRouteData.TotalDistance += NewPart1.TotalDistance;
			CompleteRouteData.TotalDistance += NewPart2.TotalDistance;

			CompleteRouteData.TotalTime -= RouteToReplace.TotalTime;
			CompleteRouteData.TotalTime += NewPart1.TotalTime;
			CompleteRouteData.TotalTime += NewPart2.TotalTime;

			return CompleteRouteData;

		}

		/// <summary>
		/// Calculates the routes from the startpoint of the RouteToInsertIn to the ResolvedCoordinates and from there to the Endpoint
		/// </summary>
		/// <param name="CompleteRouteData"></param>
		/// <param name="RouteToInsertIn"></param>
		/// <param name="ResolvedCoordinates"></param>
		/// <returns></returns>
		private Result<Tuple<Route, Route>> GetPartialRoutes(RouteData CompleteRouteData, Route RouteToInsertIn, RouterPoint ResolvedCoordinates)
		{
			Route NewPart1 = null;
			Route NewPart2 = null;

			Result<Route> RouteCalculationResult1 = Router1.TryCalculate(CompleteRouteData.Profile.ItineroProfile.profile, Router1.Resolve(CompleteRouteData.Profile.ItineroProfile.profile, RouteToInsertIn.Shape[0]), ResolvedCoordinates);
			if (!RouteCalculationResult1.IsError)
			{
				NewPart1 = RouteCalculationResult1.Value;

				Result<Route> RouteCalculationResult2 = Router2.TryCalculate(CompleteRouteData.Profile.ItineroProfile.profile, ResolvedCoordinates, Router2.Resolve(CompleteRouteData.Profile.ItineroProfile.profile, RouteToInsertIn.Shape[RouteToInsertIn.Shape.Length - 1]));
				if (!RouteCalculationResult2.IsError)
				{
					NewPart2 = RouteCalculationResult2.Value;
					FailedRouteCalculations--;//To make sure error isn't thrown when x geocaches create error, but only if there are more geocaches in row that cause error than not
				}
				//Resolving error of Item on Route shouldn't happen
				else
				{
					FailedRouteCalculations++;
					return new Result<Tuple<Route, Route>>("RouteCalculationFailed");
				}
			}
			else
			{
				FailedRouteCalculations++;
				return new Result<Tuple<Route, Route>>("RouteCalculationFailed");
			}

			return new Result<Tuple<Route, Route>>(new Tuple<Route, Route>(NewPart1, NewPart2));
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
	}
}
