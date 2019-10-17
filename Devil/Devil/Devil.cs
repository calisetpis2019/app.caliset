using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Devil
{
    public partial class Devil : ServiceBase
    {
        static HttpClient client = new HttpClient();
        private int eventId = 1;

        public Devil()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("CalidApp"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "CalidApp", "Logs_CalidApp");
            }
            eventLog1.Source = "CalidApp";
            eventLog1.Log = "Logs_CalidApp";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Start DevilService.");

            Timer timer = new Timer();
            timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            timer.Elapsed += new ElapsedEventHandler(this.ChangeOperationsState);
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stop DevilService.");
        }

        public void ChangeOperationsState(object sender, ElapsedEventArgs args)
        {

            eventLog1.WriteEntry("Start ChangeOperationsState", EventLogEntryType.Information, eventId++);

            string mySetting = System.Configuration.ConfigurationManager.AppSettings["Client"];

            var content = new StringContent("");
            content.Headers.ContentType = null;

            var client = new HttpClient { BaseAddress = new Uri(mySetting) };
            
            var response = client.PostAsync("/api/services/app/Operation/ActvateOperations", content).Result;
            if (response.IsSuccessStatusCode)
            {
                eventLog1.WriteEntry("Finish ChangeOperationState", EventLogEntryType.Information, eventId);
            } else
            {
                eventLog1.WriteEntry("Error " + response.StatusCode, EventLogEntryType.Information, eventId);
            }
        }
    }
}