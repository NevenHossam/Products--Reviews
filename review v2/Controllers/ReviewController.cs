using review_v2.Models;
using review_v2.ViewModel;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace review_v2.Controllers
{
    public class ReviewController : Controller
    {
        private ApplicationDbContext _context;

        public ReviewController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Review/ReviewsOfProduct/2004
        public ActionResult ReviewsOfProduct(int id)
        {
            try
            {
                #region ReviewsOfProduct

                var product = _context.Products.SingleOrDefault(p => p.Id == id);
                var blockedReviewOwners = _context.Users.Where(u => u.LockoutEnabled == false).ToList();
                var listOfReviews = _context.Reviews.Where(r => r.productId == id).ToList();

                List<Review> listOfReviewsWithoutBlockedUsers = new List<Review>();

                foreach (var review in listOfReviews)
                {
                    if (blockedReviewOwners.Exists(x => x.UserName == review.ReviewOwner) == false)
                    {
                        listOfReviewsWithoutBlockedUsers.Add(review);
                    }
                }

                //foreach(var r in listOfReviewsWithoutBlockedUsers)
                //{
                //    _context.Reviews.Remove(r);
                //}
                //_context.SaveChanges();

                decimal calculateTotalRate = 0;
                foreach (var review in listOfReviewsWithoutBlockedUsers)
                {
                    calculateTotalRate += review.Ratio;
                }

                product.TotalPercentageRate = calculateTotalRate;
                _context.SaveChanges();

                var catId = product.ProductCategoryId;
                var catInDb = _context.ProductCategories.SingleOrDefault(x => x.Id == catId);
                var catName = catInDb.Category;

                var latestProductsInDb = _context.Products.Where(x => x.ProductCategoryId == catId).Take(4).ToList();

                List<Review> positiveReviews = new List<Review>();
                List<Review> negativeReviews = new List<Review>();

                foreach (var review in listOfReviewsWithoutBlockedUsers)
                {
                    if (review.Rate >= 0)
                    {
                        positiveReviews.Add(review);
                    }
                    else
                    {
                        negativeReviews.Add(review);
                    }
                }

                var viewModel = new ProductReviewViewModel()
                {
                    ProductName = product.Name,
                    ProductType = product.Type,
                    ProductFeatures = product.Features,
                    ProdcutPrice = product.Price,
                    ProductId = product.Id,
                    ProductReviewsPositiveList = positiveReviews,
                    ProductReviewsNegativeList = negativeReviews,
                    ProductReviewsList = listOfReviewsWithoutBlockedUsers,
                    TotalPercentageRate = calculateTotalRate,
                    Image = product.ProductImageUrl,
                    CategoryName = catName,
                    LatestProductsList = latestProductsInDb
                };
                #endregion
                return View(viewModel);
            }
            catch
            {
                return View();
            }
        }

        // GET: Review/Create
        public ActionResult Create(int id)
        {
            if (User.Identity.Name != null)
            {
                var model = new Review()
                {
                    productId = id
                };
                return View(model);
            }
            else
                return RedirectToAction("Login", "Account");

        }

        [HttpPost]
        public string AddComment(int productId, string comment)
        {
            try
            {
                #region AddReview
                var review = new Review();
                review.productId = productId;
                review.Comment = comment;
                review.ReviewOwner = User.Identity.Name;
                string mycomment = review.Comment;
                string[] commentSplitted = mycomment.Split(null);
                List<int> specialValue = new List<int>();
                List<int> sentimentWordValue = new List<int>();
                List<int> rateValues = new List<int>();
                var value = 0;
                var after = false;
                var before = false;
                var specialWordsInDb = _context.SentimentsSpecialDb.ToList();
                var sentimentWordsInDb = _context.SentimentsDb.ToList();

                for (var i = 0; i < commentSplitted.Count(); i++)
                {
                    var existedSentiment = sentimentWordsInDb.Exists(x => x.Word.Equals(commentSplitted[i]));
                    if (existedSentiment)
                    {
                        if (i != (commentSplitted.Count() - 1) && i == 0) // awl klma
                        {
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            after = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i + 1]);
                            var afterWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i + 1])?.Value;
                            if (after == false)
                            {
                                rateValues.Add(value);
                            }
                            else if (after == true)
                            {
                                if (value > 0)
                                {
                                    value = value + afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = value - afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                            }
                        }
                        else if (i != (commentSplitted.Count() - 1) && i != 0) // msh awl klma w msh l a5era
                        {
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            after = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i + 1]);
                            before = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i - 1]);

                            var afterWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i + 1])?.Value;
                            var beforeWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i - 1])?.Value;

                            // law mfesh la ablha wla b3dha
                            if (after == false && before == false)
                            {
                                rateValues.Add(value);
                            }

                            // law feh b3dha bs
                            else if (after == true && before == false)
                            {
                                if (value > 0)
                                {
                                    value = value + afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = value - afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                            }

                            // law feh ablha bs
                            else if (after == false && before == true)
                            {
                                if (value > 0)
                                {
                                    value = value * beforeWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = (value * beforeWordValue ?? 1) + 2;
                                    rateValues.Add(value);
                                }

                            }

                            // law feh ablha w b3dha
                            else if (before == true && after == true)
                            {
                                var afterWordEx = commentSplitted[i + 1];
                                var specialAfter = _context.SentimentsSpecialDb.FirstOrDefault(x => x.SpecialWord == afterWordEx).IsBefore;
                                var beforeSpecialValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i - 1]).Value;
                                // law 0 yb2a after law 1 yb2a weak law 2 yb2a strong
                                if (specialAfter == 1) // case gdn aw awi
                                {
                                    if (value > 0)
                                    {

                                        value = ((afterWordValue ?? 1) * beforeSpecialValue) + value;
                                        rateValues.Add(value);
                                    }
                                    else
                                    {
                                        value = value + (afterWordValue ?? 1);
                                        rateValues.Add(value);
                                    }
                                }
                                else if (specialAfter == 2) // case 5ales
                                {
                                    if (value > 0)
                                    {
                                        value = (value + afterWordValue ?? 1) * beforeSpecialValue;
                                        rateValues.Add(value);
                                    }
                                    else
                                    {
                                        value = (value * beforeSpecialValue) + afterWordValue ?? 1;
                                        rateValues.Add(value);
                                    }
                                }
                            }
                        }
                        else if (commentSplitted.Count() == 1 && i == 0) // klma lw7dha
                        {
                            value = 0;
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            rateValues.Add(value);
                        }
                        else
                        {
                            value = 0;
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            before = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i - 1]);
                            var wordbefore = commentSplitted[i - 1];
                            var beforeWordValue = _context.SentimentsSpecialDb.SingleOrDefault(x => x.SpecialWord == wordbefore)?.Value;

                            // law mfesh la ablha wla b3dha
                            if (before == false)
                            {
                                rateValues.Add(value);
                            }

                            // law feh ablha bs
                            else if (before == true && after == false)
                            {
                                if (value > 0)
                                {
                                    value = value * beforeWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = (value * beforeWordValue ?? 1) - 2;
                                    rateValues.Add(value);
                                }

                            }
                        }

                    }
                }
                foreach (var item in rateValues)
                {
                    review.Rate += item;
                }

                review.Ratio = Convert.ToDecimal(review.Rate) / Convert.ToDecimal(10);
                review.Date = DateTime.Now;
                review.Time = DateTime.Now;
                review.ReviewOwner = User.Identity.Name;
                bool isValid = ValidateSpam(review);
                if (!isValid)
                {
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
                    currentUser.LockoutEnabled = false;
                    _context.SaveChanges();
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    RedirectPermanent("Lockout");
                }


                _context.Reviews.Add(review);
                var productID = review.productId;
                var productInDb = _context.Products.FirstOrDefault(x => x.Id == productID);
                //productInDb.TotalPercentageRate = productInDb.TotalPercentageRate + review.Ratio;
                _context.SaveChanges();
                #endregion

                return "success";
            }
            catch (Exception ex)
            {
                return "failed";
            }

        }

        public PartialViewResult ReloadReviews(int productId)
        {
            #region ReviewsOfProduct
            var product = _context.Products.SingleOrDefault(p => p.Id == productId);
            var listOfReviews = _context.Reviews.Where(r => r.productId == productId).ToList();

            decimal calculateTotalRate = 0;
            foreach (var review in listOfReviews)
            {
                calculateTotalRate += review.Ratio;
            }

            product.TotalPercentageRate = calculateTotalRate;
            _context.SaveChanges();

            var catId = product.ProductCategoryId;
            var catInDb = _context.ProductCategories.SingleOrDefault(x => x.Id == catId);
            var catName = catInDb.Category;

            var latestProductsInDb = _context.Products.Where(x => x.ProductCategoryId == catId).Take(4).ToList();

            List<Review> positiveReviews = new List<Review>();
            List<Review> negativeReviews = new List<Review>();

            foreach (var review in listOfReviews)
            {
                if (review.Rate >= 0)
                {
                    positiveReviews.Add(review);
                }
                else
                {
                    negativeReviews.Add(review);
                }
            }

            var viewModel = new ProductReviewViewModel()
            {
                ProductName = product.Name,
                ProductType = product.Type,
                ProductFeatures = product.Features,
                ProdcutPrice = product.Price,
                ProductId = product.Id,
                ProductReviewsPositiveList = positiveReviews,
                ProductReviewsNegativeList = negativeReviews,
                ProductReviewsList = listOfReviews,
                Ratio = product.TotalPercentageRate,
                Image = product.ProductImageUrl,
                CategoryName = catName,
                LatestProductsList = latestProductsInDb
            };
            #endregion
            return PartialView("_Reviews", viewModel);
        }
        // POST: Review/Create
        [HttpPost]
        public ActionResult Create(int id, Review review)
        {
            try
            {
                string comment = review.Comment;
                string[] commentSplitted = comment.Split(null);
                List<int> specialValue = new List<int>();
                List<int> sentimentWordValue = new List<int>();
                List<int> rateValues = new List<int>();
                var value = 0;
                var after = false;
                var before = false;
                var specialWordsInDb = _context.SentimentsSpecialDb.ToList();
                var sentimentWordsInDb = _context.SentimentsDb.ToList();

                for (var i = 0; i < commentSplitted.Count(); i++)
                {
                    var existedSentiment = sentimentWordsInDb.Exists(x => x.Word.Equals(commentSplitted[i]));
                    if (existedSentiment)
                    {
                        if (i != (commentSplitted.Count() - 1) && i == 0) // awl klma
                        {
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            after = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i + 1]);
                            var afterWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i + 1])?.Value;
                            if (after == false)
                            {
                                rateValues.Add(value);
                            }
                            else if (after == true)
                            {
                                if (value > 0)
                                {
                                    value = value + afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = value - afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                            }
                        }
                        else if (i != (commentSplitted.Count() - 1) && i != 0) // msh awl klma w msh l a5era
                        {
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            after = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i + 1]);
                            before = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i - 1]);

                            var afterWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i + 1])?.Value;
                            var beforeWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i - 1])?.Value;

                            // law mfesh la ablha wla b3dha
                            if (after == false && before == false)
                            {
                                rateValues.Add(value);
                            }

                            // law feh b3dha bs
                            else if (after == true && before == false)
                            {
                                if (value > 0)
                                {
                                    value = value + afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = value - afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                            }

                            // law feh ablha bs
                            else if (after == false && before == true)
                            {
                                if (value > 0)
                                {
                                    value = value * beforeWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = (value * beforeWordValue ?? 1) + 2;
                                    rateValues.Add(value);
                                }

                            }

                            // law feh ablha w b3dha
                            else if (before == true && after == true)
                            {
                                var afterWordEx = commentSplitted[i + 1];
                                var specialAfter = _context.SentimentsSpecialDb.FirstOrDefault(x => x.SpecialWord == afterWordEx).IsBefore;
                                var beforeSpecialValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i - 1]).Value;
                                // law 0 yb2a after law 1 yb2a weak law 2 yb2a strong
                                if (specialAfter == 1) // case gdn aw awi
                                {
                                    if (value > 0)
                                    {

                                        value = ((afterWordValue ?? 1) * beforeSpecialValue) + value;
                                        rateValues.Add(value);
                                    }
                                    else
                                    {
                                        value = value + (afterWordValue ?? 1);
                                        rateValues.Add(value);
                                    }
                                }
                                else if (specialAfter == 2) // case 5ales
                                {
                                    if (value > 0)
                                    {
                                        value = (value + afterWordValue ?? 1) * beforeSpecialValue;
                                        rateValues.Add(value);
                                    }
                                    else
                                    {
                                        value = (value * beforeSpecialValue) + afterWordValue ?? 1;
                                        rateValues.Add(value);
                                    }
                                }
                            }
                        }
                        else if (commentSplitted.Count() == 1 && i == 0) // klma lw7dha
                        {
                            value = 0;
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            rateValues.Add(value);
                        }
                        else
                        {
                            value = 0;
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            before = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i - 1]);
                            var wordbefore = commentSplitted[i - 1];
                            var beforeWordValue = _context.SentimentsSpecialDb.SingleOrDefault(x => x.SpecialWord == wordbefore)?.Value;

                            // law mfesh la ablha wla b3dha
                            if (before == false)
                            {
                                rateValues.Add(value);
                            }

                            // law feh ablha bs
                            else if (before == true && after == false)
                            {
                                if (value > 0)
                                {
                                    value = value * beforeWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = (value * beforeWordValue ?? 1) - 2;
                                    rateValues.Add(value);
                                }

                            }
                        }

                    }
                }
                foreach (var item in rateValues)
                {
                    review.Rate += item;
                }

                review.Ratio = Convert.ToDecimal(review.Rate) / Convert.ToDecimal(10);
                review.Date = DateTime.Now;
                review.Time = DateTime.Now;
                review.ReviewOwner = User.Identity.Name;
                review.Id = id;
                bool isValid = ValidateSpam(review);
                if (!isValid)
                {
                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
                    currentUser.LockoutEnabled = false;
                    _context.SaveChanges();
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return View("Lockout");
                }

                _context.Reviews.Add(review);

                //var productID = review.productId;
                //var productInDb = _context.Products.FirstOrDefault(x => x.Id == productID);
                //productInDb.TotalPercentageRate = productInDb.TotalPercentageRate + review.Ratio;
                //_context.SaveChanges();

                return RedirectToAction("ReviewsOfProduct", "Review", new { id = id });
            }
            catch (Exception ex)
            {
                return Content("Failed");
            }
        }

        private bool ValidateSpam(Review CurrentReview)
        {
            var currentTime = DateTime.Now.AddMinutes(-3);
            var LatestComments = _context.Reviews.Where(x =>
                                x.ReviewOwner == CurrentReview.ReviewOwner &&
                                x.Time >= currentTime).ToList();

            var SameProductReview = _context.Reviews.Where(x => x.productId == CurrentReview.productId
                                                    && x.ReviewOwner == CurrentReview.ReviewOwner).ToList();

            var repeatedComments = SameProductReview.Where(x => x.Comment == CurrentReview.Comment).ToList();


            if (LatestComments.Count >= 5 || SameProductReview.Count >= 5 || repeatedComments.Count >= 5)
            {
                return false;
            }

            return true;

        }

        // GET: Review/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    var reviewInDb = _context.Reviews.SingleOrDefault(r => r.Id == id);
                    var userName = User.Identity.Name;
                    if (userName != null && reviewInDb.Id == id)
                    {
                        if (reviewInDb.ReviewOwner == userName)
                            return View("Edit", reviewInDb);
                        else
                            return View();
                    }
                    else
                        return View();
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }

        }

        // POST: Review/Edit/5
        [HttpPost]
        public ActionResult Edit(int ID, Review review)
        {
            try
            {
                var reviewInDb = _context.Reviews.SingleOrDefault(r => r.Id == ID);

                reviewInDb.Comment = review.Comment;
                reviewInDb.Rate = 0;
                string comment = review.Comment;
                string[] commentSplitted = comment.Split(null);
                List<int> specialValue = new List<int>();
                List<int> sentimentWordValue = new List<int>();
                List<int> rateValues = new List<int>();
                var value = 0;
                var after = false;
                var before = false;
                var specialWordsInDb = _context.SentimentsSpecialDb.ToList();
                var sentimentWordsInDb = _context.SentimentsDb.ToList();

                for (var i = 0; i <= commentSplitted.Count() - 1; i++)
                {
                    var existedSentiment = sentimentWordsInDb.Exists(x => x.Word.Equals(commentSplitted[i]));
                    if (existedSentiment)
                    {
                        if (i != (commentSplitted.Count() - 1) && i == 0) // awl klma
                        {
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            after = specialWordsInDb.Exists(x => x.SpecialWord.Equals(commentSplitted[i + 1]));
                            var afterWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i + 1])?.Value;
                            if (after == false)
                            {
                                rateValues.Add(value);
                            }
                            else if (after == true)
                            {
                                if (value > 0)
                                {
                                    value = value + afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = value - afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                i++;
                            }
                        }
                        else if (i != (commentSplitted.Count() - 1) && i != 0) // msh awl klma w msh l a5era
                        {
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            after = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i + 1]);
                            before = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i - 1]);

                            var afterWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i + 1])?.Value;
                            var beforeWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i - 1])?.Value;

                            // law mfesh la ablha wla b3dha
                            if (after == false && before == false)
                            {
                                rateValues.Add(value);
                            }

                            // law feh b3dha bs
                            else if (after == true && before == false)
                            {
                                if (value > 0)
                                {
                                    value = value + afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = value - afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                i++;
                            }

                            // law feh ablha bs
                            else if (after == false && before == true)
                            {
                                if (value > 0)
                                {
                                    value = value * beforeWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = (value * beforeWordValue ?? 1) + 2;
                                    rateValues.Add(value);
                                }

                            }

                            // law feh ablha w b3dha
                            else if (before == true && after == true)
                            {
                                var afterWordEx = commentSplitted[i + 1];
                                var specialAfter = _context.SentimentsSpecialDb.FirstOrDefault(x => x.SpecialWord == afterWordEx).IsBefore;
                                var beforeSpecialValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i - 1]).Value;
                                // law 0 yb2a after law 1 yb2a weak law 2 yb2a strong
                                if (specialAfter == 1) // case gdn aw awi
                                {
                                    if (value > 0)
                                    {

                                        value = ((afterWordValue ?? 1) * beforeSpecialValue) + value;
                                        rateValues.Add(value);
                                    }
                                    else
                                    {
                                        value = value + (afterWordValue ?? 1);
                                        rateValues.Add(value);
                                    }
                                }
                                else if (specialAfter == 2) // case 5ales
                                {
                                    if (value > 0)
                                    {
                                        value = (value + afterWordValue ?? 1) * beforeSpecialValue;
                                        rateValues.Add(value);
                                    }
                                    else
                                    {
                                        value = (value * beforeSpecialValue) + afterWordValue ?? 1;
                                        rateValues.Add(value);
                                    }
                                }
                                i++;
                            }
                        }
                        else if (commentSplitted.Count() == 1 && i == 0) // klma lw7dha
                        {
                            value = 0;
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            rateValues.Add(value);
                        }
                        else
                        {
                            value = 0;
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            before = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i - 1]);
                            var wordbefore = commentSplitted[i - 1];
                            var beforeWordValue = _context.SentimentsSpecialDb.SingleOrDefault(x => x.SpecialWord == wordbefore)?.Value;

                            // law mfesh la ablha wla b3dha
                            if (before == false)
                            {
                                rateValues.Add(value);
                            }

                            // law feh ablha bs
                            else if (before == true && after == false)
                            {
                                if (value > 0)
                                {
                                    value = value * beforeWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = (value * beforeWordValue ?? 1) - 2;
                                    rateValues.Add(value);
                                }

                            }
                        }

                    }
                }
                foreach (var item in rateValues)
                {
                    review.Rate += item;
                }

                reviewInDb.Rate = review.Rate;
                reviewInDb.Ratio = Convert.ToDecimal(reviewInDb.Rate) / Convert.ToDecimal(10);
                reviewInDb.Date = DateTime.Now;
                reviewInDb.Time = DateTime.Now;
                reviewInDb.ReviewOwner = User.Identity.Name;

                var productID = reviewInDb.productId;
                //var productInDb = _context.Products.FirstOrDefault(x => x.Id == reviewInDb.productId);
                //productInDb.TotalPercentageRate = productInDb.TotalPercentageRate - review.Ratio;
                //productInDb.TotalPercentageRate = productInDb.TotalPercentageRate + reviewInDb.Ratio;

                _context.SaveChanges();
                return RedirectToAction("ReviewsOfProduct", "Review", new { id = productID });
            }
            catch
            {
                return Content("Failed");
            }
        }

        // GET: Review/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    var reviewInDb = _context.Reviews.SingleOrDefault(r => r.Id == id);
                    var reviewRatioInDb = reviewInDb.Ratio;

                    _context.Reviews.Remove(reviewInDb);
                    _context.SaveChanges();
                    var productInDb = _context.Products.SingleOrDefault(x => x.Id == reviewInDb.productId);
                    var productId = productInDb.Id;
                    //if (productInDb.TotalPercentageRate >= 0)
                    //{
                    //    productInDb.TotalPercentageRate = productInDb.TotalPercentageRate - reviewRatioInDb;
                    //}
                    //else if (productInDb.TotalPercentageRate < 0)
                    //{
                    //    productInDb.TotalPercentageRate = productInDb.TotalPercentageRate + reviewRatioInDb;
                    //}

                    //_context.SaveChanges();

                    return RedirectToAction("ReviewsOfProduct", "Review", new { id = productId }); //"ReviewsOfProduct", new { id = review.productId }
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetReview(int Id)
        {
            var review = _context.Reviews.FirstOrDefault(x => x.Id == Id);
            return Json(review, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string EditComment(int reviewId, string comment)
        {
            try
            {
                var reviewInDb = _context.Reviews.SingleOrDefault(r => r.Id == reviewId);

                reviewInDb.Comment = comment;
                reviewInDb.Rate = 0;

                string[] commentSplitted = comment.Split(null);
                List<int> specialValue = new List<int>();
                List<int> sentimentWordValue = new List<int>();
                List<int> rateValues = new List<int>();
                var value = 0;
                var after = false;
                var before = false;
                var specialWordsInDb = _context.SentimentsSpecialDb.ToList();
                var sentimentWordsInDb = _context.SentimentsDb.ToList();

                var productInDb = _context.Products.FirstOrDefault(x => x.Id == reviewInDb.productId);
                //productInDb.TotalPercentageRate = productInDb.TotalPercentageRate - reviewInDb.Ratio;

                for (var i = 0; i <= commentSplitted.Count() - 1; i++)
                {
                    var existedSentiment = sentimentWordsInDb.Exists(x => x.Word.Equals(commentSplitted[i]));
                    if (existedSentiment)
                    {
                        if (i != (commentSplitted.Count() - 1) && i == 0) // awl klma
                        {
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            after = specialWordsInDb.Exists(x => x.SpecialWord.Equals(commentSplitted[i + 1]));
                            var afterWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i + 1])?.Value;
                            if (after == false)
                            {
                                rateValues.Add(value);
                            }
                            else if (after == true)
                            {
                                if (value > 0)
                                {
                                    value = value + afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = value - afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                i++;
                            }
                        }
                        else if (i != (commentSplitted.Count() - 1) && i != 0) // msh awl klma w msh l a5era
                        {
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            after = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i + 1]);
                            before = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i - 1]);

                            var afterWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i + 1])?.Value;
                            var beforeWordValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i - 1])?.Value;

                            // law mfesh la ablha wla b3dha
                            if (after == false && before == false)
                            {
                                rateValues.Add(value);
                            }

                            // law feh b3dha bs
                            else if (after == true && before == false)
                            {
                                if (value > 0)
                                {
                                    value = value + afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = value - afterWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                i++;
                            }

                            // law feh ablha bs
                            else if (after == false && before == true)
                            {
                                if (value > 0)
                                {
                                    value = value * beforeWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = (value * beforeWordValue ?? 1) + 2;
                                    rateValues.Add(value);
                                }

                            }

                            // law feh ablha w b3dha
                            else if (before == true && after == true)
                            {
                                var afterWordEx = commentSplitted[i + 1];
                                var specialAfter = _context.SentimentsSpecialDb.FirstOrDefault(x => x.SpecialWord == afterWordEx).IsBefore;
                                var beforeSpecialValue = specialWordsInDb.FirstOrDefault(x => x.SpecialWord == commentSplitted[i - 1]).Value;
                                // law 0 yb2a after law 1 yb2a weak law 2 yb2a strong
                                if (specialAfter == 1) // case gdn aw awi
                                {
                                    if (value > 0)
                                    {

                                        value = ((afterWordValue ?? 1) * beforeSpecialValue) + value;
                                        rateValues.Add(value);
                                    }
                                    else
                                    {
                                        value = value + (afterWordValue ?? 1);
                                        rateValues.Add(value);
                                    }
                                }
                                else if (specialAfter == 2) // case 5ales
                                {
                                    if (value > 0)
                                    {
                                        value = (value + afterWordValue ?? 1) * beforeSpecialValue;
                                        rateValues.Add(value);
                                    }
                                    else
                                    {
                                        value = (value * beforeSpecialValue) + afterWordValue ?? 1;
                                        rateValues.Add(value);
                                    }
                                }
                                i++;
                            }
                        }
                        else if (commentSplitted.Count() == 1 && i == 0) // klma lw7dha
                        {
                            value = 0;
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            rateValues.Add(value);
                        }
                        else
                        {
                            value = 0;
                            value = sentimentWordsInDb.FirstOrDefault(x => x.Word == commentSplitted[i]).Value;
                            before = specialWordsInDb.Exists(x => x.SpecialWord == commentSplitted[i - 1]);
                            var wordbefore = commentSplitted[i - 1];
                            var beforeWordValue = _context.SentimentsSpecialDb.SingleOrDefault(x => x.SpecialWord == wordbefore)?.Value;

                            // law mfesh la ablha wla b3dha
                            if (before == false)
                            {
                                rateValues.Add(value);
                            }

                            // law feh ablha bs
                            else if (before == true && after == false)
                            {
                                if (value > 0)
                                {
                                    value = value * beforeWordValue ?? 1;
                                    rateValues.Add(value);
                                }
                                else
                                {
                                    value = (value * beforeWordValue ?? 1) - 2;
                                    rateValues.Add(value);
                                }

                            }
                        }

                    }
                }
                foreach (var item in rateValues)
                {
                    reviewInDb.Rate += item;
                }
                reviewInDb.Ratio = Convert.ToDecimal(reviewInDb.Rate) / Convert.ToDecimal(10);
                reviewInDb.Date = DateTime.Now;
                reviewInDb.Time = DateTime.Now;
                reviewInDb.ReviewOwner = User.Identity.Name;

                //var productID = reviewInDb.productId;

                //productInDb.TotalPercentageRate = productInDb.TotalPercentageRate + reviewInDb.Ratio;

                _context.SaveChanges();
                return "success";
            }
            catch
            {
                return "Failed";
            }
        }
    }

}
