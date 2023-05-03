using System;

namespace SystemResourceMonitor.pages {

    /// <summary>
    /// Provides a URI index for the pages
    /// </summary>
    static class PageUriIndex {
        public readonly static Uri startpage = new Uri("/pages/StartPage.xaml", UriKind.Relative);
        public readonly static Uri account = new Uri("/pages/Account.xaml", UriKind.Relative);
        public readonly static Uri accountpage = new Uri("/pages/AccountPage.xaml", UriKind.Relative);
    }
}
