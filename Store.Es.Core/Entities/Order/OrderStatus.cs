﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Entities.Order
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]
        Pending,
        [EnumMember(Value = "Payment Receieved")]
        PaymentReceieved,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}
