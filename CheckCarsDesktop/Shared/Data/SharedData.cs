using CheckCarsDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsDesktop.Shared.Data
{
    public static class SharedData
    {
        public static string Token="";
        public static EntryExitReport EntryExitReport = new EntryExitReport();
        public static IssueReport IssueReport = new IssueReport();
        public static CrashReport CrashReport = new CrashReport();
    }
}
