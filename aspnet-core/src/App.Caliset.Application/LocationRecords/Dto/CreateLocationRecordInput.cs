﻿using System;

namespace App.Caliset.LocationRecords
{
    public class CreateLocationRecordInput
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public DateTime When { get; set; }
    }
}