using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourenplaner
{
    public class Ratingprofile
    {
        public string Name;
        public List<KeyValuePair<GeocacheType, int>> TypeRatings { get; set;}
        public int TypePriority { get; set; }
        public List<KeyValuePair<GeocacheSize, int>> SizeRatings { get; set;}
        public int SizePriority { get; set; }
        public List<KeyValuePair<float, int>> DRatings { get; set;}
        public int DPriority { get; set; }
        public List<KeyValuePair<float, int>> TRatings { get; set;}
        public int TPriority { get; set; }
        public int Yearfactor { get; set; }
        /// <summary>
        /// true=multiplizieren, false=quadrieren
        /// </summary>
        public bool Yearmode { get; set; }
        /// <summary>
        /// Punkte die abgezogen werden wenn der Needs Maintenance Flag gesetzt ist
        /// </summary>
        public int NMPenalty { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
