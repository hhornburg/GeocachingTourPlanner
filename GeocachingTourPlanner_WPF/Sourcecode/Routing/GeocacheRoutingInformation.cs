using GeocachingTourPlanner.Types;
using Itinero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeocachingTourPlanner.Routing
{
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
        private float EstimatedExtraDistance_field = -1;


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
