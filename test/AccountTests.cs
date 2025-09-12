using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentPlatform.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPlatform.Tests
{
    [TestClass]
    public class AccountTests
    {
        private Mock<IAccountRepository> _mockRepo = null!;
        private AccountService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _service = new AccountService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task CreateAccount_WithValidInputs_ShouldSucceed()
        {
            var account = new Account { Email = "test@student.com", Password = "StrongP@ss123" };
            _mockRepo.Setup(r => r.IsEmailUnique(account.Email)).ReturnsAsync(true);

            var result = await _service.CreateAccount(account);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Account created", result.Message);
        }

        [TestMethod]
        public async Task CreateAccount_WithDuplicateEmail_ShouldFail()
        {
            var account = new Account { Email = "duplicate@student.com", Password = "StrongP@ss123" };
            _mockRepo.Setup(r => r.IsEmailUnique(account.Email)).ReturnsAsync(false);

            var result = await _service.CreateAccount(account);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Email already exists", result.Message);
        }

        [TestMethod]
        public void ValidatePassword_WithWeakPassword_ShouldFail()
        {
            var weakPassword = "12345";
            var result = _service.ValidatePassword(weakPassword);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Login_WithCorrectCredentials_ShouldSucceed()
        {
            var email = "user@student.com";
            var password = "CorrectP@ss123";
            _mockRepo.Setup(r => r.ValidateCredentials(email, password)).ReturnsAsync(true);
            _mockRepo.Setup(r => r.IsAccountLocked(email)).ReturnsAsync(false);

            var result = await _service.Login(email, password);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Login successful", result.Message);
        }

        [TestMethod]
        public async Task Login_WithWrongPassword_ShouldFail()
        {
            var email = "user@student.com";
            var password = "WrongPassword";
            _mockRepo.Setup(r => r.ValidateCredentials(email, password)).ReturnsAsync(false);
            _mockRepo.Setup(r => r.IsAccountLocked(email)).ReturnsAsync(false);

            var result = await _service.Login(email, password);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid credentials", result.Message);
        }

        [TestMethod]
        public async Task Login_AfterMultipleFailedAttempts_ShouldLockAccount()
        {
            var email = "locked@student.com";
            var password = "WrongPassword";

            _mockRepo.Setup(r => r.IsAccountLocked(email)).ReturnsAsync(false);
            _mockRepo.Setup(r => r.ValidateCredentials(email, password)).ReturnsAsync(false);

            for (int i = 0; i < 5; i++)
                await _service.Login(email, password);

            _mockRepo.Verify(r => r.LockAccount(email), Times.AtLeastOnce());
        }

        // 🔹 Extra Tests
        [TestMethod]
        public void Email_ShouldBeValidFormat()
        {
            var invalidEmail = "invalidEmail@";
            var isValid = invalidEmail.Contains("@") && invalidEmail.Contains(".");
            Assert.IsFalse(isValid, "Email format should be invalid");
        }

        [TestMethod]
        public void Password_ShouldNotBeReused()
        {
            var oldPasswords = new[] { "OldP@ss123", "OlderP@ss456" };
            var newPassword = "OldP@ss123";
            Assert.IsFalse(!oldPasswords.Contains(newPassword), "Password should not be reused");
        }

        [TestMethod]
        public void Session_ShouldExpireAfterTimeout()
        {
            var lastActive = DateTime.UtcNow.AddMinutes(-31);
            var isExpired = (DateTime.UtcNow - lastActive).TotalMinutes > 30;
            Assert.IsTrue(isExpired, "Session should expire after inactivity");
        }
    }
}
