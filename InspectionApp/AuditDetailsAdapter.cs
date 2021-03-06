using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Views;
using Android.Widget;
using BusinessObjects;
using BusinessLayer;



namespace InspectionApp
{
    public class AuditDetailsAdapter :BaseAdapter
    {
        List<AuditDetails> _auditDetailList;
        Activity _activity;
        private Template manageTemplate = new Template();

        public AuditDetailsAdapter(Activity activity)
        {
            _activity = activity;
            manageTemplate.SetContext(_activity);
            FillAuditList();            
        }

        void FillAuditList()
        {
            _auditDetailList = manageTemplate.GetAllAudit();
           
        }

        public override int Count
        {
            get { return _auditDetailList.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            // could wrap a Contact in a Java.Lang.Object
            // to return it here if needed
            return null;
        }

        public override long GetItemId(int position)
        {
            return _auditDetailList[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(
                Resource.Layout.AuditDetailItem, parent, false);
            var auditDescription = view.FindViewById<TextView>(Resource.Id.AuditDescription);
            var btnViewAudit = view.FindViewById<Button>(Resource.Id.btnView);
            auditDescription.Text = string.Concat(_auditDetailList[position].Location, "-", _auditDetailList[position].UserId);


            if (_auditDetailList[position].Id == 0)
            {
                auditDescription = view.FindViewById<TextView>(Resource.Id.AuditDescription);
                btnViewAudit = view.FindViewById<Button>(Resource.Id.btnView);
                auditDescription.Text = "No audits are created yet";
                btnViewAudit.Visibility = ViewStates.Gone;
            }
            else
            {
                auditDescription.Text = string.Concat(_auditDetailList[position].Location, "-", _auditDetailList[position].UserId);
                btnViewAudit.Tag = _auditDetailList[position].Id;
                btnViewAudit.Click += delegate {
                    btnOneClick((int)btnViewAudit.Tag);
                };
            }
            return view;
        }

        void btnOneClick(int id)
        {
            var activity2 = new Intent(_activity, typeof(AuditQuesAnswersActivity));
            activity2.PutExtra("auditId", id.ToString());
            _activity.StartActivity(activity2);
        }
    }
}