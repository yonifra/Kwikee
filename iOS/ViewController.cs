using System;
using System.Linq;
using UIKit;
using FiveMin.Portable.Data;

namespace FiveMin.iOS
{
    public partial class ViewController : UIViewController
    {
        private UITableView _table;

        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            _table = new UITableView (View.Bounds);
            Add (_table);

            FetchData ();
        }

        private void FetchData ()
        {
          //  var entities = await FirebaseManager.Instance.GetAllCompetitions ();
              var tableItems = new string [] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };


          //  var tableItems = entities.Values.Select (i => i.Name).ToArray ();
            _table.Source = new CompetitionSource (tableItems);
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.		
        }
    }
}
