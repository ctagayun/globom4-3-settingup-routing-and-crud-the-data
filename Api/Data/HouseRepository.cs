using Microsoft.EntityFrameworkCore;

public interface IHouseRepository
{
    Task<List<HouseDto>> GetAll();
    Task<HouseDetailDto?> Get(int id);
    
    //Task<HouseDetailDto> Add(HouseDetailDto house);
    //Task<HouseDetailDto> Update(HouseDetailDto house);
    //Task Delete(int id);
}

public class HouseRepository : IHouseRepository
{
    private readonly HouseDbContext context;

    private static HouseDetailDto EntityToDetailDto(HouseEntity e)
    {
        return new HouseDetailDto(e.Id, e.Address, e.Country, e.Price,  e.Description, e.Photo);
    }

    private static void DtoToEntity(HouseDetailDto dto, HouseEntity e)
    {
        e.Address = dto.Address;
        e.Country = dto.Country;
        e.Description = dto.Description;
        e.Price = dto.Price;
        e.Photo = dto.Photo;
    }

    public HouseRepository(HouseDbContext context)
    {
        this.context = context;
    }

    public async Task<List<HouseDto>> GetAll()
    {
        return await context.Houses.Select(e => new HouseDto(e.Id, e.Address, e.Country, e.Price)).ToListAsync();
    }

    //*Get details of the house providing ID as parameter
    public async Task<HouseDetailDto?> Get(int id)
    {
        //*fetch the record using the LINQ extesion method SingleOrDefaultAsync
        var entity = await context.Houses.SingleOrDefaultAsync(h => h.Id == id);

        //*return null if not found 
        if (entity == null)
            return null;

        //*else create a new instance of HouseDetailDTO and return.
        //*this is one of the benefits of using DTO. Returning an entity is not  
        //*good idea. Now it is easy to see the benefits of using DTO because 
        //*we can exactly control what is returned from the API
        return new HouseDetailDto(entity.Id, entity.Address,
                                  entity.Country, entity.Price,
                                  entity.Description, entity.Photo);
    }

    //*Endpoint to add a House
    public async Task<HouseDetailDto> Add(HouseDetailDto dto)
    {
        var entity = new HouseEntity();
        DtoToEntity(dto, entity);
        context.Houses.Add(entity);
        await context.SaveChangesAsync();
        return EntityToDetailDto(entity);
    }

    public async Task<HouseDetailDto> Update(HouseDetailDto dto)
    {
        var entity = await context.Houses.FindAsync(dto.Id);
        if (entity == null)
            throw new ArgumentException($"Trying to update house: entity with ID {dto.Id} not found.");
        DtoToEntity(dto, entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return EntityToDetailDto(entity);
    }

    public async Task Delete(int id)
    {
        var entity = await context.Houses.FindAsync(id);
        if (entity == null)
            throw new ArgumentException($"Trying to delete house: entity with ID {id} not found.");
        context.Houses.Remove(entity);
        await context.SaveChangesAsync();
    }
}