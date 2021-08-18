using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;
using System.Data;

namespace Capstone.BAL
{
    public class DesignationBAL
    {
        DesignationDAL objDesignationDal;
         

        public List<CabserviceModel> SaveCabservice(CabserviceModel model)
        {
            objDesignationDal = new DesignationDAL();
            DataTable dt = objDesignationDal.SaveCabsdervice(model);
            if (dt == null)
                return null;
            return ConvertToCabList(dt);
        }
        public List<OtherServiceModel> SaveOtherervice(OtherServiceModel model)
        {
            objDesignationDal = new DesignationDAL();
            DataTable dt = objDesignationDal.SaveOthersdervice(model);
            if (dt == null)
                return null;
            return ConvertTootherList(dt);
        }
        public List<CabserviceModel> GetCabservice(int userid)
        {
            objDesignationDal = new DesignationDAL();
            DataTable dt = objDesignationDal.GetCabservice(userid);
            if (dt == null)
                return null;
            return ConvertToCabList(dt);
        }
        public List<OtherServiceModel> GetOtherservice(int userid,int serviceid)
        {
            objDesignationDal = new DesignationDAL();
            DataTable dt = objDesignationDal.GetOtherservice(userid, serviceid);
            if (dt == null)
                return null;
            return ConvertTootherList(dt);
        }
   
        private List<CabserviceModel> ConvertToCabList(DataTable dt)
        {
            List<CabserviceModel> cblst = new List<CabserviceModel>();
            foreach (DataRow row in dt.Rows)
            {
                CabserviceModel cab = new CabserviceModel();
                cab.Id = Convert.ToInt32(row["id"]);
                cab.PNR = row["flightNo"].ToString();
                cab.PhoneNumber = row["bookingrefNo"].ToString();
                cab.DropingAddress = row["DropingAddress"].ToString();
                cab.ArrivalDate= Convert.ToString(row["arrivalDate"]);
                cab.DepartureDate = Convert.ToString(row["departureDate"]);

                cab.AirportId = Convert.ToInt32(row["airportId"]);
               cab.AirportName = Convert.ToString(row["airportname"]);

                cblst.Add(cab);
            }
            return cblst;
        }

        private List<OtherServiceModel> ConvertTootherList(DataTable dt)
        {
            List<OtherServiceModel> cblst = new List<OtherServiceModel>();
            foreach (DataRow row in dt.Rows)
            {
                OtherServiceModel other = new OtherServiceModel();
                other.Id = Convert.ToInt32(row["id"]);
                other.PNR = row["PNR"].ToString();
                other.DropingAddress = row["droppingAddress"].ToString();
                other.PostalCode = row["PostalCode"].ToString();
                other.Timeslot = row["TimeSlot"].ToString();
                other.PickupDate = row["PickupDate"].ToString();
 

                cblst.Add(other);
            }
            return cblst;
        }
    }
}
