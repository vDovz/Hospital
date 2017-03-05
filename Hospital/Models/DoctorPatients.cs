using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class DoctorPatients
    {
        public Doctor Doctor { get; set; }

        public List<Patient> Patients { get; set; }
    }
}