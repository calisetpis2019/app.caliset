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
            if (!System.Diagnostics.EventLog.SourceExists("Caliset"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Caliset", "MyNewLog");
            }
            eventLog1.Source = "Caliset";
            eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Start.");

            Timer timer = new Timer();
            //timer.Interval = TimeSpan.FromMinutes(15).TotalMilliseconds;
            timer.Interval = 60000 * 15; // 60 seconds * 15
            timer.Elapsed += new ElapsedEventHandler(this.ChangeOperationsState);
            timer.Start();
        }

        protected override void OnStop()
        {
        }

        public void ChangeOperationsState(object sender, ElapsedEventArgs args)
        {
            eventLog1.WriteEntry("Start ChangeOperationsState", EventLogEntryType.Information, eventId++);
            string queryString = "UPDATE Operations SET OperationStateId = 2 WHERE OperationStateId = 1 AND Date <= convert(date,GETDATE());";

            using (SqlConnection connection = new SqlConnection("Server = N71412542\\LOCALHOST; Database = CalisetDb; Trusted_Connection = True;"))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

            eventLog1.WriteEntry("Finish ChangeOperationState");

        }
    }
}
