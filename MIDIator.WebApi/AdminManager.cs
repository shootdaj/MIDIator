using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MIDIator.Web
{
    public class AdminManager : IDisposable
    {
        #region Singleton

        public static void Instantiate()
        {
            Instance = new AdminManager();
        }

        public static AdminManager Instance { get; private set; }

        #endregion

        public Subject<string> OnShutdown = new Subject<string>();
        public void Dispose()
        {
            OnShutdown = null;
        }
    }
}
