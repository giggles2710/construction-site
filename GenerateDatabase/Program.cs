﻿using GenerateDatabase.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

            // generate parent category
            Console.WriteLine("==== GENERATING PARENT CATEGORY ====");
            generator.GenerateParentCategory();
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
            Account account = new Account();

            XmlTextReader reader = new XmlTextReader("Xml/user.xml");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        switch (reader.Name)
                        {
                            case "user":
                                account = new Account();
                                account.AccountID = Guid.NewGuid().ToString();
                                account.HashToken = UtilityHelper.RandomString(10);
                                account.IsActive = true;
                                break;
                            case "username":
                                reader.Read();
                                account.Username = reader.Value;
                                break;
                            case "password":
                                reader.Read();
                                account.Password = EncryptionHelper.HashPassword(reader.Value, account.HashToken);
                                break;
                            case "firstName":
                                reader.Read();
                                account.FirstName = reader.Value;
                                break;
                            case "lastName":
                                reader.Read();
                                account.LastName = reader.Value;
                                break;
                            case "phoneNumber":
                                reader.Read();
                                account.PhoneNumber = reader.Value;
                                break;
                            case "emailAddress":
                                reader.Read();
                                account.EmailAddress = reader.Value;
                                break;
                            case "Note":
                                reader.Read();
                                account.Note = reader.Value;
                                break;
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        switch (reader.Name)
                        {
                            case "user":
                                var isDuplicate = dbContext.Accounts.Any(a => a.EmailAddress.Equals(account.EmailAddress, StringComparison.InvariantCultureIgnoreCase));

                                if (!isDuplicate)
                                    dbContext.Accounts.Add(account);
                                break;
                        }
                        break;
                }
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

        public void GenerateParentCategory()
        {
            Category category = new Category();

            XmlTextReader reader = new XmlTextReader("D:/Working Place/Dot NET/Construction Site/construction-site/GenerateDatabase/Xml/parent-category.xml");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        switch (reader.Name)
                        {
                            case "category":
                                category = new Category();
                                break;
                            case "name":
                                reader.Read();
                                category.Name = reader.Value;
                                break;
                            case "description":
                                reader.Read();
                                category.Description = reader.Value;
                                break;
                            case "isActive":
                                reader.Read();
                                category.IsActive = true;
                                break;
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        switch (reader.Name)
                        {
                            case "category":
                                dbContext.Categories.Add(category);
                                break;
                        }
                        break;
                }
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

        public void GenerateCategory()
        {
            Category category = new Category();

            XmlTextReader reader = new XmlTextReader("D:/Working Place/Dot NET/Construction Site/construction-site/GenerateDatabase/Xml/category.xml");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        switch (reader.Name)
                        {
                            case "category":
                                category = new Category();
                                break;
                            case "name":
                                reader.Read();
                                category.Name = reader.Value;
                                break;
                            case "parentCategoryName":
                                reader.Read();
                                var parentCategory = dbContext.Categories.FirstOrDefault(c => c.Name.Equals(reader.Value, StringComparison.InvariantCultureIgnoreCase));
                                if (parentCategory != null)
                                    category.ParentID = parentCategory.CategoryID;
                                break;
                            case "description":
                                reader.Read();
                                category.Description = reader.Value;
                                break;
                            case "isActive":
                                reader.Read();
                                category.IsActive = true;
                                break;
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        switch (reader.Name)
                        {
                            case "category":
                                dbContext.Categories.Add(category);
                                break;
                        }
                        break;
                }
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
            Product product = new Product();
            ProductDetail detail = new ProductDetail();

            XmlTextReader reader = new XmlTextReader("D:/Working Place/Dot NET/Construction Site/construction-site/GenerateDatabase/Xml/product.xml");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        switch (reader.Name)
                        {
                            case "product":
                                product = new Product();
                                break;
                            case "specification":
                                detail = new ProductDetail();
                                break;
                            case "name":
                                reader.Read();
                                product.Name = reader.Value;
                                break;
                            case "categoryName":
                                reader.Read();
                                var category = dbContext.Categories.FirstOrDefault(c => c.Name.Equals(reader.Value, StringComparison.InvariantCultureIgnoreCase));
                                if (category != null)
                                    product.CategoryID = category.CategoryID;
                                break;
                            case "description":
                                reader.Read();
                                product.Description = reader.Value;
                                break;
                            case "discount":
                                reader.Read();
                                if (!string.IsNullOrWhiteSpace(reader.Value))
                                    product.Discount = double.Parse(reader.Value);
                                break;
                            case "isAvailable":
                                reader.Read();
                                product.IsAvailable = bool.Parse(reader.Value);
                                break;
                            case "unitPrice":
                                reader.Read();
                                product.UnitPrice = decimal.Parse(reader.Value);
                                break;
                            case "unitsInStock":
                                reader.Read();
                                product.UnitsInStock = int.Parse(reader.Value);
                                break;
                            case "summary":
                                reader.Read();
                                product.Summary = reader.Value;
                                break;
                            case "detailName":
                                reader.Read();
                                detail.Name = reader.Value;
                                break;
                            case "detailValue":
                                reader.Read();
                                detail.Value = reader.Value;
                                break;
                            case "detailType":
                                reader.Read();
                                detail.Type = int.Parse(reader.Value);
                                break;
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        switch (reader.Name)
                        {
                            case "product":
                                var random = new Random();

                                product.DateModified = DateTime.Now;
                                product.Rate = random.Next(0, 5);
                                product.ViewCount = random.Next(0, 1000000);
                                product.OrderCount = random.Next(0, 1000000);

                                dbContext.Products.Add(product);
                                break;
                            case "specification":
                                product.ProductDetails.Add(detail);
                                break;
                        }
                        break;
                }
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
