using Npgsql;
using System;
using System.Collections.Generic;

namespace library_system
{
    public class BookService
    {
        private readonly Database db = new Database();

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT BookID, ISBN, Title, Author, CategoryID,
                           Publisher, PublishYear, Quantity, AvailableQuantity
                    FROM Book
                    ORDER BY BookID";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(MapBook(reader));
                    }
                }
            }

            return books;
        }

        public void AddBook(Book book)
        {
            ValidateBook(book);
            book.AvailableQuantity = book.Quantity;
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    INSERT INTO Book
                    (
                        ISBN,
                        Title,
                        Author,
                        CategoryID,
                        Publisher,
                        PublishYear,
                        Quantity,
                        AvailableQuantity
                    )
                    VALUES
                    (
                        @isbn,
                        @title,
                        @author,
                        @categoryID,
                        @publisher,
                        @publishYear,
                        @quantity,
                        @availableQuantity
                    )";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    AddBookParameters(cmd, book);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateBook(Book book)
        {
            ValidateBook(book);
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE Book
                    SET ISBN = @isbn,
                        Title = @title,
                        Author = @author,
                        CategoryID = @categoryID,
                        Publisher = @publisher,
                        PublishYear = @publishYear,
                        Quantity = @quantity,
                        AvailableQuantity = AvailableQuantity + @quantity - Quantity
                    WHERE BookID = @id AND @quantity >= Quantity - AvailableQuantity";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", book.BookID);
                    AddBookParameters(cmd, book);

                    if (cmd.ExecuteNonQuery() != 1)
                        throw new InvalidOperationException("The book was removed, or its quantity is less than the number of copies currently on loan.");
                }
            }
        }

        public void DeleteBook(int bookID)
        {
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    DELETE FROM Book
                    WHERE BookID = @id";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", bookID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Book> SearchBooks(string keyword)
        {
            List<Book> books = new List<Book>();

            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT BookID, ISBN, Title, Author, CategoryID,
                           Publisher, PublishYear, Quantity, AvailableQuantity
                    FROM Book
                    WHERE ISBN ILIKE @keyword
                       OR Title ILIKE @keyword
                       OR Author ILIKE @keyword
                       OR Publisher ILIKE @keyword
                    ORDER BY BookID";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(MapBook(reader));
                        }
                    }
                }
            }

            return books;
        }

        private Book MapBook(NpgsqlDataReader reader)
        {
            return new Book
            {
                BookID = Convert.ToInt32(reader["BookID"]),
                ISBN = reader["ISBN"].ToString() ?? "",
                Title = reader["Title"].ToString() ?? "",
                Author = reader["Author"].ToString() ?? "",
                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                Publisher = reader["Publisher"].ToString() ?? "",
                PublishYear = Convert.ToInt32(reader["PublishYear"]),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                AvailableQuantity = Convert.ToInt32(reader["AvailableQuantity"])
            };
        }

        private static void ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.ISBN) || string.IsNullOrWhiteSpace(book.Title) ||
                string.IsNullOrWhiteSpace(book.Author) || book.CategoryID <= 0)
                throw new InvalidOperationException("ISBN, title, author, and a valid category are required.");
            if (book.Quantity < 0 || book.PublishYear < 1 || book.PublishYear > DateTime.Today.Year + 1)
                throw new InvalidOperationException("Enter a valid quantity and publication year.");
        }

        private void AddBookParameters(NpgsqlCommand cmd, Book book)
        {
            cmd.Parameters.AddWithValue("@isbn", book.ISBN);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@author", book.Author);
            cmd.Parameters.AddWithValue("@categoryID", book.CategoryID);
            cmd.Parameters.AddWithValue("@publisher", book.Publisher);
            cmd.Parameters.AddWithValue("@publishYear", book.PublishYear);
            cmd.Parameters.AddWithValue("@quantity", book.Quantity);
            cmd.Parameters.AddWithValue("@availableQuantity", book.AvailableQuantity);
        }
    }
}
