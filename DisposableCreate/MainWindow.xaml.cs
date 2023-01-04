using DisposableCreate.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DisposableCreate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IEnumerable<string> _newsItems;
        private bool _isBusy;

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public bool IsBusy
        {
            get => this._isBusy;
            set
            {
                if (value == this._isBusy)
                {
                    return;
                }

                this._isBusy = value;
                this.OnPropertyChanged();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.RefreshNewsAsync();
        }

        public IEnumerable<string> NewsItems
        {
            get => this._newsItems;
            set
            {
                if (Equals(value, this._newsItems))
                {
                    return;
                }

                this._newsItems = value;
                this.OnPropertyChanged();
            }
        }

        private async void RefreshNewsAsync()
        {
            this.IsBusy = true;
            this.NewsItems = Enumerable.Empty<string>();
            using (Disposable.Create(() => this.IsBusy = false))
            {
                this.NewsItems = await this.DownloadNewsItems();
            }
        }

        private async void RefreshNews2Async()
        {
            this.NewsItems = Enumerable.Empty<string>();
            using (this.StartBusy())
            {
                this.NewsItems = await this.DownloadNewsItems();
            }
        }

        public IDisposable StartBusy()
        {
            this.IsBusy = true;
            return new ContextDisposable(
                SynchronizationContext.Current,
                Disposable.Create(() => this.IsBusy = false));
        }

        private async Task<IEnumerable<string>> DownloadNewsItems()
        {
            await Task.Delay(2000);
            return Enumerable.Range(1, 10).Select(i => "News Items " + i);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
