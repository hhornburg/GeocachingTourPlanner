using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Itinero;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
	class Tourplanning
	{
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

		Router Router1 = null;
		Router Router2 = null;


		/// <summary>
		/// Returns null if calculation fails
		/// </summary>
		/// <param name="router"></param>
		/// <param name="profile"></param>
		/// <param name=""></param>
		/// <returns></returns>
		public void GetRoute_Recursive(Routingprofile profile, List<Geocache> AllGeocaches, Coordinate Startpoint, Coordinate Endpoint, List<Geocache> GeocachesToInclude)
		{
			/// <summary>
			/// Holds all information on the route. 
			/// </summary>
			RouteData CompleteRouteData = new RouteData();

			RouterPoint Startpoint_RP;
			RouterPoint Endpoint_RP;
			Route InitialRoute;
			List<Geocache> GeocachesNotAlreadyUsed = new List<Geocache>();
			CompleteRouteData.Profile = profile;

			#region Create Routers
			if (Program.RouterDB.IsEmpty)
			{
				MessageBox.Show("Import or set RouterDB before creating route!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.UseWaitCursor = false;
				return;
			}
			//One for every thread
			Router1 = new Router(Program.RouterDB);
			Router2 = new Router(Program.RouterDB);
			#endregion

			GeocachesNotAlreadyUsed = RemoveGeocachesWithNegativePoints(AllGeocaches);

			#region Calculate Initial route
			try
			{
				Startpoint_RP = Router1.Resolve(CompleteRouteData.Profile.ItineroProfile.profile, Startpoint, 100F);
			}
			catch (Itinero.Exceptions.ResolveFailedException)
			{
				MessageBox.Show("Please select a Startingpoint close to a road");
				Application.UseWaitCursor = false;
				return;
			}
			try
			{
				Endpoint_RP = Router1.Resolve(CompleteRouteData.Profile.ItineroProfile.profile, Endpoint, 100F);
			}
			catch (Itinero.Exceptions.ResolveFailedException)
			{
				MessageBox.Show("Please select an Endpoint close to a road");
				Application.UseWaitCursor = false;
				return;
			}

			//Calculate initial Route
			try
			{
				InitialRoute = Router1.Calculate(CompleteRouteData.Profile.ItineroProfile.profile, Startpoint_RP, Endpoint_RP);
			}
			catch (Itinero.Exceptions.RouteNotFoundException)
			{
				MessageBox.Show("Can't calculate a route between start and endpoint. Please change your selection");
				Application.UseWaitCursor = false;
				return;
			}
			CompleteRouteData.TotalDistance = InitialRoute.TotalDistance;
			CompleteRouteData.TotalTime = InitialRoute.TotalTime;


			//Empty list as no resolving happenend yet
			CompleteRouteData.partialRoutes.Add(new PartialRoute(InitialRoute, new List<GeocacheRoutingInformation>()));
			#endregion

			//TODO
			//Program.MainWindow.DisplayPreliminaryRoute(RoutingData);

			AddGeocachesToRoute(CompleteRouteData, GeocachesToInclude);

			if (Program.DB.Autotargetselection)
			{
				Program.MainWindow.UpdateStatus("Starting Autotargetselection", 10);
				CompleteRouteData=DirectionDecision(CompleteRouteData,GeocachesNotAlreadyUsed);

				//Program.MainWindow.DisplayPreliminaryRoute(RoutingData);
			}

			Program.MainWindow.UpdateStatus("Starting calculation of geocache positions", 30);

			CompleteRouteData = ResolveAndAddGeocachesToPartialRoutes(CompleteRouteData, GeocachesNotAlreadyUsed);

			Program.MainWindow.UpdateStatus("Started filling route with geocaches", 50);
			CompleteRouteData = AddGeocachesThatImproveRating(CompleteRouteData);

			//TODO Add geocaches that lie directly on route

			KeyValuePair<Route, List<Geocache>> Result = CreateResultingRoute(CompleteRouteData);
			Program.MainWindow.AddFinalRoute(Result, profile);
			Program.MainWindow.UpdateStatus("Route calculation done", 100);
			Application.UseWaitCursor = false;
			Program.RouteCalculationRunning = false;
		}

		//REDESIGNED
		#region Main Functions
		/// <summary>
		/// Removes all geocaches that don't have a positive rating
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
		/// Adds geocaches directly to route
		/// </summary>
		/// <param name="CompleteRouteData"></param>
		/// <param name="GeocachesToInclude"></param>
		/// <returns></returns>
		private RouteData AddGeocachesToRoute(RouteData CompleteRouteData, List<Geocache> GeocachesToInclude)
		{
			foreach (Geocache GeocacheToAdd in GeocachesToInclude)
			{
				int IndexOfRouteToInsertIn = -1;
				Route RouteToInsertIn = null;

				//find which partial route it is closest to. Shouldn't tkae too long as there are only few partial routes at this point
				float MinDistance = -1;
				for (int i = 0; i < CompleteRouteData.partialRoutes.Count; i++)
				{
					float Distance = GetMinimalDistanceToRoute(CompleteRouteData.partialRoutes[i].partialRoute, new Coordinate(GeocacheToAdd.lat, GeocacheToAdd.lon));
					if (MinDistance < 0 || Distance < MinDistance)
					{
						IndexOfRouteToInsertIn = i;
						MinDistance = Distance;
					}
				}

				RouteToInsertIn = CompleteRouteData.partialRoutes[IndexOfRouteToInsertIn].partialRoute;

				Result<RouterPoint> GeocacheToAddResolveResult = Router1.TryResolve(CompleteRouteData.Profile.ItineroProfile.profile, GeocacheToAdd.lat, GeocacheToAdd.lon, 100F);
				if (!GeocacheToAddResolveResult.IsError)
				{
					Result<Tuple<Route,Route>> RoutingResult = GetPartialRoutes(CompleteRouteData, RouteToInsertIn, GeocacheToAddResolveResult.Value);
					if (!RoutingResult.IsError)
					{
						CompleteRouteData = ReplaceRoute(CompleteRouteData, RoutingResult.Value.Item1, RoutingResult.Value.Item2, IndexOfRouteToInsertIn);
						CompleteRouteData.AddGeocacheOnRoute(GeocacheToAdd);
					}
				}
			}
			return CompleteRouteData;
		}

		private RouteData DirectionDecision(RouteData CompleteRouteData, List<Geocache> GeocachesToConsider)
		{
			GeocachesToConsider.OrderByDescending(x => x.Rating);

			List<Geocache> GeocachesToRemove = new List<Geocache>(); //Collects geocaches that couldn't be resolved and deletes them from the list of available geocaches for the whole routing process
			List<RouteData> Suggestions = new List<RouteData>();


			int FirstGeocacheNotUsedAsSuggestionBase = 0;
			for (int SuggestionNumber = 0; SuggestionNumber < Program.DB.RoutefindingWidth; SuggestionNumber++)
			{
				RouteData SuggestionRouteData = (RouteData)CompleteRouteData.DeepCopy();
				Suggestions.Add(SuggestionRouteData);

				int SuggestedCacheIndex = FirstGeocacheNotUsedAsSuggestionBase;

				//Add geocaches to suggested route, as long as the route is shorter than half the allowed length, but also make sure it won't get longer than three quarters 
				while (SuggestionRouteData.TotalDistance < 0.5 * SuggestionRouteData.Profile.MaxDistance * 1000)
				{
					Geocache SuggestedCache = GeocachesToConsider[SuggestedCacheIndex];

					#region Find shortest resulting route if cache is inserted Route
					int IndexOfRouteToInsertIn = 0;
					float MinEstimatedRouteLength = -1;
					for (int PartialRouteIndex = 0; PartialRouteIndex < SuggestionRouteData.partialRoutes.Count; PartialRouteIndex++)//Thus each partial route
					{
						float Length = EstimateRouteLengthIfInserted(SuggestionRouteData.partialRoutes[PartialRouteIndex].partialRoute, new Coordinate(SuggestedCache.lat, SuggestedCache.lon));
						//Whether this is the route the geocache is currently closest to
						if (MinEstimatedRouteLength < 0 || Length < MinEstimatedRouteLength)
						{
							IndexOfRouteToInsertIn = PartialRouteIndex;
							MinEstimatedRouteLength = Length;
						}
					}
					#endregion

					//Check if the Cache seems to be in range. 0.7 * Remaining Distance, since the roads to the cache are quite surely not in a straight line and we want to fill up to 0.75 only
					if (MinEstimatedRouteLength < 0.7 * (SuggestionRouteData.Profile.MaxDistance * 1000 - (SuggestionRouteData.TotalDistance - SuggestionRouteData.partialRoutes[IndexOfRouteToInsertIn].partialRoute.TotalDistance)))
					{
						Result<RouterPoint> GeocacheToAddResolveResult = Router1.TryResolve(SuggestionRouteData.Profile.ItineroProfile.profile, SuggestedCache.lat, SuggestedCache.lon, SearchDistanceInMeters);
						Route RouteToInsertIn = SuggestionRouteData.partialRoutes[IndexOfRouteToInsertIn].partialRoute;

						GeocacheRoutingInformation SuggestionBaseCache_Info;
						if (!GeocacheToAddResolveResult.IsError)
						{
							SuggestionBaseCache_Info = new GeocacheRoutingInformation(SuggestedCache, MinEstimatedRouteLength, GeocacheToAddResolveResult.Value);

							Result<Tuple<Route, Route>> RoutingResult = GetPartialRoutes(SuggestionRouteData, RouteToInsertIn, SuggestionBaseCache_Info.ResolvedCoordinates);
							if (!RoutingResult.IsError)
							{
								// So one doesn't have to iterate through all routes
								//TestRouteDistance, as one doesn't know wether this on is taken
								float NewDistance = SuggestionRouteData.TotalDistance - RouteToInsertIn.TotalDistance + RoutingResult.Value.Item1.TotalDistance + RoutingResult.Value.Item1.TotalDistance;
								float NewTime = SuggestionRouteData.TotalTime - RouteToInsertIn.TotalTime + RoutingResult.Value.Item1.TotalTime + RoutingResult.Value.Item1.TotalTime;

								if (NewDistance < 0.75 * SuggestionRouteData.Profile.MaxDistance *1000 && NewTime < 0.75 * SuggestionRouteData.Profile.MaxTime*60)
								{
									SuggestionRouteData = ReplaceRoute(SuggestionRouteData, RoutingResult.Value.Item1, RoutingResult.Value.Item2, IndexOfRouteToInsertIn);
									SuggestionRouteData.AddGeocacheOnRoute(SuggestedCache);

								}//Else just skip this cache. 
							}
						}
						else //Couldn't resolve
						{
							if (SuggestedCacheIndex == FirstGeocacheNotUsedAsSuggestionBase)
							{
								FirstGeocacheNotUsedAsSuggestionBase++;//To avoid using the same cache as base twice
							}
							GeocachesToConsider.RemoveAt(SuggestedCacheIndex);//Since it will never be possible to resolve it
						}
					}
					SuggestedCacheIndex++;
				}

				#region Find geocaches in reach
				float ReachablePoints = 0;
				Parallel.ForEach(GeocachesToConsider, geocache =>
				{
					foreach (PartialRoute Route in SuggestionRouteData.partialRoutes)
					{
						if (EstimateRouteLengthIfInserted(Route.partialRoute, new Coordinate(geocache.lat, geocache.lon)) < 0.9 * (SuggestionRouteData.Profile.MaxDistance * 1000 - (SuggestionRouteData.TotalDistance - Route.partialRoute.TotalDistance)))
						{
							ReachablePoints += geocache.Rating;
							break;//It suffices that the cache is in reach of one partial route
						}
					}
				});
				SuggestionRouteData.ReachablePointsAfterDirectionDecision = ReachablePoints + SuggestionRouteData.TotalPoints;
				#endregion

				FirstGeocacheNotUsedAsSuggestionBase++;//Since we want multiple suggestions, which can only happen if we start from different points
			}

			if (Suggestions.Count != 0)
			{
				//Return best suggestion
				return Suggestions.Find(x => x.ReachablePointsAfterDirectionDecision == Suggestions.Max(y => y.ReachablePointsAfterDirectionDecision));
			}
			//If no suggestion has been found, just return the uncanged original Data
			return CompleteRouteData;
		}

		private RouteData ResolveAndAddGeocachesToPartialRoutes(RouteData CompleteRouteData, List<Geocache> geocaches)
		{
			//This way the Geocache can be added to multiple partial routes, but no multiple times to the same one
			for (int CurrentPartialrouteIndex = 0; CurrentPartialrouteIndex < CompleteRouteData.partialRoutes.Count; CurrentPartialrouteIndex++)
			{
				Parallel.ForEach(geocaches, GC =>
				{
					float MinEstimatedRouteLength = EstimateRouteLengthIfInserted(CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].partialRoute, new Coordinate(GC.lat, GC.lon));

					//Only exclude those that definitely can't be reached
					if (MinEstimatedRouteLength <(CompleteRouteData.Profile.MaxDistance * 1000 - (CompleteRouteData.TotalDistance - CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].partialRoute.TotalDistance)))
					{
						//TODO Cache Routerpoints to improve performance when geocaches get added to multiple routes
						RouterPoint RouterPointOfGeocache = null;
						Result<RouterPoint> ResolveResult = Router1.TryResolve(CompleteRouteData.Profile.ItineroProfile.profile, GC.lat, GC.lon, SearchDistanceInMeters);
						if (!ResolveResult.IsError)
						{
							RouterPointOfGeocache = ResolveResult.Value;
							lock (CompleteRouteData.partialRoutes)
							{
								CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].GeocachesInReach.Add(new GeocacheRoutingInformation(GC, MinEstimatedRouteLength, RouterPointOfGeocache));//Push the resoved location on
							}
						}
					}
				});
			}
			return CompleteRouteData;
		}

		private RouteData AddGeocachesThatImproveRating(RouteData CompleteRouteData)
		{
			//ALWAYS keep RoutingDataList sorted by the way it came. It determines the direction of the route.

			//TODO Make MaxDistance to meters as well
			///////////////////////////////////////////////////////////////////////////////////////
			//TAKE CARE RouteDistance is in m, MaxDistance in km!!!
			////////////////////////////////////////////////////////////////////////////////
			do
			{
				//Values
				GeocacheRoutingInformation GeocacheToAdd = null;
				Route RouteToInsertIn = null;
				int IndexOfRouteToInsertIn = -1;

				#region Filter Geocaches
				/*Remove all Caches that definitely are not in Range (estimated distance, which is the shortest possible distance, bigger than distance set).
				 *Meanwhile find best rated geocache and the route it is the closest to.
				 *Only allow 0.9* the distance set, as the roads are most definitely not straight. 
				 *For the case they are, there is another routine that picks up the caches that are directly on the route. 
				*/
				for (int CurrentPartialRouteIndex = 0; CurrentPartialRouteIndex < CompleteRouteData.partialRoutes.Count; CurrentPartialRouteIndex++)
				{
					PartialRoute CurrentPartialRoute = CompleteRouteData.partialRoutes[CurrentPartialRouteIndex];
					List<GeocacheRoutingInformation> GeocachesToRemove = new List<GeocacheRoutingInformation>();
					foreach (GeocacheRoutingInformation CurrentGeocache in CurrentPartialRoute.GeocachesInReach)
					{
						//(CompleteRouteData.TotalDistance-CurrentPartialRoute.partialRoute.TotalDistance) since the latter part will be replaced
						if (CurrentGeocache.EstimatedDistanceIfInserted > (CompleteRouteData.Profile.MaxDistance * 1000 - (CompleteRouteData.TotalDistance - CurrentPartialRoute.partialRoute.TotalDistance)))
						{
							GeocachesToRemove.Add(CurrentGeocache);
						}
						else if (CompleteRouteData.GeocachesOnRoute().Contains(CurrentGeocache.geocache))
						{
							GeocachesToRemove.Add(CurrentGeocache);
						}
						else if (CurrentGeocache.EstimatedDistanceIfInserted < 0.9 * (CompleteRouteData.Profile.MaxDistance * 1000 - (CompleteRouteData.TotalDistance - CurrentPartialRoute.partialRoute.TotalDistance)))
						{
							/* If no cache is set as next geocache to insert
							 * If the cache we stumbled accross is rated better than the old best geocache
							 * If They are the same geocache but from a different route, take the one that is shorter
							*/
							if (GeocacheToAdd == null || CurrentGeocache.geocache.Rating > GeocacheToAdd.geocache.Rating || (CurrentGeocache.geocache == GeocacheToAdd.geocache && CurrentGeocache.EstimatedDistanceIfInserted < GeocacheToAdd.EstimatedDistanceIfInserted))
							{
								GeocacheToAdd = CurrentGeocache;
								RouteToInsertIn = CurrentPartialRoute.partialRoute;
								IndexOfRouteToInsertIn = CurrentPartialRouteIndex;
							}
						}
					}

					//Remove all geocaches that can't be reached or are already used
					foreach (GeocacheRoutingInformation Geocache_Info in GeocachesToRemove)
					{
						CurrentPartialRoute.GeocachesInReach.Remove(Geocache_Info);
					}
				}
				#endregion

				if (GeocacheToAdd != null)
				{
					Result<Tuple<Route, Route>> RoutingResult = GetPartialRoutes(CompleteRouteData, RouteToInsertIn, GeocacheToAdd.ResolvedCoordinates);

					if (!RoutingResult.IsError)
					{
						float NewDistance = CompleteRouteData.TotalDistance - RouteToInsertIn.TotalDistance + RoutingResult.Value.Item1.TotalDistance + RoutingResult.Value.Item1.TotalDistance;
						float NewTimeWithGeocaches = CompleteRouteData.TotalTime - RouteToInsertIn.TotalTime + RoutingResult.Value.Item1.TotalTime + RoutingResult.Value.Item1.TotalTime + (CompleteRouteData.GeocachesOnRoute().Count + 1) * CompleteRouteData.Profile.TimePerGeocache * 60;
						float NewRoutePoints = CompleteRouteData.TotalPoints + GeocacheToAdd.geocache.Rating;

						//calculate in meters
						if (NewDistance > CompleteRouteData.Profile.MaxDistance * 1000)
						{
							NewRoutePoints -= (NewDistance - CompleteRouteData.Profile.MaxDistance * 1000) * CompleteRouteData.Profile.PenaltyPerExtraKM / 1000;
						}
						//Calculate in minutes
						if (NewTimeWithGeocaches > CompleteRouteData.Profile.MaxTime * 60)
						{
							NewRoutePoints -= (NewTimeWithGeocaches - CompleteRouteData.Profile.MaxTime * 60) * CompleteRouteData.Profile.PenaltyPerExtra10min / 600;
						}

						if (NewRoutePoints > CompleteRouteData.TotalPoints)
						{
							CompleteRouteData = ReplaceRoute(CompleteRouteData, RoutingResult.Value.Item1, RoutingResult.Value.Item2, IndexOfRouteToInsertIn);
							CompleteRouteData.AddGeocacheOnRoute(GeocacheToAdd.geocache);
							CompleteRouteData.TotalPoints = NewRoutePoints;//Overwrites the addition automatically made in the lne before, to make sure the 
						}
						else
						{
							CompleteRouteData.partialRoutes[IndexOfRouteToInsertIn].GeocachesInReach.Remove(GeocacheToAdd);//As the geocache doesn't help from this route
						}
					}
				}
				else
				{
					break;//since there are no more geocaches to be added
				}

			} while (FailedRouteCalculations < FailedRouteCalculationsLimit);//-1, as the one has been tried to be added to the Route. If the if clause jumped in, it makes no differenc, as -1 is still less than 0

			if (FailedRouteCalculations == FailedRouteCalculationsLimit)
			{
				MessageBox.Show("Couldn't properly calculate route due to a probable island. Please select different Start/Endpoints", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return CompleteRouteData;
		}

		private KeyValuePair<Route,List<Geocache>> CreateResultingRoute(RouteData CompleteRouteData)
		{
			Route FinalRoute = CompleteRouteData.partialRoutes[0].partialRoute;

			for (int i = 1; i < CompleteRouteData.partialRoutes.Count; i++)
			{
				FinalRoute = FinalRoute.Concatenate(CompleteRouteData.partialRoutes[i].partialRoute);
			}

			return new KeyValuePair<Route, List<Geocache>>(FinalRoute, CompleteRouteData.GeocachesOnRoute());
		}
		#endregion

		#region Helper Functions
		/// <summary>
		/// Returns the shortest distance of the coordinates to the route.
		/// </summary>
		/// <param name="route"></param>
		/// <param name="coordinates"></param>
		/// <returns></returns>
		private float GetMinimalDistanceToRoute(Route route, Coordinate coord)
		{
			float MinDistance = -1;//-1, so it is known if no "highscore" has been set

			for (int k = 0; k < route.Shape.Length; k += Program.DB.EveryNthShapepoint)
			{
				float Distance = Coordinate.DistanceEstimateInMeter(route.Shape[k], coord);
				if (MinDistance < 0 || Distance < MinDistance)
				{
					MinDistance = Distance;
				}
			}
			return MinDistance;
		}

		/// <summary>
		/// Sum of distance of coordinates to endpoint and startpoint of route
		/// </summary>
		/// <param name="route"></param>
		/// <param name="coord"></param>
		/// <returns></returns>
		private float EstimateRouteLengthIfInserted(Route route, Coordinate coord)
		{
			return Coordinate.DistanceEstimateInMeter(route.Shape[0], coord) + Coordinate.DistanceEstimateInMeter(route.Shape[route.Count() - 1], coord);
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
					float minDistance = GetMinimalDistanceToRoute(NewPart1, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					NewPart1Geocaches.Add(new GeocacheRoutingInformation(GC_Info.geocache, minDistance, GC_Info.ResolvedCoordinates));//Push the resolved location on
				}
			}));

			Thread Thread2 = new Thread(new ThreadStart(() =>
			{
				foreach (GeocacheRoutingInformation GC_Info in new List<GeocacheRoutingInformation>(OldRouteGeocaches2))
				{
					float minDistance = GetMinimalDistanceToRoute(NewPart1, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					NewPart2Geocaches.Add(new GeocacheRoutingInformation(GC_Info.geocache, minDistance, GC_Info.ResolvedCoordinates));//Push the resolved location on
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
		#endregion

		#region structs
		class RouteData
		{
			public List<PartialRoute> partialRoutes { get; set; }
			private List<Geocache> _GeocachesOnRoute { get; set; }
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
			public float ReachablePointsAfterDirectionDecision { get; set; }

			public void AddGeocacheOnRoute(Geocache geocache)
			{
				_GeocachesOnRoute.Add(geocache);
				TotalPoints += geocache.Rating;
			}
			public List<Geocache> GeocachesOnRoute()
			{
				return _GeocachesOnRoute;
			}

			public RouteData()
			{
				partialRoutes = new List<PartialRoute>();
				_GeocachesOnRoute = new List<Geocache>();
			}

			public RouteData DeepCopy()
			{
				RouteData _DeepCopy = new RouteData();
				foreach(PartialRoute partialRoute in partialRoutes)
				{
					_DeepCopy.partialRoutes.Add(partialRoute.DeepCopy());
				}
				foreach(Geocache geocache in _GeocachesOnRoute)
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

		class PartialRoute
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

			public PartialRoute()
			{
				GeocachesInReach = new List<GeocacheRoutingInformation>();
			}
			public PartialRoute DeepCopy()
			{
				PartialRoute _DeepCopy = new PartialRoute();
				_DeepCopy.partialRoute = partialRoute;//No deeper copy needed since partial route won't be changed
				foreach(GeocacheRoutingInformation geocacheRoutingInfo in GeocachesInReach)
				{
					_DeepCopy.GeocachesInReach.Add(new GeocacheRoutingInformation(geocacheRoutingInfo));
				}
				return _DeepCopy;
			}
		}

		class GeocacheRoutingInformation
		{
			/// <summary>
			/// Always use the position of the geocache for the shortest distance calculations
			/// </summary>
			public Geocache geocache { get; set; }
			/// <summary>
			/// In meters
			/// </summary>
			public float DistanceToRoute { get; set; }
			/// <summary>
			/// In meters. Triangle between Startpoint of partial route, geocache and endpoint of partial route
			/// </summary>
			public float EstimatedDistanceIfInserted { get; set; }
			public RouterPoint ResolvedCoordinates { get; set; }//Used so coordinates only have to be reoslved once

			public GeocacheRoutingInformation(Geocache geocache, float DistanceToRoute, float EstimatedDistanceIfInserted, RouterPoint ResolvedCoordinates)
			{
				this.geocache = geocache;
				this.DistanceToRoute = DistanceToRoute;
				this.EstimatedDistanceIfInserted = EstimatedDistanceIfInserted;
				this.ResolvedCoordinates = ResolvedCoordinates;
			}

			public GeocacheRoutingInformation(Geocache geocache, float EstimatedDistanceIfInserted, RouterPoint ResolvedCoordinates)
			{
				this.geocache = geocache;
				this.EstimatedDistanceIfInserted = EstimatedDistanceIfInserted;
				this.ResolvedCoordinates = ResolvedCoordinates;
			}

			public GeocacheRoutingInformation(GeocacheRoutingInformation ObjectToCopy)
			{
				geocache = ObjectToCopy.geocache;
				DistanceToRoute = ObjectToCopy.DistanceToRoute;
				EstimatedDistanceIfInserted = ObjectToCopy.EstimatedDistanceIfInserted;
				ResolvedCoordinates = ObjectToCopy.ResolvedCoordinates;
			}
		}
		#endregion
	}
}
