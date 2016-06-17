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
            generator.GenerateUser();
            Console.WriteLine("==== DONE ====");

            // generate category
            Console.WriteLine("==== GENERATING CATEGORY ====");
            generator.GenerateCategory();
            Console.WriteLine("==== DONE ====");

            // generate supplier
            Console.WriteLine("==== GENERATING SUPPLIER ====");
            generator.GenerateSupplier();
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
                    AccountID = Guid.NewGuid().ToString(),
                    EmailAddress = string.Format("User_{0}.TEST@gmail.com", i).ToLower(),
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
            var parentCategory = new string[] { "Gạch và ngói", "Thiết bị vệ sinh", "Điện", "Cửa nhựa lõi thép" };
            var gachVaNgoiCate = new string[] { "Gạch block", "Gạch cổ và Ngói cổ", "Gạch ngoại thất", "Gạch nhẹ", "Gạch nung", "Ngói màu", "Ngói nung", "Ngói tráng men", "Gạch ốp lát", "TOLE và Tấm lợp" };
            var thietBiVeSinh = new string[] { "Bồn cầu", "Bồn rửa mặt", "Máy sấy khăn", "Nội thất phòng tắm", "Lavabo", "Bồn tiểu", "Vòi nước", "Bồn tắm", "Phụ kiện khác" };
            var dien = new string[] { "Đèn chiếu sáng", "Dây và Cáp điện", "Thiết bị điện", "Thiết Bị Cơ Điện-Lạnh" };

            // generate category
            // parents
            #region Parent Category

            foreach (var parent in parentCategory)
            {
                dbContext.Categories.Add(new Category
                {
                    DateModified = DateTime.Now,
                    Description = random.Next() % 2 != 0 ? UtilityHelper.RandomString(100) : string.Empty,
                    IsActive = true,
                    Name = parent
                });
            }

            dbContext.SaveChanges();

            #endregion

            #region Gach va Ngoi
            var gachVaNgoiParent = dbContext.Categories.FirstOrDefault(c => c.Name.Equals("Gạch và ngói", StringComparison.OrdinalIgnoreCase));

            if (gachVaNgoiParent != null)
            {
                foreach (var item in gachVaNgoiCate)
                {
                    dbContext.Categories.Add(new Category
                    {
                        DateModified = DateTime.Now,
                        Description = random.Next() % 2 != 0 ? UtilityHelper.RandomString(100) : string.Empty,
                        IsActive = true,
                        Name = item,
                        ParentID = gachVaNgoiParent.CategoryID
                    });
                }
            }

            var gachBlock = dbContext.Categories.FirstOrDefault(c => c.Name.Equals("Gạch block", StringComparison.OrdinalIgnoreCase));

            if (gachBlock != null)
            {
                for (var i = 0; i < 5; i++)
                {
                    dbContext.Categories.Add(new Category
                    {
                        DateModified = DateTime.Now,
                        Description = random.Next() % 2 != 0 ? UtilityHelper.RandomString(100) : string.Empty,
                        IsActive = true,
                        Name = string.Format("Gạch block {0}",i),
                        ParentID = gachBlock.CategoryID
                    });
                }
            }
            #endregion

            #region Thiet bi ve sinh

            var thietbivesinhCate = dbContext.Categories.FirstOrDefault(c => c.Name.Equals("Thiết bị vệ sinh", StringComparison.OrdinalIgnoreCase));

            if (thietbivesinhCate != null)
            {
                foreach (var item in thietBiVeSinh)
                {
                    dbContext.Categories.Add(new Category
                    {
                        DateModified = DateTime.Now,
                        Description = random.Next() % 2 != 0 ? UtilityHelper.RandomString(100) : string.Empty,
                        IsActive = true,
                        Name = item,
                        ParentID = thietbivesinhCate.CategoryID
                    });
                }
            }
            #endregion

            #region Dien

            var dienCategory = dbContext.Categories.FirstOrDefault(c => c.Name.Equals("Điện", StringComparison.OrdinalIgnoreCase));

            if (dienCategory != null)
            {
                foreach (var item in dien)
                {

                    dbContext.Categories.Add(new Category
                    {
                        DateModified = DateTime.Now,
                        Description = random.Next() % 2 != 0 ? UtilityHelper.RandomString(100) : string.Empty,
                        IsActive = true,
                        Name = item,
                        ParentID = dienCategory.CategoryID
                    });
                }
            }

            #endregion

            dbContext.SaveChanges();
        }

        public void GenerateSupplier()
        {
            for (var i = 0; i < 300; i++)
            {
                var supplier = dbContext.Suppliers.FirstOrDefault(s => s.SupplierID == i);

                if (supplier == null)
                    supplier = new Supplier();

                supplier.Address1 = UtilityHelper.RandomString(50);
                supplier.Address2 = random.Next() % 2 != 0 ? UtilityHelper.RandomString(50) : string.Empty;
                supplier.City = "Da Nang";
                supplier.CompanyName = string.Format("Supplier {0}", UtilityHelper.RandomString(10));
                supplier.Discount = random.Next() % 2 != 0 ? random.NextDouble() : 0;
                supplier.EmailAddress = string.Format("Supplier_{0}.TEST@gmail.com", i);
                supplier.Fax = random.Next() % 2 != 0 ? UtilityHelper.RandomTelephoneNumber() : null;
                supplier.Phone = UtilityHelper.RandomTelephoneNumber();
                supplier.ProductType = UtilityHelper.RandomString(20);

                if (random.Next() % 2 != 0)
                {
                    supplier.ContactFName = UtilityHelper.RandomString(15);
                    supplier.ContactLName = UtilityHelper.RandomString(15);
                }
                else
                {
                    supplier.ContactFName = string.Empty;
                    supplier.ContactLName = string.Empty;
                }

                if (supplier == null)
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
                var product = dbContext.Products.FirstOrDefault(p => p.ProductID == i);

                if (product == null)
                    product = new Product();

                var category = dbContext.Categories.ToArray()[random.Next(1, dbContext.Categories.Count())];

                product.CategoryID = category.CategoryID;
                product.CreatedUserID = string.Empty;
                product.DateModified = DateTime.Now;
                product.Description = random.Next() % 2 != 0 ? UtilityHelper.RandomString(50) : string.Empty;
                product.Discount = random.Next() % 2 != 0 ? random.NextDouble() : 0;
                product.IsAvailable = random.Next() % 2 != 0;
                product.Name = string.Format("{0}_{1}", category.Name, i);
                product.SupplierID = random.Next(1, 300);
                product.UnitPrice = (decimal)random.NextDouble();
                product.UnitsInStock = random.Next(1, 100);
                product.IsDiscountAvailable = product.Discount.GetValueOrDefault() > 0;

                if (product == null)
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
