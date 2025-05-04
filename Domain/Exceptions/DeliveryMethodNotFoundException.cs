using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DeliveryMethodNotFoundException(int id) 
        : NotFoundException($"No Delivery Method With id : {id} was found!")
    {
    }
}
