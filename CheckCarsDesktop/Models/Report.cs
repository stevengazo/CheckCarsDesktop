﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsAPI.Models
{
    public  abstract class Report
    {
        [Key]
 
        public string ReportId { get; set; } = Guid.NewGuid().ToString();
        public string? Author { get; set; }
        public DateTime Created { get; set; }
        public string? CarPlate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<Photo>? Photos { get; set; } = new List<Photo>();
        public int? CarId { get; set; }  
        public Car? Car { get; set; } 
    }
}


