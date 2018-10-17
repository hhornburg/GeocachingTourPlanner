﻿using GeocachingTourPlanner.Types;
using Itinero.LocalGeo;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GeocachingTourPlanner.UI
{
	public static class Markers
	{
		/// <summary>
		/// Currently missing color formatting
		/// </summary>
		/// <param name="geocache"></param>
		/// <returns></returns>
		public static Feature GetGeocacheMarker(Geocache geocache)
		{
			SymbolStyle MarkerStyle = null;

			Category GeocacheCategory;

			if (geocache.ForceInclude)
			{
				GeocacheCategory = Category.ForceInclude;
			}
			else if (geocache.Rating > (App.DB.MinimalRating) + 0.83 * (App.DB.MaximalRating - App.DB.MinimalRating))
			{
				GeocacheCategory = Category.Best_Good;
			}
			else if (geocache.Rating > (App.DB.MinimalRating) + 0.67 * (App.DB.MaximalRating - App.DB.MinimalRating))
			{
				GeocacheCategory = Category.Best_Bad;
			}
			else if (geocache.Rating > (App.DB.MinimalRating) + 0.5 * (App.DB.MaximalRating - App.DB.MinimalRating))
			{
				GeocacheCategory = Category.Medium_Good;
			}
			else if (geocache.Rating > (App.DB.MinimalRating) + 0.33 * (App.DB.MaximalRating - App.DB.MinimalRating))
			{
				GeocacheCategory = Category.Medium_Bad;
			}
			else if (geocache.Rating > (App.DB.MinimalRating) + 0.16 * (App.DB.MaximalRating - App.DB.MinimalRating))
			{
				GeocacheCategory = Category.Worst_Good;
			}
			else
			{
				GeocacheCategory = Category.Worst_Bad;
			}

			if (App.MarkerStyleCache.Where(x => x.Value1 == geocache.Type && (Category)x.Value2 == GeocacheCategory).Count() > 0)
			{
				MarkerStyle = App.MarkerStyleCache.Find(x => x.Value1 == geocache.Type && x.Value2 == (int)GeocacheCategory).Key;
			}
			else
			{
				Image OriginalMarker = Properties.Images.Pin_black;

				ColorMap[] colorMap = new ColorMap[1];
				colorMap[0] = new ColorMap();
				colorMap[0].OldColor = System.Drawing.Color.Black;

				switch (GeocacheCategory)
				{
					case Category.ForceInclude:
						colorMap[0].NewColor = System.Drawing.Color.Blue;
						break;
					case Category.Best_Good:
						colorMap[0].NewColor = System.Drawing.Color.Green;
						break;
					case Category.Best_Bad:
						colorMap[0].NewColor = System.Drawing.Color.GreenYellow;
						break;
					case Category.Medium_Good:
						colorMap[0].NewColor = System.Drawing.Color.Yellow;
						break;
					case Category.Medium_Bad:
						colorMap[0].NewColor = System.Drawing.Color.Orange;
						break;
					case Category.Worst_Good:
						colorMap[0].NewColor = System.Drawing.Color.Red;
						break;
					case Category.Worst_Bad:
						colorMap[0].NewColor = System.Drawing.Color.DarkRed;
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

				Rectangle PinRect = new Rectangle(0, 0, App.DB.MarkerSize, (int)(1.5 * App.DB.MarkerSize));
				Rectangle SymbolRect = new Rectangle(0, 0, App.DB.MarkerSize, App.DB.MarkerSize);

				System.Drawing.Bitmap marker_bmp = new System.Drawing.Bitmap(App.DB.MarkerSize, (int)(1.5 * App.DB.MarkerSize));
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

				MemoryStream image = new MemoryStream();
				marker_bmp.Save(image, ImageFormat.Png);
				image.Position = 0;

				MarkerStyle = new SymbolStyle { BitmapId=BitmapRegistry.Instance.Register(image), SymbolScale = 1.0, SymbolOffset = new Offset(0.0, 0.75*App.DB.MarkerSize) };

				App.MarkerStyleCache.Add(new KeyValueTriple<SymbolStyle, GeocacheType, int>(MarkerStyle, geocache.Type, (int)GeocacheCategory));
			}

			//Create final marker
			Feature GCMarker = new Feature { Geometry = SphericalMercator.FromLonLat(geocache.lon, geocache.lat), [MarkerFields.Type] = MarkerTypes.Geocache, [MarkerFields.Label] = geocache.GCCODE };
			GCMarker.Styles.Add(MarkerStyle);
			GCMarker[MarkerFields.TooltipText] = geocache.GCCODE + "\n" + geocache.Name + "\n" + geocache.Type + " (" + geocache.DateHidden.Date.ToString().Remove(10) + ")\nD: " + geocache.DRating + " T: " + geocache.TRating + " " + geocache.Size + "\nPoints: " + geocache.Rating;

			return GCMarker;
		}

		/// <summary>
		/// Returns a Feature to be added to the mapsui map
		/// </summary>
		/// <returns></returns>
		public static Feature GetWaypointMarker(Waypoint WP)
		{
            SymbolStyle MarkerStyle = null;

            Image OriginalMarker = Properties.Images.Pin_black;

            ColorMap[] colorMap = new ColorMap[1];
            colorMap[0] = new ColorMap();
            colorMap[0].OldColor = System.Drawing.Color.Black;
            colorMap[0].NewColor = System.Drawing.Color.Blue; ImageAttributes PinAttributes = new ImageAttributes();
            PinAttributes.SetRemapTable(colorMap);

            Rectangle PinRect = new Rectangle(0, 0, App.DB.MarkerSize, (int)(1.5 * App.DB.MarkerSize));
            Rectangle SymbolRect = new Rectangle(0, 0, App.DB.MarkerSize, App.DB.MarkerSize);

            System.Drawing.Bitmap marker_bmp = new System.Drawing.Bitmap(App.DB.MarkerSize, (int)(1.5 * App.DB.MarkerSize));
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
            }

            MemoryStream image = new MemoryStream();
            marker_bmp.Save(image, ImageFormat.Png);
            image.Position = 0;

            MarkerStyle = new SymbolStyle { BitmapId = BitmapRegistry.Instance.Register(image), SymbolScale = 1.0, SymbolOffset = new Offset(0, 0.75 * App.DB.MarkerSize) };
            
			Feature StartMarker = new Feature { Geometry = SphericalMercator.FromLonLat(WP.lon, WP.lat), [MarkerFields.Type] = MarkerTypes.Waypoint };
			StartMarker.Styles.Add(MarkerStyle);
			return StartMarker;
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

		/// <summary>
		/// Quasi-enum for Feature fields
		/// </summary>
		public static class MarkerFields
		{
			public static readonly string Type = "Type";
			public static readonly string Label = "Label";
			public static readonly string TooltipText = "TooltipText";
		}
		/// <summary>
		/// Quasi-enum for feature fields
		/// </summary>
		public static class MarkerTypes
		{
			public static readonly string Waypoint = "Waypoint";
			public static readonly string Geocache = "Geocache";
		}

	}
}
