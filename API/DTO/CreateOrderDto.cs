using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.OrderAggregate;

namespace API.DTO
{
    public class CreateOrderDto
    {
        public bool SaveAddress { get; set; }

        public ShippingAddress ShipppingAddress { get; set; }
    }
}