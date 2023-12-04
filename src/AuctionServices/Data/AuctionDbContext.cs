using AuctionsServices.Entites;
using Microsoft.EntityFrameworkCore;

namespace AuctionServices.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
        {
        }
        public DbSet<Auction> Auctions { get; set; }
    }
}

