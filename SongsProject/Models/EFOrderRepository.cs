using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace SongsProject.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;
        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        //I
        //used the Include and ThenInclude methods to specify that when an Order object is read from the
        //database, the collection associated with the Lines property should also be loaded along with each
        //song object associated with each collection object.
        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Song);

        public void SaveOrder(Order order)
        {
            //For the song
            //objects, this means that Entity Framework Core tries to write objects that have already been stored,
            //which causes an error.To avoid this problem, I notify Entity Framework Core that the objects exist and
            //shouldn’t be stored in the database unless they are modified,
                        context.AttachRange(order.Lines.Select(l => l.Song));
                        if (order.OrderID == 0)
                        {
                            context.Orders.Add(order);
                        }
                        context.SaveChanges();
        }
    }
}