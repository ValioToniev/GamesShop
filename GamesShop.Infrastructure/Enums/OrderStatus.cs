using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Infrastructure.Enums
{
    public enum OrderStatus
    {
        Pending,    // Очаква одобрение
        Approved,   // Одобрена
        Shipped,    // Изпратена
        Delivered   // Доставена
    }
}
