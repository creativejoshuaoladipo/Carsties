using AuctionServices.Data;
using AuctionsServices.Entites;
using AuctionsServices.Entites.Dtos;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AuctionServices.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;


    public AuctionController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet(Name = "auctions")]
    public async Task<ActionResult<List<AuctionDto>>> GetAuctions()
    {
        var auctions = await _context.Auctions.Include(a => a.Item)
                                       .OrderBy(o => o.Item.Make)
                                       .ToListAsync();

        var auctionsDto = _mapper.Map<List<AuctionDto>>(auctions);
        return auctionsDto;

    }

    [HttpGet("{id}", Name = "GetAuction")]
    public async Task<ActionResult<AuctionDto>> GetAuction(Guid id)
    {
        var auction = await _context.Auctions.Include(a => a.Item).FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
        {
            return NotFound();
        }

        var auctionDto = _mapper.Map<AuctionDto>(auction);

        return auctionDto;
    }

    [HttpPost(Name = "PostAuction")]
    public async Task<ActionResult<Auction>> PostAuction(CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);

        //TODO: get the seller from the identitytoken
        auction.Seller = "test";

        _context.Auctions.Add(auction);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetAuction), new { auction.Id }, _mapper.Map<AuctionDto>(auction));
    }


    [HttpPut("{id}", Name = "UpdateAuction")]
    public async Task<IActionResult> UpdateAuction(Guid id, UpdateAuctionDto updatedAuction)
    {
        var auctionDb = await _context.Auctions.Include(a => a.Item).FirstOrDefaultAsync(a => a.Id == id);

        if (auctionDb == null)
        {
            return NotFound();
        }

        // update the auction
        // updatedAuction.Make 
        // updatedAuction.Model
        // updatedAuction.Year
        // updatedAuction.Mileage
        // updatedAuction.Color

        auctionDb.Item.Make = updatedAuction.Make ?? auctionDb.Item.Make;
        auctionDb.Item.Model = updatedAuction.Model ?? auctionDb.Item.Model;
        auctionDb.Item.Year = updatedAuction.Year ?? auctionDb.Item.Year;
        auctionDb.Item.Mileage = updatedAuction.Mileage ?? auctionDb.Item.Mileage;
        auctionDb.Item.Color = updatedAuction.Color ?? auctionDb.Item.Color;

        var isSuccessful = await _context.SaveChangesAsync() > 0;

        if (isSuccessful)
        {
            return Ok();
        }

        return BadRequest(isSuccessful);
    }


    [HttpPut("AI/{ids}", Name = "PutAuction")]
    public async Task<IActionResult> PutAuction(Guid ids, Auction auction)
    {

        if (ids != auction.Id)
        {
            return BadRequest();
        }

        _context.Entry(auction).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuctionExists(ids))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();


    }


    [HttpDelete("{id}", Name = "DeleteAuction")]
    public async Task<IActionResult> DeleteAuction(int id)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if (auction == null)
        {
            return NotFound();
        }

        _context.Auctions.Remove(auction);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AuctionExists(Guid id)
    {
        return _context.Auctions.Any(e => e.Id == id);
    }
}


