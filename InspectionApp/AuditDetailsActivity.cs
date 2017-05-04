using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLayer;
using BusinessObjects;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;

namespace InspectionApp
{
    [Activity(Label = "AuditDetails")]
    public class AuditDetailsActivity : Activity,ILocationListener
    {
        List<AuditTemplate> templateList = null;
        LocationManager locationManager;
        string locationProvider;
        Location currentLocation;
        string locationCoordinates;
        private Template manageTemplate = new Template();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Template template = new Template();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AuditDetails);
            // Create your application here
            template.SetContext(this);
            templateList = template.GetTemplates();

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, templateList);
            adapter.SetDropDownViewResource
                (Android.Resource.Layout.SimpleSpinnerDropDownItem);

            Spinner spnTemplate = FindViewById<Spinner>(Resource.Id.spnTemplates);
            spnTemplate.Adapter = adapter;

            Button saveAditDetails = FindViewById<Button>(Resource.Id.next);
            saveAditDetails.Click += SaveAditDetails_Click;
        }

        private void SaveAditDetails_Click(object sender, EventArgs e)
        {
            TextView txtLocation = FindViewById<TextView>(Resource.Id.txtLocation);
            Spinner spnTemplate = FindViewById<Spinner>(Resource.Id.spnTemplates);
            int templateId = templateList.ElementAt(spnTemplate.SelectedItemPosition).Id;
            AuditDetails auditDetails = new AuditDetails();
            auditDetails.Location = txtLocation.Text;
            auditDetails.TemplateId = templateId;
            auditDetails.UserId = "CGI Admin";//hardcoded for POC
            auditDetails.GPSCoordinate = locationCoordinates;
            manageTemplate.SaveAudit(auditDetails);
        }
        private void InitializeLocationManager()
        {
            
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
            }
            
        }

        public void OnLocationChanged(Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
                //Error Message
            }
            else
            {
                locationCoordinates = currentLocation.Latitude.ToString() +","+ currentLocation.Longitude.ToString();                
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
    }
}