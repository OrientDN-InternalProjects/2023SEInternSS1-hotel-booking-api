using HotelBooking.Data;
using HotelBooking.Data.Interfaces;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HotelBooking.Test
{
    [TestFixture]
    public class AddressRepositoryTest
    {
        private IEnumerable<Address> addresses;
        Mock<IAddressRepository> mockRepo;

        Mock<BookingDbContext> context;
        private DbContextOptions<BookingDbContext> dbContextOptions;

        [SetUp]
        public void Setup()
        {
            //var options = new DbContextOptionsBuilder<BookingDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            addresses = new List<Address>
            {
                new Address
                {
                Id = new Guid("ce146157-b904-48f8-ad08-7d2202dddca1"),
                City = "Da Nang",
                PinCode = "12345",
                StreetNumber = "54 NLB",
                District = "LC",
                Building = "SB"
                },
                new Address
                {
                Id = new Guid("12a57dcf-1927-48a3-8871-8efc50e1e4e9"),
                City = "HCM city",
                PinCode = "56789",
                StreetNumber = "138 HVT",
                District = "LC",
                Building = "SB"
                },
            };
            var dbName = $"BookDb_{DateTime.Now.ToFileTimeUtc()}";
            dbContextOptions = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

            context = new Mock<BookingDbContext>();


            mockRepo = new Mock<IAddressRepository>();
            mockRepo.Setup(mr => mr.GetAllAsync()).Returns(Task.FromResult(addresses));
            mockRepo.Setup(mr => mr.GetByIdAsync(
                It.IsAny<Guid>())).ReturnsAsync(((Guid id) => addresses.FirstOrDefault(x => x.Id == id)));

            mockRepo.Setup(mr => mr.CreateAsync(It.IsAny<Address>())).Callback<Address>((Address x) => addresses.ToList().Add(x));
            //mockRepo.Setup(mr => mr.DeleteTest(It.IsAny<Address>())).Returns((Address x) => addresses.ToList().Remove(x));
            mockRepo.Setup(mr => mr.Delete(It.IsAny<Guid>())).Callback<Guid>((Guid id) => addresses.ToList().RemoveAll(x => x.Id == id));
            //mockRepo.Setup(mr => mr.Add(It.IsAny<Address>())).a((Address x) => addresses.ToList().Add(x));

        }
        [Test]
        public async Task ReturnAllAddreses()
        {
            var hello = mockRepo.Object;
            var res = await Task.FromResult(hello.GetAllAsync());
            Assert.IsNotNull(res); // Test if null
            Assert.That(2, Is.EqualTo(res.Result.Count())); // Verify the correct Number
        }
        [Test]
        public async Task GetAddressById()
        {
            var hello = mockRepo.Object;
            var res = await Task.FromResult(hello.GetByIdAsync(new Guid("12a57dcf-1927-48a3-8871-8efc50e1e4e9")));
            Assert.IsNotNull(res); // Test if null
            Assert.That("HCM city", Is.EqualTo(res.Result.City)); // Verify the correct Number
        }
        [Test]
        public async Task AddNewAddress()
        {
            var hello = mockRepo.Object;
            var new_address = new Address
            {

                Id = new Guid("f51e5e1e-8fc1-4322-83f7-3c11229a9081"),
                City = "HN Capital",
                PinCode = "123197",
                StreetNumber = "138 NT",
                District = "HK",
                Building = "SB",
                CreatedDate = DateTime.Now,
            };
            hello.CreateAsync(new_address);
            var res = await Task.FromResult(hello.GetByIdAsync(new Guid("f51e5e1e-8fc1-4322-83f7-3c11229a9081")));
            Assert.IsNotNull(res);
            //var res = await Task.FromResult(hello.GetAllAsync());
            //Assert.IsNotNull(res); // Test if null
            //Assert.That(3, Is.EqualTo(res.Result.Count()));
            //    AddressRepository movieRepository = new AddressRepository(context);
            //    movieRepository.CreateAsync(new_address);
            //    context.SaveChanges();
            //    Assert.That(1, Is.EqualTo(movieRepository.GetAll().Count()));

        }
        [Test]
        public async Task DeleteAddress()
        {
            var hello = mockRepo.Object;
            var address = new Address
            {
                Id = new Guid("12a57dcf-1927-48a3-8871-8efc50e1e4e9"),
                City = "HCM city",
                PinCode = "56789",
                StreetNumber = "138 HVT",
                District = "LC",
                Building = "SB"
            };
            hello.Delete(new Guid("12a57dcf-1927-48a3-8871-8efc50e1e4e9"));
            var res = await Task.FromResult(hello.GetByIdAsync(new Guid("12a57dcf-1927-48B3-8871-8efc50e1e4e9")));

            Assert.IsNull(res.Result); // Test if null
            //Assert.That(0, Is.EqualTo(res.Result.Id));
        }
        //var res = await Task.FromResult(hello.GetAllAsync());
        //Assert.IsNotNull(res); // Test if null
        //Assert.That(3, Is.EqualTo(res.Result.Count()));
        //    AddressRepository movieRepository = new AddressRepository(context);
        //    movieRepository.CreateAsync(new_address);
        //    context.SaveChanges();
        //    Assert.That(1, Is.EqualTo(movieRepository.GetAll().Count()));


        //    [Test]
        //public void AddNewAddress()
        //{
        //var new_address = new Address
        //{

        //    Id = new Guid("f51e5e1e-8fc1-4322-83f7-3c11229a9081"),
        //    City = "HN Capital",
        //    PinCode = "123197",
        //    StreetNumber = "138 NT",
        //    District = "HK",
        //    Building = "SB"

        //};
        ////var contexthele = new BookingDbContext(dbContextOptions);
        ////contexthele.Set<Address>();
        //var ge = new Mock<IGenericRepository<Address>>(context.Object);

        //var items = ge.GetAll();
        //Assert.That(items.Count, Is.EqualTo(1));
    }
}

