//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourOperator.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tourist : SystemUser
    {
        public string TouristUsername { get; set; }
        public string Country { get; set; }
        public Nullable<System.Guid> ActivationCode { get; set; }
        public string TouristType { get; set; }
    }
}
