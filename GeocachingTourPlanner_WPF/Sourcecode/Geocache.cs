using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner.Types
{
    public class Geocache:Waypoint
    {
        public string GCCODE { get; set; }
        public string Name { get; set; }
        public float DRating { get; set; }
        public float TRating { get; set; }
        public DateTime DateHidden { get; set; }
        public bool NeedsMaintenance { get; set; }
        public GeocacheType Type { get; set; }
        public GeocacheSize Size { get; set; }
        public float Rating { get; set; }
		public bool ForceInclude { get; set; }

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
