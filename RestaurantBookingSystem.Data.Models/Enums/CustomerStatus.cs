using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.Data.Models.Enums
{
    public enum CustomerStatus
    {
        /// Regular customer - default status
        Regular = 0,

        /// VIP customer - receives priority treatment
        VIP = 1,

        /// Blacklisted - not allowed to make reservations
        Blacklisted = 2
    }
}
