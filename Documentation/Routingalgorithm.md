#Routingalgorithms
##Caching
WaypointRoutingInformation is cached. This contains the RouterPoint of the Waypoint and all calculated Routes from this Waypoint. 
##UpdateReachableGeocaches(PartialRoute partialRoute)
Updates the reachable Geocaches in this specific partial route.
First, all Geocaches are added, that are reachable from the End and Beginning when the Route is replaced.
In a second step, all Geocaches are added, that are reachable, when added to the route. Here only Geocaches should be added, when a large detour is made by the route
##AddGeocachesDirectlyOnRoute()
Lookup the needed routes and calculate if necessary, then check if Route got significantly longer (i.e. 50m)