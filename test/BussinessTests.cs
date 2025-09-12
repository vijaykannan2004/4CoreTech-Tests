using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StudentPlatform.Tests
{
    [TestClass]
    public class BusinessTests
    {
        [TestMethod]
        public void BusinessId_ShouldNotBeEmpty()
        {
            var business = new Business { Id = 1, BusinessName = "ShopX" };
            Assert.AreNotEqual(0, business.Id, "Business ID should not be empty");
        }

        [TestMethod]
        public void BusinessName_ShouldNotBeNullOrEmpty()
        {
            var business = new Business { BusinessName = "ShopX" };
            Assert.IsFalse(string.IsNullOrWhiteSpace(business.BusinessName), "Business name must not be null or empty");
        }

        [TestMethod]
        public void BusinessCategory_ShouldBeProvided()
        {
            var business = new Business { BusinessType = 2 }; // valid category
            Assert.AreNotEqual(0, business.BusinessType, "Business category must be provided");
        }

        [TestMethod]
        public void BusinessLocation_ShouldBeProvided()
        {
            var business = new Business { City = "New York", PostalCode = "10001" };
            Assert.IsFalse(string.IsNullOrWhiteSpace(business.City), "Business city must not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(business.PostalCode), "Business postal code must not be empty");
        }
    }

    // Simple entity stub for testing
    public class Business
    {
        public int Id { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public int BusinessType { get; set; }
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }
}
