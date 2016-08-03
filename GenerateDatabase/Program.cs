using GenerateDatabase.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

            // generate merge fields
            Console.WriteLine("==== GENERATING MERGE FIELD ====");
            generator.GenerateMergeFields();
            Console.WriteLine("==== DONE ====");

            // generate emailTemplate
            Console.WriteLine("==== GENERATING Email Template ====");
            generator.GenerateEmailTemplates();
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

            XmlTextReader reader = new XmlTextReader(@"Xml\user.xml");
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
                                account.HashToken = GeneratorUtilityHelper.RandomString(10);
                                account.IsActive = true;
                                break;
                            case "username":
                                reader.Read();
                                account.Username = reader.Value;
                                break;
                            case "password":
                                reader.Read();
                                account.Password = GeneratorEncryptionHelper.HashPassword(reader.Value, account.HashToken);
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

            XmlTextReader reader = new XmlTextReader(@"Xml\parent-category.xml");
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

            XmlTextReader reader = new XmlTextReader(@"Xml\category.xml");
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

                supplier.Address1 = GeneratorUtilityHelper.RandomString(50);
                supplier.Address2 = random.Next() % 2 != 0 ? GeneratorUtilityHelper.RandomString(50) : string.Empty;
                supplier.City = "Da Nang";
                supplier.CompanyName = string.Format("Supplier {0}", GeneratorUtilityHelper.RandomString(10));
                supplier.Discount = random.Next() % 2 != 0 ? random.NextDouble() : 0;
                supplier.EmailAddress = string.Format("Supplier_{0}.TEST@gmail.com", i);
                supplier.Fax = random.Next() % 2 != 0 ? GeneratorUtilityHelper.RandomTelephoneNumber() : null;
                supplier.Phone = GeneratorUtilityHelper.RandomTelephoneNumber();
                supplier.ProductType = GeneratorUtilityHelper.RandomString(20);

                if (random.Next() % 2 != 0)
                {
                    supplier.ContactFName = GeneratorUtilityHelper.RandomString(15);
                    supplier.ContactLName = GeneratorUtilityHelper.RandomString(15);
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

            XmlTextReader reader = new XmlTextReader(@"Xml\product.xml");
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
                                product.UnitPrice = double.Parse(reader.Value);
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

        public void GenerateMergeFields()
        {
            MergeField field = new MergeField();

            XmlTextReader reader = new XmlTextReader(@"Xml\merge-fields.xml");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        switch (reader.Name)
                        {
                            case "mergeField":
                                field = new MergeField();
                                break;
                            case "name":
                                reader.Read();
                                field.FieldName = reader.Value;
                                break;
                            case "type":
                                reader.Read();
                                field.FieldType = int.Parse(reader.Value);
                                break;                            
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        switch (reader.Name)
                        {
                            case "mergeField":
                                dbContext.MergeFields.Add(field);
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

        public void GenerateEmailTemplates()
        {
            EmailTemplate emailTempOrder = new EmailTemplate();
            emailTempOrder.EmailTemplateName = @"Xác nhận đơn hàng";
            emailTempOrder.EmailSubject = @"Xác nhận đơn hàng trên ConstructionSite.com";
            emailTempOrder.HtmlBody = File.ReadAllText(@"Xml\OrderConfirmationEmailTemp.txt");
            emailTempOrder.EmailType = 1;
            emailTempOrder.CreatedDate = DateTime.Now;
            emailTempOrder.LastUpdatedDate = DateTime.Now;

            dbContext.EmailTemplates.Add(emailTempOrder);

            EmailTemplate emailTempReplyRequest = new EmailTemplate();
            emailTempReplyRequest.EmailTemplateName = @"Phản hồi khách hàng";
            emailTempReplyRequest.EmailSubject = @"Construction Site: Cảm ơn quý khách đã đóng góp ý kiến";
            emailTempReplyRequest.HtmlBody = File.ReadAllText(@"Xml\ReplyRequestEmailTemplate.txt");
            emailTempReplyRequest.EmailType = 2;
            emailTempReplyRequest.CreatedDate = DateTime.Now;
            emailTempReplyRequest.LastUpdatedDate = DateTime.Now;

            dbContext.EmailTemplates.Add(emailTempReplyRequest);

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

    public static class GeneratorEncryptionHelper
    {
        private static string SecretKey = "9TZTi6PdCcG0JurXNjH6ww==";
        private static string IvKey = "TZJbkD968uwJ2ZVEDmyPiw==";

        public static byte[] Encrypt(string value)
        {
            var key = Convert.FromBase64String(SecretKey);
            var iv = Convert.FromBase64String(IvKey);

            byte[] encrypted;
            using (var rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(value);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public static string HashPassword(string password, string saltToken)
        {
            var hmacSHA1 = new HMACSHA1(System.Text.Encoding.UTF8.GetBytes(saltToken));
            var saltedHash = hmacSHA1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(saltedHash);
        }
    }

    public static class GeneratorUtilityHelper
    {
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomTelephoneNumber()
        {
            var rand = new Random();
            StringBuilder telNo = new StringBuilder(12);
            int number;
            for (int i = 0; i < 3; i++)
            {
                number = rand.Next(0, 8); // digit between 0 (incl) and 8 (excl)
                telNo = telNo.Append(number.ToString());
            }
            telNo = telNo.Append("-");
            number = rand.Next(0, 743); // number between 0 (incl) and 743 (excl)
            telNo = telNo.Append(String.Format("{0:D3}", number));
            telNo = telNo.Append("-");
            number = rand.Next(0, 10000); // number between 0 (incl) and 10000 (excl)
            telNo = telNo.Append(String.Format("{0:D4}", number));

            return telNo.ToString();
        }
    }

}
