using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2526
{
    public class ATM_Structure
    {
        private string NAME;
        private double LAT;
        private double LON;
   
        public ATM_Structure(double lat, double lon, string name ) { // declares a constructor that will be used to create the object of the ATMS

            this.NAME = name;
            this.LAT = lat; 
            this.LON = lon; 
        
        }
        public double Lat
        {  // declaring a Properties function for latitude
            get { return LAT; }  // gets the latitude  from the privet variable latitude
            set { LAT = value; } // sets it to the latitude  variable to the value of the privet latitude variable 
        } 
        public double Lon
        {  // declaring a Properties function for longitude
            get { return LON; } // gets the name  from the privet variable longitude
            set { LON = value; } // sets it to the longitude variable to the value of the privet longitude variable  
        }
        public string Name
        { // declaring a Properties function for name 
            get {return NAME; } // gets the name  from the privet variable name 
            set {NAME = value; }  // sets it to the name variable to the value of the privet name varaible 
        }
    }
}
