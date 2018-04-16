using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
			ImageAttributes PinAttributes = new ImageAttributes();
			PinAttributes.SetRemapTable(colorMap);

			Image TypeImage;
			switch (geocache.Type)
			{
				case GeocacheType.EarthCache:
					TypeImage = Properties.Images.type_earth;
					break;
				case GeocacheType.Letterbox:
					TypeImage = Properties.Images.type_letterbox;
					break;
				case GeocacheType.Multi:
					TypeImage = Properties.Images.type_multi;
					break;
				case GeocacheType.Mystery:
					TypeImage = Properties.Images.type_mystery;
					break;
				case GeocacheType.Traditional:
					TypeImage = Properties.Images.type_traditional;
					break;
				case GeocacheType.Virtual:
					TypeImage = Properties.Images.type_virtual;
					break;
				case GeocacheType.Webcam:
					TypeImage = Properties.Images.type_webcam;
					break;
				case GeocacheType.Wherigo:
					TypeImage = Properties.Images.type_wherigo;
					break;
				default:
					TypeImage = Properties.Images.type_unknown; // Currently events and rare cache tyoes fall under this.
					break;
			}

			Rectangle PinRect = new Rectangle(0, 0, Program.DB.MarkerSize, (int)(1.5 * Program.DB.MarkerSize));
			Rectangle SymbolRect = new Rectangle((int)((Program.DB.MarkerSize - 0.9 * Program.DB.MarkerSize) / 2), (int)((Program.DB.MarkerSize - 0.9 * Program.DB.MarkerSize) / 2), (int)(0.9 * Program.DB.MarkerSize), (int)(0.9 * Program.DB.MarkerSize));
			Bitmap marker_bmp = new Bitmap(Program.DB.MarkerSize, (int)(1.5 * Program.DB.MarkerSize));
			using (Graphics graphics = Graphics.FromImage(marker_bmp))
			{
				// see https://stackoverflow.com/questions/1922040/resize-an-image-c-sharp
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				PinAttributes.SetWrapMode(WrapMode.TileFlipXY);
				graphics.DrawImage(OriginalMarker, PinRect, 0, 0, PinRect.Width, PinRect.Height, GraphicsUnit.Pixel, PinAttributes);

				ImageAttributes SymbolAttribute = new ImageAttributes();
				SymbolAttribute.SetWrapMode(WrapMode.TileFlipXY);

				graphics.DrawImage(TypeImage, SymbolRect, SymbolRect.X, SymbolRect.Y, SymbolRect.Width, SymbolRect.Height, GraphicsUnit.Pixel, PinAttributes);
			}


			//Create final marker
			GMapMarker GCMarker = new GMarkerGoogle(new PointLatLng(geocache.lat, geocache.lon),marker_bmp);

			GCMarker.ToolTipText = geocache.GCCODE + "\n" + geocache.Name + "\n" + geocache.Type + " (" + geocache.DateHidden.Date.ToString().Remove(10) + ")\nD: " + geocache.DRating + " T: " + geocache.TRating + " " + geocache.Size + "\nPoints: " + geocache.Rating;
			GCMarker.Tag = geocache.GCCODE;

			return GCMarker;
		}
	}
}
