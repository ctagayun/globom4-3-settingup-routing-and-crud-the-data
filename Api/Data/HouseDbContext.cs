using Microsoft.EntityFrameworkCore;
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "<Pending>")]

//A DbContext instance represents a session with the database and can 
//be used to query and save instances of your entities. 
//DbContext is a combination of the Unit Of Work and Repository patterns.
//https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
public class HouseDbContext: DbContext
 {
     //This is the special constructor of the HouseDbContext. All you 
     //have to do is pass it on to base constructor
     public HouseDbContext(DbContextOptions<HouseDbContext> o) : 
        base(o) {  
     }

    //Create a property called "Houses" of type DbSet
    //The DBSet collection represents the database table itself.
    //We set the initial value of Houses to an empty DbSet using the Set method.
    public DbSet<HouseEntity> Houses => Set<HouseEntity> ();
    
    //Create a property called "Bids"  
    //public DbSet<BidEntity> Bids => Set<BidEntity>();

    //Now we have to configure which database to use by overriding OnConfiguring()
    //of the HouseDbContext
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //first two lines we are deermining a good place for the database file
        //in the special directory where local application data is stored.
        //This path differs depending on the operating system you are on
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        //here we are going to use optionsBuilder to tell EF we are using
        //Sqlite and we are passing the connectionstring that points to the 
        //path and specify the filename "houses.db". If you ran the 
        //completed you will have a house.db already.. so give it another 
        //name such as "houses2.db" or something
        options 
           .UseSqlite($"Data Source={Path.Join(path, "houses2.db")}");
    }

    //Create SeeData.cs class and copy the houses entity. Do not copy
    //bid entities yet and override OnModelCreating() of the DbContext
    //that takes the model builder. All we have to do is call the static method 
    //on SeeData and pass it in,
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData.Seed(modelBuilder);
    }
 }