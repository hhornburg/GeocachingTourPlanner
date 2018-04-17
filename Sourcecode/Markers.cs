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
			Category GeocacheCategory;
			Bitmap marker_bmp;

			if (geocache.ForceInclude)
			{
				GeocacheCategory = Category.ForceInclude;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.83 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				GeocacheCategory = Category.Best_Good;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.67 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				GeocacheCategory = Category.Best_Bad;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.5 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				GeocacheCategory = Category.Medium_Good;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.33 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				GeocacheCategory = Category.Medium_Bad;
			}
			else if (geocache.Rating > (Program.DB.MinimalRating) + 0.16 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
			{
				GeocacheCategory = Category.Worst_Good;
			}
			else
			{
				GeocacheCategory = Category.Worst_Bad;
			}

			if (Program.MarkerImageCache.Where(x => x.Value1 == geocache.Type && x.Value2 == (int)GeocacheCategory).Count() > 0)
			{
				marker_bmp = Program.MarkerImageCache.Find(x => x.Value1 == geocache.Type && x.Value2 == (int)GeocacheCategory).Key;
			}
			else
			{
				Image OriginalMarker = Properties.Images.Pin_black;

				ColorMap[] colorMap = new ColorMap[1];
				colorMap[0] = new ColorMap();
				colorMap[0].OldColor = Color.Black;

				switch (GeocacheCategory)
				{
					case Category.ForceInclude:
						colorMap[0].NewColor = Color.Blue;
						break;
					case Category.Best_Good:
						colorMap[0].NewColor = Color.Green;
						break;
					case Category.Best_Bad:
						colorMap[0].NewColor = Color.GreenYellow;
						break;
					case Category.Medium_Good:
						colorMap[0].NewColor = Color.Yellow;
						break;
					case Category.Medium_Bad:
						colorMap[0].NewColor = Color.Orange;
						break;
					case Category.Worst_Good:
						colorMap[0].NewColor = Color.Red;
						break;
					case Category.Worst_Bad:
						colorMap[0].NewColor = Color.DarkRed;
						break;
				}

				ImageAttributes PinAttributes = new ImageAttributes();
				PinAttributes.SetRemapTable(colorMap);

				Image TypeImage;
				switch (geocache.Type)
				{
					case GeocacheType.Ape:
						TypeImage = Properties.Images.type_ape;
						break;
					case GeocacheType.Cito:
						TypeImage = Properties.Images.type_cito;
						break;
					case GeocacheType.EarthCache:
						TypeImage = Properties.Images.type_earth;
						break;
					case GeocacheType.Event:
						TypeImage = Properties.Images.type_event;
						break;
					case GeocacheType.GigaEvent:
						TypeImage = Properties.Images.type_giga;
						break;
					case GeocacheType.Letterbox:
						TypeImage = Properties.Images.type_letterbox;
						break;
					case GeocacheType.MegaEvent:
						TypeImage = Properties.Images.type_mega;
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
				Rectangle SymbolRect = new Rectangle(0, 0, Program.DB.MarkerSize, Program.DB.MarkerSize);

				marker_bmp = new Bitmap(Program.DB.MarkerSize, (int)(1.5 * Program.DB.MarkerSize));
				marker_bmp.SetResolution(OriginalMarker.HorizontalResolution, OriginalMarker.VerticalResolution);

				using (Graphics graphics = Graphics.FromImage(marker_bmp))
				{
					// see https://stackoverflow.com/questions/1922040/resize-an-image-c-sharp
					graphics.CompositingMode = CompositingMode.SourceOver;
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					graphics.SmoothingMode = SmoothingMode.HighQuality;
					graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

					PinAttributes.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(OriginalMarker, PinRect, 0, 0, OriginalMarker.Width, OriginalMarker.Height, GraphicsUnit.Pixel, PinAttributes);

					ImageAttributes SymbolAttribute = new ImageAttributes();
					SymbolAttribute.SetWrapMode(WrapMode.TileFlipXY);

					graphics.DrawImage(TypeImage, SymbolRect, 0, 0, TypeImage.Width, TypeImage.Height, GraphicsUnit.Pixel, PinAttributes);
				}

				Program.MarkerImageCache.Add(new KeyValueTriple<Bitmap, GeocacheType, int>(marker_bmp, geocache.Type, (int)GeocacheCategory));
			}

			//Create final marker
			GMapMarker GCMarker = new GMarkerGoogle(new PointLatLng(geocache.lat, geocache.lon),marker_bmp);

			GCMarker.ToolTipText = geocache.GCCODE + "\n" + geocache.Name + "\n" + geocache.Type + " (" + geocache.DateHidden.Date.ToString().Remove(10) + ")\nD: " + geocache.DRating + " T: " + geocache.TRating + " " + geocache.Size + "\nPoints: " + geocache.Rating;
			GCMarker.Tag = geocache.GCCODE;

			return GCMarker;
		}

		enum Category
		{
			Worst_Bad,
			Worst_Good,
			Medium_Bad,
			Medium_Good,
			Best_Bad,
			Best_Good,
			ForceInclude
		}
	}
}
