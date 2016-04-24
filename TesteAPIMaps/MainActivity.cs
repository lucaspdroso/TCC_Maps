using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Android.Util;

namespace TesteAPIMaps
{
    [Activity(Label = "TesteAPIMaps", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback, GoogleMap.IInfoWindowAdapter, GoogleMap.IOnInfoWindowClickListener, ILocationListener
    {
        private GoogleMap mMap;
        private Location currentLocation;
        private LocationManager locationManager;
        private double lat = 0;
        private double lon = 0;

        string locationProvider;

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria locationServiceCriteria = new Criteria
            {
                Accuracy = Accuracy.Coarse,
                PowerRequirement = Power.Medium
            };

            IList<string> acceptableLocationProviders = locationManager.GetProviders(locationServiceCriteria, true);

            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
            }

            LatLng latlong = new LatLng(lat, lon);

            CameraUpdate posicao = CameraUpdateFactory.NewLatLngZoom(latlong, 10);
            mMap.MoveCamera(posicao);

            MarkerOptions opcoes = new MarkerOptions()
            .SetPosition(latlong)
            .SetTitle("Sua Posição")
            .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure)) //Mudar a cor do marcador
            .SetSnippet("Você se encontra aqui!!");

            mMap.AddMarker(opcoes);

            //mMap.MarkerClick += MMap_MarkerClick;
            mMap.SetInfoWindowAdapter(this);
            mMap.SetOnInfoWindowClickListener(this);

        }

        private void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            LatLng pos = e.Marker.Position;
            mMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(pos, 10));
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mMap.MapType = GoogleMap.MapTypeHybrid;

            // existem outros tipos de mapas que podem ser escolhidos

            //mMap.MapType = GoogleMap.MapTypeNormal;
            //mMap.MapType = GoogleMap.MapTypeSatellite;
            //mMap.MapType = GoogleMap.MapTypeTerrain;

            SetUpMap();
        }

        private void SetUpMap()
        {
            if (mMap == null)
            {
                //FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            //View view = LayoutInflater.Inflate(Resource.Layout.Info, null, false);
            
            //view.FindViewById<TextView>(Resource.Id.txtNome).Text = "Quadra Do Taquaral";
            //view.FindViewById<TextView>(Resource.Id.txtEnd).Text =  "Rua do Taquarebas";
            //view.FindViewById<TextView>(Resource.Id.txtHora).Text = "Status: Com jogo";

            return null;
        }

        public void OnLocationChanged(Location location)
        {
            currentLocation = location;

            if (currentLocation != null)
            {
                lat = currentLocation.Latitude;
                lon = currentLocation.Longitude;
            }
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public void OnInfoWindowClick(Marker marker)
        {
            throw new NotImplementedException(); // o que eu quero fazer quando clicar na janela
        }
    }
}

