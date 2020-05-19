namespace review_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProdctsSmmaryView : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE VIEW ProductsSummary AS Select Prod.Id,                                " +
                                             "Prod.CompanyName,                             " +
                                             "Prod.Name,                                    " +
                                             "Cat.Category,                                 " +
                                             "Prod.TotalPercentageRate,                     " +
                                             "isnull(reviews.AverageRate, 0) as             " +
                                             "AverageRate,                                  " +
                                             "isnull(reviews.ReviewsCount, 0) as            " +
                                             "ReviewsCount                                  " +
                                             "                                              " +
                                             "From Products as Prod                         " +
                                             "Left Join(Select Reviews.productId,           " +
                                             "AVG(Reviews.Rate) As AverageRate,             " +
                                             "COUNT(Reviews.Id) as                          " +
                                             "ReviewsCount                                  " +
                                             "From Reviews                                  " +
                                             "Group by Reviews.productId) as reviews        " +
                                             "on reviews.productId = Prod.Id                " +
                                             "Inner Join ProductCategories as Cat on        " +
                                             "Cat.Id = Prod.ProductCategoryId               ");
        }
        public override void Down()
        {
            Sql("Drop View ProductsSummary");
        }
    }
}
