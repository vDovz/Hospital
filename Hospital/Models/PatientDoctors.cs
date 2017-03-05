using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class PatientDoctors
    {
        public Patient Patient { get; set; }

        public List<Doctor> Doctors { get; set; }

    }
}