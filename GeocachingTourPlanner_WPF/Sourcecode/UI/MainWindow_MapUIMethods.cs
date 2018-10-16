﻿using GeocachingTourPlanner.Routing;
using GeocachingTourPlanner.Types;
using Itinero.LocalGeo;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.UI.Wpf;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GeocachingTourPlanner.UI
{
    public partial class MainWindow : Window
    {
        #region UI Events
        private void Map_Enter(object sender, EventArgs e)
        {
            Map_RenewGeocacheLayer();
        }

        private void mapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapContextMenu.HideContextMenu();
            MapTooltip.HideTooltip();

            MapInfo mapInfo = GetMapInfo(e);
            if (mapInfo != null && mapInfo.Layer != null && mapInfo.Layer.Name == Layers.GeocacheLayer)
            {
                Process.Start("http://coord.info/" + mapInfo.Feature[Markers.MarkerFields.Label]);
            }

            Mapsui.Geometries.Point Coordinates = SphericalMercator.ToLonLat(mapInfo.WorldPosition.X, mapInfo.WorldPosition.Y);
            App.DB.LastMapResolution = mapControl.Viewport.Resolution;
            App.DB.LastMapPosition = new Coordinate((float)Coordinates.Y, (float)Coordinates.X);
            e.Handled = true;
        }

        private void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            MapInfo mapInfo = GetMapInfo(e);
            if (mapInfo != null && mapInfo.Feature != null)
            {
                MapTooltip.ShowTooltip((string)mapInfo.Feature[Markers.MarkerFields.TooltipText], new System.Windows.Point(mapInfo.ScreenPosition.X, mapInfo.ScreenPosition.Y));
            }
        }

        private void mapControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapInfo mapInfo = GetMapInfo(e);
            if (mapInfo != null)
            {
                MapContextMenu.ShowContextMenu(mapInfo);
            }
        }

        private void MinRatingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.DB.MinAllowedRating = MinRatingSlider.Value;
            MinAllowedRatingTextBlock.Text = MinRatingSlider.Value.ToString("F0");
            Map_RenewGeocacheLayer();
        }
        #endregion

        #region Methods
        private MapInfo GetMapInfo(MouseEventArgs e)
        {
            try
            {
                Mapsui.Geometries.Point Location = e.GetPosition(mapControl).ToMapsui();
                return mapControl.GetMapInfo(Location);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates Map
        /// </summary>
        public void Map_RenewGeocacheLayer()
        {
            if (mapControl != null)//Occurs during startup
            {
                //GeocacheLayer
                if (mapControl.Map.Layers.Count(x => x.Name == Layers.GeocacheLayer) > 0)
                {
                    foreach (WritableLayer GClayer in mapControl.Map.Layers.Where(x => x.Name == Layers.GeocacheLayer).ToList())
                    {
                        mapControl.Map.Layers.Remove(GClayer);
                    }
                }
                WritableLayer GeocacheLayer = new WritableLayer
                {
                    Name = Layers.GeocacheLayer,
                    Style = null
                };

                foreach (Geocache GC in App.Geocaches.Where(x => x.Rating >= App.DB.MinAllowedRating))
                {
                    GeocacheLayer.Add(Markers.GetGeocacheMarker(GC));
                }
                GeocacheLayer.IsMapInfoLayer = true;
                mapControl.Map.Layers.Add(GeocacheLayer);

                //Set Views
                if (App.DB.LastMapResolution == 0)
                {
                    App.DB.LastMapResolution = 5;
                }
                mapControl.Refresh();
            }
        }

        private object Map_WaypointLocker = new object();
        /// <summary>
        /// Updates Waypoints shown on Map
        /// </summary>
        public void Map_RenewWaypointLayer(object sender, EventArgs eventArgs)
        {
            if (App.DB.ActiveRoute != null)
            {
                lock (Map_WaypointLocker)
                {
                    //Waypointlayer
                    if (mapControl.Map.Layers.Count(x => x.Name == Layers.WaypointLayer) > 0)
                    {
                        foreach (WritableLayer WPLayer in mapControl.Map.Layers.Where(x => x.Name == Layers.WaypointLayer).ToList())
                        {
                            mapControl.Map.Layers.Remove(WPLayer);
                        }
                    }
                    WritableLayer Waypointlayer = new WritableLayer
                    {
                        Name = Layers.WaypointLayer,
                        Style = null
                    };

                    foreach (Waypoint WP in App.DB.ActiveRoute.CompleteRouteData.Waypoints)
                    {
                        if (WP.GetType() != typeof(Geocache))
                        {
                            Waypointlayer.Add(Markers.GetWaypointMarker(WP));
                        }
                        else
                        {
                            Waypointlayer.Add(Markers.GetGeocacheMarker((Geocache)WP));
                        }
                    }
                    Waypointlayer.IsMapInfoLayer = true;
                    mapControl.Map.Layers.Add(Waypointlayer);
                    mapControl.Refresh();
                }
            }
        }

        public void Map_NavigateToLastVisited()
        {
            mapControl.Viewport.Resolution = App.DB.LastMapResolution;
            mapControl.Navigator.NavigateTo(SphericalMercator.FromLonLat(App.DB.LastMapPosition.Longitude, App.DB.LastMapPosition.Latitude));
        }

        private object Map_RouteLocker = new object();
        /// <summary>
        /// Updates the Route on the map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listChangedEventArgs"></param>
        /// <returns></returns>
        public void Map_RenewCurrentRoute(object sender, EventArgs listChangedEventArgs)
        {
            if (mapControl != null)//1. Occurs during startup
            {
                //This long lock needed, to prevent multiple parallel additions to the map
                lock (Map_RouteLocker)
                {
                    //Remove Current Route in any case
                    if (mapControl.Map.Layers.Count(x => x.Name == Layers.CurrentRouteLayer) > 0)
                    {
                        foreach (WritableLayer GClayer in mapControl.Map.Layers.Where(x => x.Name == Layers.CurrentRouteLayer).ToList())
                        {
                            mapControl.Map.Layers.Remove(GClayer);
                        }
                    }


                    //Workaround for Mapsui issue #525
                    if (App.DB.ActiveRoute != null && App.DB.ActiveRoute.CompleteRouteData.PartialRoutes.Count > 0)
                    {
                        WritableLayer RouteLayer = new WritableLayer
                        {
                            Name = Layers.CurrentRouteLayer,
                            Style = new VectorStyle
                            {
                                Fill = null,
                                Outline = null,
                                Line = { Color = Color.FromString("Blue"), Width = 4 }
                            }
                        };

                        LineString Route = new LineString();
                        lock (App.DB.ActiveRoute.CompleteRouteData.PartialRouteLocker)
                        {
                            for (int i = 0; i < App.DB.ActiveRoute.CompleteRouteData.PartialRoutes.Count; i++)
                            {
                                PartialRoute CurrentPartialRoute = App.DB.ActiveRoute.CompleteRouteData.PartialRoutes[i];
                                Route.Vertices.Add(SphericalMercator.FromLonLat(CurrentPartialRoute.From.lon, CurrentPartialRoute.From.lat));
                                for (int j = 0; j < CurrentPartialRoute.Route.Shape.Count(); j++)
                                {
                                    Coordinate point = CurrentPartialRoute.Route.Shape[j];
                                    Route.Vertices.Add(SphericalMercator.FromLonLat(point.Longitude, point.Latitude));
                                }
                                Route.Vertices.Add(SphericalMercator.FromLonLat(CurrentPartialRoute.To.lon, CurrentPartialRoute.To.lat));
                            }
                        }
                        RouteLayer.Add(new Feature { Geometry = Route });
                        RouteLayer.IsMapInfoLayer = true;
                        mapControl.Map.Layers.Add(RouteLayer);
                    }

                    //Set Views
                    if (App.DB.LastMapResolution == 0)
                    {
                        App.DB.LastMapResolution = 5;
                    }
                    mapControl.Refresh();
                }
            }
        }

        /// <summary>
        /// Sets the correct values for the slider from the DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateSliderMinMax(object sender, EventArgs args)
        {
            MinRatingSlider.Minimum = App.DB.MinimalRating;
            MinRatingSlider.Maximum = App.DB.MaximalRating;
            MinRatingSlider.Value = App.DB.MinAllowedRating;
        }
        #endregion
    }
}
