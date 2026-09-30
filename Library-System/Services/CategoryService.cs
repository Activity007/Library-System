
using Npgsql;
using System;
using System.Collections.Generic;

namespace library_system
{
    public class CategoryService
    {
        private readonly Database db = new Database();

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT CategoryID, CategoryName, Description
                    FROM Category
                    ORDER BY CategoryID";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            CategoryName = reader["CategoryName"].ToString() ?? "",
                            Description = reader["Description"].ToString() ?? ""
                        });
                    }
                }
            }

            return categories;
        }

        public void AddCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new InvalidOperationException("Enter a category name.");
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    INSERT INTO Category (CategoryName, Description)
                    VALUES (@name, @description)";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", category.CategoryName);
                    cmd.Parameters.AddWithValue("@description", category.Description);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new InvalidOperationException("Enter a category name.");
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE Category
                    SET CategoryName = @name,
                        Description = @description
                    WHERE CategoryID = @id";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", category.CategoryID);
                    cmd.Parameters.AddWithValue("@name", category.CategoryName);
                    cmd.Parameters.AddWithValue("@description", category.Description);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCategory(int categoryID)
        {
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    DELETE FROM Category
                    WHERE CategoryID = @id";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", categoryID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Category> SearchCategories(string keyword)
        {
            List<Category> categories = new List<Category>();

            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT CategoryID, CategoryName, Description
                    FROM Category
                    WHERE CategoryName ILIKE @keyword
                       OR Description ILIKE @keyword
                    ORDER BY CategoryID";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                                CategoryName = reader["CategoryName"].ToString() ?? "",
                                Description = reader["Description"].ToString() ?? ""
                            });
                        }
                    }
                }
            }

            return categories;
        }
    }
}
