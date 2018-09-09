using Itinero;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeocachingTourPlanner.IO;
using System.Windows;
using GeocachingTourPlanner.Types;
using System.Xml.Serialization;

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

		public RoutePlanner(string Name)
		{
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
		}

		/// <summary>
		/// Takes the waypoints from the CompleteRouteData and calculates the route that goes through the waypoints in their specified order.
		/// </summary>
		/// <returns></returns>
		public bool CalculateDirectRoute()
		{
            if(Router1 == null || Router2 == null)
            {
                //One for every thread
                Router1 = new Router(App.RouterDB);
                Router2 = new Router(App.RouterDB);
            }

            if (CompleteRouteData.Profile == null)
            {
                MessageBox.Show("Please select a Routingprofile", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

			CompleteRouteData.partialRoutes.Clear();
			CompleteRouteData.GeocachesOnRoute.Clear();
			CompleteRouteData.TotalDistance = 0;
			CompleteRouteData.TotalPoints = 0;
			CompleteRouteData.TotalTime = 0; ;

			//TODO Parallel.FOR
			for(int i=0; i < CompleteRouteData.Waypoints.Count-1; i++)//-1 since always the next is also used
			{
				if (CompleteRouteData.Waypoints[i].routerPoint == null)
				{
						float lat= CompleteRouteData.Waypoints[i].lat;
						float lon = CompleteRouteData.Waypoints[i].lon;
						Result<RouterPoint> result = Router1.TryResolve(CompleteRouteData.Profile.ItineroProfile.profile, lat, lon, SearchDistanceInMeters);
						if (result.IsError)
						{
							//TODO Messagebox informing about geocache
							return false;
						}
						else
						{
							CompleteRouteData.Waypoints[i].routerPoint = result.Value;
						}
				}

				if (CompleteRouteData.Waypoints[i+1].routerPoint == null)
				{
						float lat = CompleteRouteData.Waypoints[i+1].lat;
						float lon = CompleteRouteData.Waypoints[i+1].lon;
						Result<RouterPoint> result = Router1.TryResolve(CompleteRouteData.Profile.ItineroProfile.profile, lat, lon, SearchDistanceInMeters);
						if (result.IsError)
						{
							//TODO Messagebox informing about geocache
							return false;
						}
						else
						{
							CompleteRouteData.Waypoints[i+1].routerPoint = result.Value;
						}
				}

				Result<Route> routeResult = Router1.TryCalculate(CompleteRouteData.Profile.ItineroProfile.profile, CompleteRouteData.Waypoints[i].routerPoint, CompleteRouteData.Waypoints[i + 1].routerPoint);
				if (routeResult.IsError)
				{
					return false;
				}
				else
				{
					CompleteRouteData.partialRoutes.Add(new PartialRoute(routeResult.Value));
				}
			}
			return true;
		}

		/// <summary>
		/// Adds Geocaches that lie directly on the Route
		/// Returns true only if all geocaches could be added
		/// </summary>
		public bool AddGeocachesDirectlyOnRoute()
		{
			//TODO Prallel.Foreach outer loop, since then there is less waiting, because it won't happen that two geocaches are added to the same route at a time
			foreach(PartialRoute PR in CompleteRouteData.partialRoutes)
			{
				Route RouteToInsertIn = PR.partialRoute;
				foreach (GeocacheRoutingInformation GC in PR.GeocachesInReach)
				{
					Geocache GeocacheToAdd = GC.geocache;
					if (GC.EstimatedExtraDistance_InRoute == -1)
					{
						GC.EstimatedExtraDistance_InRoute = ExtraDistance_InRoute(RouteToInsertIn, new Coordinate(GC.geocache.lat, GC.geocache.lon));
					}

					if(GC.EstimatedExtraDistance_InRoute < 10)
					{
						Result<Tuple<Route, Route>> RoutingResult = GetPartialRoutes(RouteToInsertIn, GC.ResolvedCoordinates);
						if (!RoutingResult.IsError)
						{
							float NewDistance = CompleteRouteData.TotalDistance - RouteToInsertIn.TotalDistance + RoutingResult.Value.Item1.TotalDistance + RoutingResult.Value.Item2.TotalDistance;
							float NewTimeWithGeocaches = CompleteRouteData.TotalTime - RouteToInsertIn.TotalTime + RoutingResult.Value.Item1.TotalTime + RoutingResult.Value.Item2.TotalTime + (CompleteRouteData.GeocachesOnRoute.Count + 1) * CompleteRouteData.Profile.TimePerGeocache * 60;
							float NewRoutePoints = CompleteRouteData.TotalPoints + GeocacheToAdd.Rating;

							//calculate in meters, since the geocache may not lie on the route in the end
							if (/*NewDistance < CompleteRouteData.Profile.MaxDistance * 1000 &&*/ NewTimeWithGeocaches < CompleteRouteData.Profile.MaxTime * 60)
							{
								CompleteRouteData = ReplaceRoute(CompleteRouteData,RoutingResult.Value.Item1, RoutingResult.Value.Item2, PR);
								CompleteRouteData.AddGeocacheOnRoute(GeocacheToAdd);
								CompleteRouteData.TotalPoints = NewRoutePoints;//Overwrites the addition automatically made in the lne before, to make sure the 
							}
							else
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Adds Geocaches that are rated well and near the route, until the route is full.
		/// </summary>
		/// <returns></returns>
		public bool AddGeocachesByRoutingRating()
		{
			UpdateReachableGeocachesListRoutingRatings();
			List<GeocacheRoutingInformation> GeocachesOrderedByRoutingRating = CompleteRouteData.ReachableGeocaches.OrderByDescending(x=>x.RoutingRating).ToList();
			return false;
			//TODO
		}
		/// <summary>
		/// Takes the current waypoints and uses itinero to optmize the resulting route
		/// </summary>
		/// <returns></returns>
		public bool OptimizeRoute()
		{
			//TODO Route optimization (travelling salesman problem)
			return false;
		}

		private bool AddBestReachableGeocaches(List<Geocache> GeocachesToConsider)
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
						   float Length = GetEstimatedExtraDistance_NewRoute(SuggestionRouteData.partialRoutes[PartialRouteIndex].partialRoute, new Coordinate(SuggestedCache.lat, SuggestedCache.lon));
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

							   Result<Tuple<Route, Route>> RoutingResult = GetPartialRoutes(RouteToInsertIn, SuggestionBaseCache_Info.ResolvedCoordinates);
							   if (!RoutingResult.IsError)
							   {
								   // So one doesn't have to iterate through all routes
								   //TestRouteDistance, as one doesn't know wether this on is taken
								   float NewDistance = SuggestionRouteData.TotalDistance - RouteToInsertIn.TotalDistance + RoutingResult.Value.Item1.TotalDistance + RoutingResult.Value.Item1.TotalDistance;
								   float NewTime = SuggestionRouteData.TotalTime - RouteToInsertIn.TotalTime + RoutingResult.Value.Item1.TotalTime + RoutingResult.Value.Item1.TotalTime;

								   if (NewDistance < App.DB.PercentageOfDistanceInAutoTargetselection_Max * SuggestionRouteData.Profile.MaxDistance * 1000 && NewTime < App.DB.PercentageOfDistanceInAutoTargetselection_Max * SuggestionRouteData.Profile.MaxTime * 60)
								   {
									   Log.AddSubInformation("Added the Geocache to the Route. New Distance:" + NewDistance);
									   SuggestionRouteData = ReplaceRoute(SuggestionRouteData, RoutingResult.Value.Item1, RoutingResult.Value.Item2, SuggestionRouteData.partialRoutes[IndexOfRouteToInsertIn]);
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
				//Save best suggestion
				CompleteRouteData = Suggestions.Find(x => x.TotalPoints == Suggestions.Max(y => y.TotalPoints));
				return true;
			}
			//If no suggestion has been found, just return false
			return false;
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
						 if (CurrentGeocache.EstimatedExtraDistance_NewRoute > (CompleteRouteData.Profile.MaxDistance * 1000 - CompleteRouteData.TotalDistance))
						 {
							 GeocachesToRemove.Add(CurrentGeocache);
						 }
						 else if (CompleteRouteData.GeocachesOnRoute.Contains(CurrentGeocache.geocache))
						 {
							 GeocachesToRemove.Add(CurrentGeocache);
						 }
						 else if (CurrentGeocache.EstimatedExtraDistance_NewRoute <CompleteRouteData.Profile.MaxDistance * 1000 - CompleteRouteData.TotalDistance)
						 {
							 /* If no cache is set as next geocache to insert
							  * If the cache we stumbled accross has more routingpoints than the old cache
							 */
							 if (GeocacheToAdd == null || CurrentGeocache.RoutingRating > GeocacheToAdd.RoutingRating)
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
					Result<Tuple<Route, Route>> RoutingResult = GetPartialRoutes(RouteToInsertIn, GeocacheToAdd.ResolvedCoordinates);

					if (!RoutingResult.IsError)
					{
						float NewDistance = CompleteRouteData.TotalDistance - RouteToInsertIn.TotalDistance + RoutingResult.Value.Item1.TotalDistance + RoutingResult.Value.Item2.TotalDistance;
						float NewTimeWithGeocaches = CompleteRouteData.TotalTime - RouteToInsertIn.TotalTime + RoutingResult.Value.Item1.TotalTime + RoutingResult.Value.Item2.TotalTime + (CompleteRouteData.GeocachesOnRoute.Count + 1) * CompleteRouteData.Profile.TimePerGeocache * 60;
						float NewRoutePoints = CompleteRouteData.TotalPoints + GeocacheToAdd.geocache.Rating;

						//calculate in meters
						if (NewDistance < CompleteRouteData.Profile.MaxDistance * 1000 && NewTimeWithGeocaches < CompleteRouteData.Profile.MaxTime * 60)
						{
							Debug.WriteLine("Added " + GeocacheToAdd.geocache);
							CompleteRouteData = ReplaceRoute(CompleteRouteData, RoutingResult.Value.Item1, RoutingResult.Value.Item2, CompleteRouteData.partialRoutes[IndexOfRouteToInsertIn]);
							CompleteRouteData.AddGeocacheOnRoute(GeocacheToAdd.geocache);
							CompleteRouteData.TotalPoints = NewRoutePoints;//Overwrites the addition automatically made in the lne before, to make sure the 
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

		
	}
}
