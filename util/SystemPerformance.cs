using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemResourceMonitor.util {
    class SystemPerformance {
        public Dictionary<string, PerformanceCounter> SysCounters { get; }

        public SystemPerformance() { 
            SysCounters = new() {
            { "CpuUtil", new PerformanceCounter("Processor Information", "% Processor Utility", "_Total", true) }
                //Pending New Performance Counters...
            };
        }
    }
}
