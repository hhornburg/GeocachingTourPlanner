﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner
{
    public class Geocache
    {
        public float lat { get; set; }
        public float lon { get; set; }
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
		public void Rate(Ratingprofile Profil)
        {
            Rating = 0;
            Rating += (Profil.TypeRatings.Where(x=>x.Key==Type).First().Value * Profil.TypePriority);
            Rating += (Profil.SizeRatings.Where(x=>x.Key==Size).First().Value * Profil.SizePriority);
            Rating += (Profil.DRatings.Where(x =>x.Key==DRating).First().Value * Profil.DPriority);
            Rating += (Profil.TRatings.Where(x =>x.Key==TRating).First().Value * Profil.TPriority);
            if (Profil.Yearmode)
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
