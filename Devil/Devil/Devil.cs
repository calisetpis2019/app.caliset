using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Devil
{
    public partial class Devil : ServiceBase
    {
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
            timer.Interval = TimeSpan.FromMinutes(15).TotalMilliseconds;
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

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            string queryString = "UPDATE Operations SET OperationStateId = 2 WHERE OperationStateId = 1 AND Date <= convert(date,GETDATE());";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();

                    eventLog1.WriteEntry("Finish ChangeOperationState", EventLogEntryType.Information, eventId);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    eventLog1.WriteEntry(ex.Message, EventLogEntryType.Information, eventId);
                }
                
            }

        }
    }
}
