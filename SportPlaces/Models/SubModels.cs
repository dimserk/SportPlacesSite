﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportPlaces.Models
{
    public class EditRecord
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public double Length { get; set; }
        public static int SportObjectId { get; set; }
        public static int UserId { get; set; }
    }

    public class ExpandedRecord
    {
        public DateTime Date { get; set; }
        public double Length { get; set; }
        public int SportObjectId { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
    }

    public class SportObjectsRecords
    {
        public DateTime Date { get; set; }
        public int SportObjectId { get; set; }
    }
}
