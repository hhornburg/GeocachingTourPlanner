using Itinero;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeocachingTourPlanner_WPF;
using System.Windows;

namespace GeocachingTourPlanner
{
	public class Tourplanning
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
			Fileoperations.Routerlog.AddMainSection("Started new Routing");
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
			if (App.RouterDB.IsEmpty)
			{
				MessageBox.Show("Import or set RouterDB before creating route!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				//FIX App.mainWindow.UseWaitCursor = false;
				return;
			}
			//One for every thread
			Router1 = new Router(App.RouterDB);
			Router2 = new Router(App.RouterDB);
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
				//FIX Application.UseWaitCursor = false;
				return;
			}
			try
			{
				Endpoint_RP = Router1.Resolve(CompleteRouteData.Profile.ItineroProfile.profile, Endpoint, 100F);
			}
			catch (Itinero.Exceptions.ResolveFailedException)
			{
				MessageBox.Show("Please select an Endpoint close to a road");
				//FIX Application.UseWaitCursor = false;
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
				//FIX Application.UseWaitCursor = false;
				return;
			}
			CompleteRouteData.TotalDistance = InitialRoute.TotalDistance;
			CompleteRouteData.TotalTime = InitialRoute.TotalTime;

			//Empty list as no resolving happenend yet
			CompleteRouteData.partialRoutes.Add(new PartialRoute(InitialRoute, new List<GeocacheRoutingInformation>()));
			

			Fileoperations.Routerlog.AddMainInformation("Calculated Initial Route");
			Fileoperations.Routerlog.AddSubInformation("Length:" + InitialRoute.TotalDistance);
			Fileoperations.Routerlog.AddSubInformation("Time:" + InitialRoute.TotalTime);

			DisplayPreliminaryRoute(CompleteRouteData);
			#endregion

			#region ForceInclude
			App.mainWindow.UpdateStatus("Starting adding Geocaches set as ForceInclude", 10);
			AddGeocachesToRoute(CompleteRouteData, GeocachesToInclude);

			Fileoperations.Routerlog.AddMainInformation("Adding Geocaches the user selected done");
			Fileoperations.Routerlog.AddSubInformation("Length: " + CompleteRouteData.TotalDistance);
			Fileoperations.Routerlog.AddSubInformation("Time: " + CompleteRouteData.TotalTime);
			Fileoperations.Routerlog.AddSubInformation("Geocaches on Route: " + CompleteRouteData.GeocachesOnRoute().Count);
			Fileoperations.Routerlog.AddSubInformation("Points collected: " + CompleteRouteData.TotalPoints);

			DisplayPreliminaryRoute(CompleteRouteData);
			#endregion

			#region Autotargetselection
			if (App.DB.Autotargetselection)
			{
				App.mainWindow.UpdateStatus("Starting Autotargetselection", 20);
				CompleteRouteData = DirectionDecision(CompleteRouteData, GeocachesNotAlreadyUsed);

				Fileoperations.Routerlog.AddMainInformation("Autotargetselection done");
				Fileoperations.Routerlog.AddSubInformation("Length: " + CompleteRouteData.TotalDistance);
				Fileoperations.Routerlog.AddSubInformation("Time: " + CompleteRouteData.TotalTime);
				Fileoperations.Routerlog.AddSubInformation("Geocaches on Route: " + CompleteRouteData.GeocachesOnRoute().Count);
				Fileoperations.Routerlog.AddSubInformation("Points collected: " + CompleteRouteData.TotalPoints);

				if (CheckIfIsland())
				{
					App.RouteCalculationRunning = false;
					App.mainWindow.UpdateStatus("Route calculation failed", 100);
					Application.UseWaitCursor = false;
				}

				DisplayPreliminaryRoute(CompleteRouteData);
			}

			#endregion

			#region Resolving
			App.mainWindow.UpdateStatus("Starting calculation of geocache positions", 40);

			CompleteRouteData = ResolveAndAddGeocachesToPartialRoutes(CompleteRouteData, GeocachesNotAlreadyUsed);
			#endregion

