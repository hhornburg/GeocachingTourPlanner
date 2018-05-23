using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner
{
    public class Ratingprofile
    {
        public string Name { get; set; }
        public List<SerializableKeyValuePair<GeocacheType, int>> TypeRatings { get; set;}
        public int TypePriority { get; set; }
        public List<SerializableKeyValuePair<GeocacheSize, int>> SizeRatings { get; set;}
        public int SizePriority { get; set; }
        public List<SerializableKeyValuePair<float, int>> DRatings { get; set;}
        public int DPriority { get; set; }
        public List<SerializableKeyValuePair<float, int>> TRatings { get; set;}
        public int TPriority { get; set; }
        public int Yearfactor { get; set; }
        public Yearmode Yearmode { get; set; }
        /// <summary>
        /// Punkte die abgezogen werden wenn der Needs Maintenance Flag gesetzt ist
        /// </summary>
        public int NMPenalty { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

	public enum Yearmode
	{
		multiply,
		square_n_divide
	}
}
