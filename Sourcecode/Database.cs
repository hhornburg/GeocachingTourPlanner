using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;

namespace Tourenplaner
{
    public class Database
    {
		/// <summary>
		/// Initialisierer für das Serialisieren
		/// </summary>
        public Database() { }

		/// <summary>
		/// Letzer Speicherrt der in einem Dateimangaer benutzt wurde
		/// </summary>
		public string LastUsedFilepath { get; set; }

		//DB Spezifisch
		public string LastGeocachingDB_Filepath { get; set; }
        public string LastRatingDB_Filepath { get; set; }
        public string LastRoutingDB_Filepath { get; set; }

		//Kartenspezifisch
		public PointLatLng LetzteKartenposition { get; set; }
		/// <summary>
		/// Minimale Bewertung die ein Geocache in der Aktuellen Liste erreicht. Wird zum Erstellen der Farbcodierung benutzt
		/// </summary>
		public float MinimalRating { get; set; }
		/// <summary>
		/// Maximale Bewertung die ein Geocache in der Aktuellen Liste erreicht. Wird zum Erstellen der Farbcodierung benutzt
		/// </summary>
		public float MaximalRating { get; set; }

	}
}