			#region Filling route
			App.mainWindow.UpdateStatus("Started filling route with geocaches", 50);
			CompleteRouteData = FillRouteWithGeocachesUntilMaxDistanceIsReached(CompleteRouteData);

			Fileoperations.Routerlog.AddMainInformation("Adding Geocaches that improve rating done");
			Fileoperations.Routerlog.AddSubInformation("Length: " + CompleteRouteData.TotalDistance);
			Fileoperations.Routerlog.AddSubInformation("Time: " + CompleteRouteData.TotalTime);
			Fileoperations.Routerlog.AddSubInformation("Geocaches on Route: " + CompleteRouteData.GeocachesOnRoute().Count);
			Fileoperations.Routerlog.AddSubInformation("Points collected: " + CompleteRouteData.TotalPoints);
			#endregion

			if (CheckIfIsland())
			{
				App.RouteCalculationRunning = false;
				App.mainWindow.UpdateStatus("Route calculation failed", 100);
				//FIX Application.UseWaitCursor = false;
			}

			//TODO Add geocaches that lie directly on route

			App.RouteCalculationRunning = false;//To make sure not another preliminary route is displayed
			App.mainWindow.AddFinalRoute(CompleteRouteData);
			App.mainWindow.UpdateStatus("Route calculation done", 100);
			//FIX Application.UseWaitCursor = false;
			
		}

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
					Result<Tuple<Route, Route>> RoutingResult = GetPartialRoutes(CompleteRouteData, RouteToInsertIn, GeocacheToAddResolveResult.Value);
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
			GeocachesToConsider = GeocachesToConsider.OrderByDescending(x => x.Rating).ToList();

			List<RouteData> Suggestions = new List<RouteData>();

			int NumberofRouteslongerthanlimit = 0;

			int FirstGeocacheNotUsedAsSuggestionBase = App.DB.RoutefindingWidth+1;
			try
			{
				Parallel.For(0, App.DB.RoutefindingWidth, NumberOfSuggestions =>
			   {
				   Fileoperations.Routerlog.LogCollector Log = new Fileoperations.Routerlog.LogCollector("DirectionDecision" + NumberOfSuggestions);

				   RouteData SuggestionRouteData;
				   lock (CompleteRouteData)
				   {
					   SuggestionRouteData = CompleteRouteData.DeepCopy();
				   }
				   lock (Suggestions)
				   {
					   Suggestions.Add(SuggestionRouteData);
				   }

				   int SuggestedCacheIndex = NumberOfSuggestions;

				   //Add geocaches to suggested route, as long as the route is shorter than the min allowed length, but also make sure it doesn't get too long in time and distance
				   while (SuggestionRouteData.TotalDistance < App.DB.PercentageOfDistanceInAutoTargetselection_Min * SuggestionRouteData.Profile.MaxDistance * 1000 && SuggestedCacheIndex < GeocachesToConsider.Count)
				   {
					   Geocache SuggestedCache = GeocachesToConsider[SuggestedCacheIndex];
					   Log.AddMainInformation("Suggested " + SuggestedCache + " at Index Position " + SuggestedCacheIndex);

					   #region Find shortest resulting route if cache is inserted Route
					   int IndexOfRouteToInsertIn = 0;
					   float MinEstimatedExtraRouteLength = -1;
					   for (int PartialRouteIndex = 0; PartialRouteIndex < SuggestionRouteData.partialRoutes.Count; PartialRouteIndex++)//Thus each partial route
					   {
						   float Length = GetEstimatedExtraDistance(SuggestionRouteData.partialRoutes[PartialRouteIndex].partialRoute, new Coordinate(SuggestedCache.lat, SuggestedCache.lon));
						   //Whether this is the route the geocache is currently closest to
						   if (MinEstimatedExtraRouteLength < 0 || Length < MinEstimatedExtraRouteLength)
						   {
							   IndexOfRouteToInsertIn = PartialRouteIndex;
							   MinEstimatedExtraRouteLength = Length;
						   }
					   }
					   #endregion

					   Log.AddSubInformation("Estimated Route length: " + MinEstimatedExtraRouteLength);

					   //Check if the Cache seems to be in range. Error_Percentage * Max_Percentage * Remaining Distance, since the roads to the cache are quite surely not in a straight line and we want to fill up to the percentage only
					   if (MinEstimatedExtraRouteLength <App.DB.PercentageOfDistanceInAutoTargetselection_Max * (SuggestionRouteData.Profile.MaxDistance * 1000 - SuggestionRouteData.TotalDistance))
					   {
						   Result<RouterPoint> GeocacheToAddResolveResult = Router1.TryResolve(SuggestionRouteData.Profile.ItineroProfile.profile, SuggestedCache.lat, SuggestedCache.lon, SearchDistanceInMeters);
						   Route RouteToInsertIn = SuggestionRouteData.partialRoutes[IndexOfRouteToInsertIn].partialRoute;

						   GeocacheRoutingInformation SuggestionBaseCache_Info;
						   if (!GeocacheToAddResolveResult.IsError)
						   {
							   SuggestionBaseCache_Info = new GeocacheRoutingInformation(SuggestedCache,MinEstimatedExtraRouteLength, GeocacheToAddResolveResult.Value);

							   Result<Tuple<Route, Route>> RoutingResult = GetPartialRoutes(SuggestionRouteData, RouteToInsertIn, SuggestionBaseCache_Info.ResolvedCoordinates);
							   if (!RoutingResult.IsError)
							   {
								   // So one doesn't have to iterate through all routes
								   //TestRouteDistance, as one doesn't know wether this on is taken
								   float NewDistance = SuggestionRouteData.TotalDistance - RouteToInsertIn.TotalDistance + RoutingResult.Value.Item1.TotalDistance + RoutingResult.Value.Item1.TotalDistance;
								   float NewTime = SuggestionRouteData.TotalTime - RouteToInsertIn.TotalTime + RoutingResult.Value.Item1.TotalTime + RoutingResult.Value.Item1.TotalTime;

								   if (NewDistance < App.DB.PercentageOfDistanceInAutoTargetselection_Max * SuggestionRouteData.Profile.MaxDistance * 1000 && NewTime < App.DB.PercentageOfDistanceInAutoTargetselection_Max * SuggestionRouteData.Profile.MaxTime * 60)
								   {
									   Log.AddSubInformation("Added the Geocache to the Route. New Distance:" + NewDistance);
									   SuggestionRouteData = ReplaceRoute(SuggestionRouteData, RoutingResult.Value.Item1, RoutingResult.Value.Item2, IndexOfRouteToInsertIn);
									   SuggestionRouteData.AddGeocacheOnRoute(SuggestedCache);

								   }//Else just skip this cache. 
								   else
								   {
									   Log.AddSubInformation("The Resulting Distance was too long");
									   NumberofRouteslongerthanlimit++;
								   }
							   }
						   }
						   else //Couldn't resolve
						   {
							   Log.AddSubInformation("Couldn't resolve Geocache");
							   if (SuggestedCacheIndex == FirstGeocacheNotUsedAsSuggestionBase)
							   {
								   FirstGeocacheNotUsedAsSuggestionBase++;//To avoid using the same cache as base twice
								   SuggestedCacheIndex = FirstGeocacheNotUsedAsSuggestionBase--;//Since ++ will happen
							   }
							   lock (GeocachesToConsider)
							   {
								   GeocachesToConsider.RemoveAt(SuggestedCacheIndex);//Since it will never be possible to resolve it
							   }
						   }
					   }
					   else
					   {
						   Log.AddSubInformation("Estimated Distance was too long");
					   }

					   if (FailedRouteCalculations > FailedRouteCalculationsLimit)
					   {
						   break;
					   }
					   else
					   {
						   SuggestedCacheIndex++;
					   }
				   }

				   Log.Write(); // Since this thread is done
			   });
			}
			catch (Exception)
			{
				Fileoperations.Routerlog.AddSubInformation("At least one exception has been caused.");
			}
			Fileoperations.Routerlog.AddSubInformation(NumberofRouteslongerthanlimit + " Routes have been to long.");

			if (Suggestions.Count != 0)
			{
				//Return best suggestion
				return Suggestions.Find(x => x.TotalPoints == Suggestions.Max(y => y.TotalPoints));
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
					float ExtraDistance = GetEstimatedExtraDistance(CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].partialRoute, new Coordinate(GC.lat, GC.lon));

					//Only exclude those that definitely can't be reached
					if (ExtraDistance < CompleteRouteData.Profile.MaxDistance * 1000 - CompleteRouteData.TotalDistance)
					{
						//TODO Cache Routerpoints to improve performance when geocaches get added to multiple routes
						RouterPoint RouterPointOfGeocache = null;
						Result<RouterPoint> ResolveResult = Router1.TryResolve(CompleteRouteData.Profile.ItineroProfile.profile, GC.lat, GC.lon, SearchDistanceInMeters);
						if (!ResolveResult.IsError)
						{
							float DistanceToRoute = GetMinimalDistanceToRoute(CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].partialRoute, new Coordinate(GC.lat, GC.lon));
							RouterPointOfGeocache = ResolveResult.Value;
							lock (CompleteRouteData.partialRoutes)
							{
								CompleteRouteData.partialRoutes[CurrentPartialrouteIndex].GeocachesInReach.Add(new GeocacheRoutingInformation(GC, DistanceToRoute,ExtraDistance, RouterPointOfGeocache));//Push the resoved location on
							}
						}
					}
				});
			}
			return CompleteRouteData;
		}

		#region Fill Route
		/// <summary>
		/// Only use to lock GeeocacheToAdd in AddGeocacheToThatImportRating
		/// </summary>
		private readonly object GeocacheToAddLocker = new object();
		/// <summary>
		/// Only use to lock RouteToInsertIn in AddGeocacheToThatImportRating
		/// </summary>
		private readonly object RouteToInsertInLocker = new object();
		private RouteData FillRouteWithGeocachesUntilMaxDistanceIsReached(RouteData CompleteRouteData)
		{
			Fileoperations.Routerlog.AddSubSection("Entered filling route with geocaches");
			//TODO Make MaxDistance to meters as well
			///////////////////////////////////////////////////////////////////////////////////////
			//TAKE CARE RouteDistance is in m, MaxDistance in km!!!
			////////////////////////////////////////////////////////////////////////////////

			GeocacheRoutingInformation GeocacheToAdd = null;
			do
			{
				//Values
				GeocacheToAdd = null;
				Route RouteToInsertIn = null;
				int IndexOfRouteToInsertIn = -1;

				#region Filter Geocaches
				/*Remove all Caches that definitely are not in Range (estimated distance, which is the shortest possible distance, bigger than distance set).
				 *Meanwhile find best rated geocache and the route it is the closest to.
				 *Only allow apercentage of the distance set, as the roads are most definitely not straight. 
				 *For the case they are, there is another routine that picks up the caches that are directly on the route. 
				*/
				Parallel.For(0, CompleteRouteData.partialRoutes.Count, CurrentPartialRouteIndex =>
				 {
					 PartialRoute CurrentPartialRoute = CompleteRouteData.partialRoutes[CurrentPartialRouteIndex];
					 List<GeocacheRoutingInformation> GeocachesToRemove = new List<GeocacheRoutingInformation>();
					 foreach (GeocacheRoutingInformation CurrentGeocache in CurrentPartialRoute.GeocachesInReach)
					 {
						 if (CurrentGeocache.EstimatedExtraDistance > (CompleteRouteData.Profile.MaxDistance * 1000 - CompleteRouteData.TotalDistance))
						 {
							 GeocachesToRemove.Add(CurrentGeocache);
						 }
						 else if (CompleteRouteData.GeocachesOnRoute().Contains(CurrentGeocache.geocache))
						 {
							 GeocachesToRemove.Add(CurrentGeocache);
						 }
						 else if (CurrentGeocache.EstimatedExtraDistance <CompleteRouteData.Profile.MaxDistance * 1000 - CompleteRouteData.TotalDistance)
						 {
							 /* If no cache is set as next geocache to insert
							  * If the cache we stumbled accross has more routingpoints than the old cache
							 */
							 if (GeocacheToAdd == null || CurrentGeocache.RoutingPoints > GeocacheToAdd.RoutingPoints)
							 {
								 lock (GeocacheToAddLocker)//Since one can't lock GeocacheToAdd directly, since it is not the same object that is locked and unlocked
								 {
									 GeocacheToAdd = CurrentGeocache;
								 }
								 lock (RouteToInsertInLocker)//See above
								 {
									 RouteToInsertIn = CurrentPartialRoute.partialRoute;
								 }
								 IndexOfRouteToInsertIn = CurrentPartialRouteIndex;

							 }
						 }
					 }

					 //Remove all geocaches that can't be reached or are already used
					 foreach (GeocacheRoutingInformation Geocache_Info in GeocachesToRemove)
					 {
						 CurrentPartialRoute.GeocachesInReach.Remove(Geocache_Info);
					 }
				 });
				#endregion
				//Won't be able to fit a geocache in
				if(CompleteRouteData.TotalTime + CompleteRouteData.Profile.TimePerGeocache*60 > CompleteRouteData.Profile.MaxTime * 60)
				{
					GeocacheToAdd = null;
				}

				if (GeocacheToAdd != null)
				{
					Result<Tuple<Route, Route>> RoutingResult = GetPartialRoutes(CompleteRouteData, RouteToInsertIn, GeocacheToAdd.ResolvedCoordinates);

					if (!RoutingResult.IsError)
					{
						float NewDistance = CompleteRouteData.TotalDistance - RouteToInsertIn.TotalDistance + RoutingResult.Value.Item1.TotalDistance + RoutingResult.Value.Item2.TotalDistance;
						float NewTimeWithGeocaches = CompleteRouteData.TotalTime - RouteToInsertIn.TotalTime + RoutingResult.Value.Item1.TotalTime + RoutingResult.Value.Item2.TotalTime + (CompleteRouteData.GeocachesOnRoute().Count + 1) * CompleteRouteData.Profile.TimePerGeocache * 60;
						float NewRoutePoints = CompleteRouteData.TotalPoints + GeocacheToAdd.geocache.Rating;

						//calculate in meters
						if (NewDistance < CompleteRouteData.Profile.MaxDistance * 1000 && NewTimeWithGeocaches < CompleteRouteData.Profile.MaxTime * 60)
						{
							Debug.WriteLine("Added " + GeocacheToAdd.geocache);
							CompleteRouteData = ReplaceRoute(CompleteRouteData, RoutingResult.Value.Item1, RoutingResult.Value.Item2, IndexOfRouteToInsertIn);
							CompleteRouteData.AddGeocacheOnRoute(GeocacheToAdd.geocache);
							CompleteRouteData.TotalPoints = NewRoutePoints;//Overwrites the addition automatically made in the lne before, to make sure the 
							DisplayPreliminaryRoute(CompleteRouteData);
						}
						else
						{
							Debug.WriteLine("Made calculation for nothing. Route was longer in distance or time than allowed");
							CompleteRouteData.partialRoutes[IndexOfRouteToInsertIn].GeocachesInReach.Remove(GeocacheToAdd);//As the geocache doesn't help from this route
						}
					}
					else
					{
						Debug.WriteLine("Routing was errounous. Island detection score:" + FailedRouteCalculations);
						CompleteRouteData.partialRoutes[IndexOfRouteToInsertIn].GeocachesInReach.Remove(GeocacheToAdd);//Since trying to route to it results in an error
					}
				}
			} while (FailedRouteCalculations < FailedRouteCalculationsLimit && GeocacheToAdd != null);

			return CompleteRouteData;
		}
		#endregion

		private void DisplayPreliminaryRoute(RouteData CompleteRouteData)
		{
			new Thread(new ThreadStart(() =>
			{
				Route PreliminaryRoute = CompleteRouteData.partialRoutes[0].partialRoute;

				for (int i = 1; i < CompleteRouteData.partialRoutes.Count; i++)
				{
					PreliminaryRoute = PreliminaryRoute.Concatenate(CompleteRouteData.partialRoutes[i].partialRoute);
				}

				App.mainWindow.DisplayPreliminaryRoute(PreliminaryRoute);
			})).Start();
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

			for (int k = 0; k < route.Shape.Length; k ++)
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
		private float GetEstimatedExtraDistance(Route route, Coordinate coord)
		{
			float TriangleLengthWithGeocache = Coordinate.DistanceEstimateInMeter(route.Shape[0], coord) + Coordinate.DistanceEstimateInMeter(route.Shape[route.Count() - 1], coord);
			float MaxCurrentTriangleLength = -1;//-1, so it is known if no "highscore" has been set

			for (int k = 1; k < route.Shape.Length - 1; k ++)//Sincce Start and endpoint don't need to be used
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
					float minDistance = GetMinimalDistanceToRoute(NewPart1, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					float estimatedDistance = GetEstimatedExtraDistance(NewPart1, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					NewPart1Geocaches.Add(new GeocacheRoutingInformation(GC_Info.geocache, minDistance, estimatedDistance, GC_Info.ResolvedCoordinates));//Push the resolved location on
				}
			}));

			Thread Thread2 = new Thread(new ThreadStart(() =>
			{
				foreach (GeocacheRoutingInformation GC_Info in new List<GeocacheRoutingInformation>(OldRouteGeocaches2))
				{
					float minDistance = GetMinimalDistanceToRoute(NewPart1, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
					float estimatedDistance = GetEstimatedExtraDistance(NewPart2, new Coordinate(GC_Info.geocache.lat, GC_Info.geocache.lon));
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
		#endregion

		#region structs
		public class RouteData
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
				foreach (PartialRoute partialRoute in partialRoutes)
				{
					_DeepCopy.partialRoutes.Add(partialRoute.DeepCopy());
				}
				foreach (Geocache geocache in _GeocachesOnRoute)
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
			public Geocache geocache { get; private set; }
			/// <summary>
			/// In meters
			/// </summary>
			public float DistanceToRoute
			{
				get { return DistanceToRoute_field; }
				set
				{
					DistanceToRoute_field = value;
					RoutingPoints = geocache.Rating / (1 + EstimatedExtraDistance * DistanceToRoute);
				}
			}
			private float DistanceToRoute_field;

			/// <summary>
			/// In meters. Triangle between Startpoint of partial route, geocache and endpoint of partial route
			/// </summary>
			public float EstimatedExtraDistance {
				get { return EstimatedExtraDistance_field; }
				set { EstimatedExtraDistance_field = value;
					RoutingPoints = geocache.Rating / (1 + EstimatedExtraDistance * DistanceToRoute);
				} }
			private float EstimatedExtraDistance_field;

			/// <summary>
			/// Points for addition, used in filling up the route.
			/// </summary>
			/// <value>RoutingPoints = geocache.Rating / (1 + EstimatedExtraDistance * DistanceToRoute);</value>
			public float RoutingPoints { get; private set; }

			public RouterPoint ResolvedCoordinates { get; set; }//Used so coordinates only have to be reoslved once

			public GeocacheRoutingInformation(Geocache geocache, float DistanceToRoute, float EstimatedExtraDistance, RouterPoint ResolvedCoordinates)
			{
				this.geocache = geocache;
				this.DistanceToRoute = DistanceToRoute;
				this.EstimatedExtraDistance = EstimatedExtraDistance;
				this.ResolvedCoordinates = ResolvedCoordinates;

				RoutingPoints = geocache.Rating / (1 + EstimatedExtraDistance * DistanceToRoute);
			}

			public GeocacheRoutingInformation(Geocache geocache, float EstimatedExtraDistance, RouterPoint ResolvedCoordinates)
			{
				this.geocache = geocache;
				this.EstimatedExtraDistance = EstimatedExtraDistance;
				this.ResolvedCoordinates = ResolvedCoordinates;
			}

			public GeocacheRoutingInformation(GeocacheRoutingInformation ObjectToCopy)
			{
				geocache = ObjectToCopy.geocache;
				DistanceToRoute = ObjectToCopy.DistanceToRoute;
				EstimatedExtraDistance = ObjectToCopy.EstimatedExtraDistance;
				ResolvedCoordinates = ObjectToCopy.ResolvedCoordinates;
			}

			public override string ToString()
			{
				return geocache.ToString();
			}
		}
		#endregion
	}
}
