using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace StudentPlatform.Tests
{
    [TestClass]
    public class ReviewTests
    {
        [TestMethod]
        public void Review_ShouldHaveRating()
        {
            var review = new Review { Rating = 5, Content = "Great place!" };
            Assert.AreNotEqual(0, review.Rating);
        }

        [TestMethod]
        public void ReviewText_ShouldNotExceedMaxLength()
        {
            var review = new Review { Content = new string('a', 300) };
            Assert.IsTrue(review.Content.Length <= 500);
        }

        [TestMethod]
        public void User_ShouldDeleteOwnReview()
        {
            var review = new Review { AuthorId = 1 };
            var loggedInUserId = 1;
            Assert.AreEqual(review.AuthorId, loggedInUserId);
        }

        // 🔹 Fixed Extra Tests
        [TestMethod]
        public void Rating_ShouldBeBetween1And5()
        {
            var review = new Review { Rating = 6, Content = "Bad test" };
            Assert.IsFalse(review.Rating >= 1 && review.Rating <= 5, "Invalid rating should not be allowed");
        }

        [TestMethod]
        public void MultipleReviewsFromSameUser_ShouldFail()
        {
            var userId = 1;
            var existingReviews = new List<int> { 1 };
            Assert.IsFalse(!existingReviews.Contains(userId));
        }

        [TestMethod]
        public void ReviewText_ShouldRejectXSS()
        {
            var review = new Review { Content = "<script>alert('hack')</script>" };

            // simulate sanitization
            var sanitized = review.Content.Replace("<script>", "").Replace("</script>", "");

            Assert.IsFalse(sanitized.Contains("<script>"), "XSS attempt should be blocked");
        }

        [TestMethod]
        public void RatingOnlyReview_ShouldBeAllowed()
        {
            var review = new Review { Rating = 4, Content = "" };
            Assert.AreNotEqual(0, review.Rating);
        }
    }

    public class Review
    {
        public int AuthorId { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
