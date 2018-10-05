# Databases
## Which exist?
* DB: Holds all information that is not a list of Routes, Profiles or Geocaches
* RatingprofileDB: Holds Ratingprofiles
* RoutingprofileDB: Holds Routingprofiles
* Routes: Holds Routes
* Geocaches: Holdes Geocaches
* RouterDB: Used by Itinero for RoutingprofileDB
## How are they kept updated
* DB: Whenever a value is changed it is written to the drive
* All others: Call Fileoperations.backup(Database Name); If the Filepath is not set, on backup a File selection Window is promted