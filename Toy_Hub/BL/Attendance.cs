using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyHub.BL
{
    public class Attendance
    {
       
        public int StaffID { get; set; }
        public int AttendanceID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? LeavingTime { get; set; } // Nullable TimeSpan

        public Attendance( int staffID, TimeSpan? arrivalTime, TimeSpan? leavingTime)
        {
         
            StaffID = staffID;
            AttendanceDate = DateTime.Today; // Set to current date
            ArrivalTime = arrivalTime;
            LeavingTime = leavingTime; // Set to current time
        }
        public Attendance(int staffID,int attendanceID, TimeSpan? arrivalTime, TimeSpan? leavingTime)
        {
            AttendanceID=attendanceID;
            StaffID = staffID;
            AttendanceDate = DateTime.Today; // Set to current date
            ArrivalTime = arrivalTime;
            LeavingTime = leavingTime; // Set to current time
        }
        public Attendance(int attendanceID , TimeSpan? leavingTime)
        {
            AttendanceID = attendanceID;
           
            LeavingTime = leavingTime; // Set to current time
        }


    }
}
