using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner.Types
{
    /// <summary>
    /// Geocache Class. Inherits its location from Waypoint
    /// </summary>
    public class Geocache:Waypoint
    {
        /// <summary>
        /// GC-Code (Unique ID)
        /// </summary>
        public string GCCODE { get; set; }
        /// <summary>
        /// Name of the Geocache
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Difficulty Rating
        /// </summary>
        public float DRating { get; set; }
        /// <summary>
        /// Terrain Rating
        /// </summary>
        public float TRating { get; set; }
        /// <summary>
        /// Hidden Date
        /// </summary>
        public DateTime DateHidden { get; set; }
        /// <summary>
        /// Wether "Needs Maintenance" Atrribute is set
        /// </summary>
        public bool NeedsMaintenance { get; set; }
        /// <summary>
        /// Geocache Type
        /// </summary>
        public GeocacheType Type { get; set; }
        /// <summary>
        /// Geocache Size
        /// </summary>
        public GeocacheSize Size { get; set; }
        /// <summary>
        /// The Rating calculates with the Rating profile the user created
        /// </summary>
        public float Rating { get; set; }
        /// <summary>
        /// Shouldn't be used anymore, since the user now has a list to which he can add geocaches. Remove if seen.
        /// </summary>
		public bool ForceInclude { get; set; }

        /// <summary>
        /// Returns GCCODE
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return GCCODE;
		}

		/// <summary>
		/// Applies the given ratingprofile to the geocache
		/// </summary>
		/// <param name="Profil"></param>
		public void Rate(Ratingprofile Profil)
        {
            Rating = 0;
			//So Cachetypes that don't have their own rating don't cause exceptions (Types that aren't rated are defaulted to 0 though!)
			if (Profil.TypeRatings.Where(x => x.Key == Type).Count() > 0)
			{
				Rating += (Profil.TypeRatings.Where(x => x.Key == Type).First().Value * Profil.TypePriority);
			}
			else
			{
				Rating += (Profil.TypeRatings.Where(x => x.Key == GeocacheType.Other).First().Value * Profil.TypePriority);
			}

			Rating += (Profil.SizeRatings.Where(x => x.Key == Size).First().Value * Profil.SizePriority);
			Rating += (Profil.DRatings.Where(x => x.Key == DRating).First().Value * Profil.DPriority);
			Rating += (Profil.TRatings.Where(x => x.Key == TRating).First().Value * Profil.TPriority);

			if (Profil.Yearmode == Yearmode.multiply)
            {
                Rating += (Profil.Yearfactor * (DateTime.Now.Year - DateHidden.Year));
            }
            else
            {
                Rating += ((DateTime.Now.Year - DateHidden.Year) * (DateTime.Now.Year - DateHidden.Year)/Profil.Yearfactor);
            }
            
            if (NeedsMaintenance)
            {
                Rating -= Profil.NMPenalty;
            }
        }

		public void toggleForceInclude()
		{
			if (ForceInclude)
			{
				ForceInclude = false;
			}
			else
			{
				ForceInclude = true;
			}
		}
	}

    public enum GeocacheType
    {
        Other,
        Traditional,
        Mystery,
        Virtual,
        Webcam,
        EarthCache,
        Multi,
        Letterbox,
        Wherigo,
		Event,
		MegaEvent,
		GigaEvent,
		Ape,
		Cito
    }

    public enum GeocacheSize
    {
        Other,
        Large,
        Regular,
        Small,
        Micro        
    }

}
