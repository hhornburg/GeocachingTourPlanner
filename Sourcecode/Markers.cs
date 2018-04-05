using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace GeocachingTourPlanner
{
	public static class Markers
	{
		public  static GMapMarker GetGeocacheMarker(Geocache geocache)
		{

			Image OriginalMarker = Properties.Images.Pin_black;

			ColorMap[] colorMap = new ColorMap[1];
			colorMap[0] = new ColorMap();
			colorMap[0].OldColor = Color.Black;

			if (geocache.ForceInclude)
			{
				colorMap[0].NewColor = Color.Blue;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.83 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				colorMap[0].NewColor = Color.Green;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.67 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				colorMap[0].NewColor = Color.GreenYellow;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.5 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				colorMap[0].NewColor = Color.Yellow;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.33 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				colorMap[0].NewColor = Color.Orange;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.16 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				colorMap[0].NewColor = Color.Red;
			}
			else
			{
				colorMap[0].NewColor = Color.DarkRed;
			}
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetRemapTable(colorMap);

			//Bitmap TypeImage = new Bitmap(); //Has to be 12x12 png
			
			Rectangle rect = new Rectangle(0, 0, OriginalMarker.Width, OriginalMarker.Height);
			Bitmap marker_bmp = new Bitmap(OriginalMarker.Width, OriginalMarker.Height);
			using (Graphics graphics = Graphics.FromImage(marker_bmp))
			{
				graphics.DrawImage(OriginalMarker, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attributes);
				//graphics.DrawImage(TypeImage, 3, 3);
			}


			//Create final marker
			GMapMarker GCMarker = new GMarkerGoogle(new PointLatLng(geocache.lat, geocache.lon),marker_bmp);

			GCMarker.ToolTipText = geocache.GCCODE + "\n" + geocache.Name + "\n" + geocache.Type + " (" + geocache.DateHidden.Date.ToString().Remove(10) + ")\nD: " + geocache.DRating + " T: " + geocache.TRating + " " + geocache.Size + "\nPoints: " + geocache.Rating;
			GCMarker.Tag = geocache.GCCODE;

			return GCMarker;
		}
	}
}
