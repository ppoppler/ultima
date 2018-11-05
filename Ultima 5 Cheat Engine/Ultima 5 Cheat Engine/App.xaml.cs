using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace Ultima_5_Cheat_Engine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
