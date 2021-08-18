using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public enum NotificationType
    {
        Information,
        Success,
        Warning,
        Danger,
        Confirmation,
        Default
    }

    public enum PopupButtonClass
    {
        Information,
        Success,
        Warning,
        Danger,
        Confirmation,
        Default
    }

    public enum Status
    {
        All = 0,
        Forward = 1,
        Return = 2,
        QueryRaised = 3,
        Draft = 4,
        Submitted = 5,
        Approved = 7,
        Rejected = 8,
        ReturnForward = 9,
        Cancelled = 10,
        Suspended = 11


        //NoPreviousRole=0,
        //Submitted=1,
        //Rejected=2,
        //RaiseQuery=3,
        //Pending=4,
        //Approved=5,
        //Return=6,
        //Forward=7
    }

    public enum FormStatus
    {
        [EnumMember(Value = "0")]
        Empty = 0,
        [EnumMember(Value = "1")]
        PartiallySaved = 1,
        [EnumMember(Value = "2")]
        Completed = 2
    }

    public enum ApplicationType
    {
        Grant,
        Resubmit,
        Renewal
    }

    public enum FacilityType
    {
        Main,
        Branch
    }
    public enum RoleTypes
    {
        SuperAdmin = 1,
        SuperUser = 2,
        DepartmentAdmin = 3,
        DepartmentUser = 4,
        Applicant = 5,
        Commissioner = 6
    }

    //Old
    public enum DocumentType
    {
        Approval = 1,
        Required = 2
    }

    
}
