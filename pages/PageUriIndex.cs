using System;

namespace SystemResourceMonitor.pages {
    static class PageUriIndex {
        public static Uri startpage { get; } = new Uri("/pages/StartPage.xaml", UriKind.Relative);
        public static Uri account { get; } = new Uri("/pages/Account.xaml", UriKind.Relative);
        public static Uri accountpage { get; } = new Uri("/pages/AccountPage.xaml", UriKind.Relative);
    }
}