//}
//[Test]
//public async Task AddANewAddress()
//{
//    var dbName = $"BookDb_{DateTime.Now.ToFileTimeUtc()}";
//    dbContextOptions = new DbContextOptionsBuilder<BookingDbContext>()
//    .UseInMemoryDatabase(dbName)
//    .Options;
//    using (var context = new BookingDbContext(dbContextOptions))
//    {
//        //context.Database.EnsureCreated();
//        context.Addresses.Add(new Address
//        {
//            Id = new Guid("ce146157-b904-48f8-ad08-7d2202dddca1"),
//            City = "Da Nang",
//            PinCode = "12345",
//            StreetNumber = "54 NLB",
//            District = "LC",
//            Building = "SB"
//        });
//        context.Addresses.Add(new Address
//        {
//            Id = new Guid("12a57dcf-1927-48a3-8871-8efc50e1e4e9"),
//            City = "HCM city",
//            PinCode = "56789",
//            StreetNumber = "138 HVT",
//            District = "LC",
//            Building = "SB"
//        });

//        context.SaveChanges();
//    }

//    // Use a clean instance of the context to run the test
//    using (var context = new BookingDbContext(dbContextOptions))
//    {
//        //context.Database.EnsureCreated();
//        var new_address = new Address
//        {

//            Id = new Guid("f51e5e1e-8fc1-4322-83f7-3c11229a9081"),
//            City = "HN Capital",
//            PinCode = "123197",
//            StreetNumber = "138 NT",
//            District = "HK",
//            Building = "SB"

//        };
//        AddressRepository movieRepository = new AddressRepository(context);
//        movieRepository.CreateAsync(new_address);
//        var movies = await movieRepository.GetAllAsync();
//        Assert.That(2, Is.EqualTo(movies.Count()));
//    }
//}
//[Test]
//public async Task AddANewAddress()
//{
//    var dbName = $"BookDb_{DateTime.Now.ToFileTimeUtc()}";
//    dbContextOptions = new DbContextOptionsBuilder<UserContext>()
//    .UseInMemoryDatabase(dbName)
//    .Options;
//    var data = addresses.AsQueryable();
//    var mockSet = new Mock<DbSet<Address>>();
//    //mockSet.As<IDbAsyncEnumerable<Address>>()
//    //    .Setup(m => m.GetAsyncEnumerator())
//    //    .Returns(new TestDbAsyncEnumerator<Blog>(data.GetEnumerator()));

//    //mockSet.As<IQueryable<Blog>>()
//    //    .Setup(m => m.Provider)
//    //    .Returns(new TestDbAsyncQueryProvider<Blog>(data.Provider));

//    mockSet.As<IQueryable<Address>>().Setup(m => m.Expression).Returns(data.Expression);
//    mockSet.As<IQueryable<Address>>().Setup(m => m.ElementType).Returns(data.ElementType);
//    mockSet.As<IQueryable<Address>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

//    ;
//    context.Setup(c => c.Addresses).Returns(mockSet.Object);

//    var unit = new UnitOfWork(context.Object);
//    var new_address = new Address
//    {

//        Id = new Guid("f51e5e1e-8fc1-4322-83f7-3c11229a9081"),
//        City = "HN Capital",
//        PinCode = "123197",
//        StreetNumber = "138 NT",
//        District = "HK",
//        Building = "SB"

//    };
//    var hello = mockRepo.Object;
//    hello.CreateAsync(new_address);
//    //context.Object.SaveChanges();
//    await unit.SaveAsync();

//    var res = await Task.FromResult(hello.GetAllAsync());
//    Assert.IsNotNull(res); // Test if null
//    Assert.That(3, Is.EqualTo(res.Result.Count()));
//}

//}
//}
