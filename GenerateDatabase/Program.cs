using GenerateDatabase.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace GenerateDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new Generator();

            // generate account
            Console.WriteLine("==== GENERATING USER ====");
            //generator.GenerateUser();
            Console.WriteLine("==== DONE ====");

            // generate category
            Console.WriteLine("==== GENERATING CATEGORY ====");
            //generator.GenerateCategory();
            Console.WriteLine("==== DONE ====");

            // generate supplier
            Console.WriteLine("==== GENERATING SUPPLIER ====");
            //generator.GenerateSupplier();
            Console.WriteLine("==== DONE ====");

            // generate product
            Console.WriteLine("==== GENERATING PRODUCT ====");
            generator.GenerateProduct();
            Console.WriteLine("==== DONE ====");

            Console.WriteLine("==== GENERATING ALL DONE ====");
            Console.ReadLine();
        }
    }

    public class Generator
    {
        private ConstructionSiteEntities dbContext;
        private Random random;

        public Generator()
        {
            dbContext = new ConstructionSiteEntities();
            random = new Random();
        }

        public void GenerateUser()
        {
            for (var i = 0; i < 300; i++)
            {
                var account = new Account
                {
                    EmailAddress = string.Format("User_{0}.TEST@gmail.com", i),
                    FirstName = "User",
                    LastName = i.ToString(),
                    ModifiedDate = DateTime.Now,
                    IsActive = random.Next() % 2 != 0,
                    HashToken = UtilityHelper.RandomString(10),
                    Note = UtilityHelper.RandomString(100),
                    PhoneNumber = UtilityHelper.RandomTelephoneNumber(),
                    Username = string.Format("User{0}", i)
                };

                account.Password = EncryptionHelper.HashPassword("yuiop67890", account.HashToken);

                dbContext.Accounts.Add(account);
            }

            dbContext.SaveChanges();
        }

        public void GenerateCategory()
        {
            for (var i = 0; i < 300; i++)
            {
                var category = new Category
                {
                    CreatedUserID = random.Next(1, 300),
                    DateModified = DateTime.Now,
                    Description = UtilityHelper.RandomString(150),
                    IsActive = random.Next() % 2 != 0,
                    Name = UtilityHelper.RandomString(50)
                };

                dbContext.Categories.Add(category);
            }

            dbContext.SaveChanges();
        }

        public void GenerateSupplier()
        {
            for (var i = 0; i < 300; i++)
            {
                var supplier = new Supplier
                {
                    Address1 = UtilityHelper.RandomString(50),
                    Address2 = random.Next() % 2 != 0 ? UtilityHelper.RandomString(50) : string.Empty,
                    City = "Da Nang",
                    CompanyName = string.Format("Supplier {0}", UtilityHelper.RandomString(10)),
                    ContactFName = UtilityHelper.RandomString(10),
                    ContactLName = UtilityHelper.RandomString(10),
                    Discount = random.Next() % 2 != 0 ? random.NextDouble() : 0,
                    EmailAddress = string.Format("Supplier_{0}.TEST@gmail.com", i),
                    Fax = random.Next() % 2 != 0 ? UtilityHelper.RandomTelephoneNumber() : null,
                    Phone = UtilityHelper.RandomTelephoneNumber(),
                    ProductType = UtilityHelper.RandomString(10)
                };

                dbContext.Suppliers.Add(supplier);
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GenerateProduct()
        {
            for (var i = 0; i < 300; i++)
            {
                var product = new Product
                {
                    CategoryID = random.Next(1, 300),
                    CreatedUserID = random.Next(1,300),
                    DateModified = DateTime.Now,
                    Description = random.Next() % 2 != 0 ? UtilityHelper.RandomString(50) : string.Empty,
                    Discount = random.Next() % 2 != 0 ? random.NextDouble() : 0,
                    IsAvailable = random.Next() % 2 != 0,
                    Name = UtilityHelper.RandomString(20),
                    Rating = 0,
                    Size = string.Format("{0} X {1}", random.Next(1,100), random.Next(1, 100)),
                    SupplierID = random.Next(1,300),
                    UnitName = "vien",
                    UnitPrice = (decimal)random.NextDouble(),
                    UnitsInStock = random.Next(1, 100),
                    UnitWeight = random.NextDouble()
                };

                product.IsDiscountAvailable = product.Discount.GetValueOrDefault() > 0;

                dbContext.Products.Add(product);
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
