using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Contacts
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadContacts();
        }

        private async void LoadContacts()
        {
            try
            {
                var permissionsContact = await Utils.PermissionsStatus(Plugin.Permissions.Abstractions.Permission.Contacts);
                if(permissionsContact)
                {
                    var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                    collectionView.ItemsSource = contacts;
                }
            }
            catch(Exception ex)
            {

            }
        }

        void collectionView_ScrollToRequested(System.Object sender, Xamarin.Forms.ScrollToRequestEventArgs e)
        {
            Console.WriteLine(e.ScrollToPosition);
        }

        int band = 0;
        void collectionView_Scrolled(System.Object sender, Xamarin.Forms.ItemsViewScrolledEventArgs e)
        {
            Console.WriteLine(e.VerticalDelta);
            if(e.VerticalDelta <=0)
            {
                if (band == 0)
                {
                    //animar extendido todo
                    band = 1;
                    AnimateExpand(framechat, 25, 120);
                }                                    
            }
            if(e.VerticalDelta > 0)
            {
                if(band == 1)
                {
                    band = 0;
                    AnimateExpand(framechat, 120, 25);//ocultar ,solo mostrar icon
                }
            }
        }

        public void AnimateExpand(View view,int start,int end)
        {
            var animation = new Animation(c => view.WidthRequest = c, start, end);
            animation.Commit(this, "ScaleIt", length: 100, easing: Easing.Linear,
                finished: (v, c) => view.WidthRequest = end, repeat: () => false);
        }
    }
}
