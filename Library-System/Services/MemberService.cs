using Npgsql;
using System;
using System.Collections.Generic;

namespace library_system
{
    public class MemberService
    {
        private readonly Database db = new Database();

        public List<Member> GetAllMembers()
        {
            List<Member> members = new List<Member>();

            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT MemberID, Name, Gender, Phone,
                           Email, Address, RegisterDate, Status
                    FROM Member
                    ORDER BY MemberID";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(MapMember(reader));
                    }
                }
            }

            return members;
        }

        public void AddMember(Member member)
        {
            ValidateMember(member);
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    INSERT INTO Member
                    (
                        Name,
                        Gender,
                        Phone,
                        Email,
                        Address,
                        RegisterDate,
                        Status
                    )
                    VALUES
                    (
                        @name,
                        @gender,
                        @phone,
                        @email,
                        @address,
                        @registerDate,
                        @status
                    )";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    AddMemberParameters(cmd, member);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateMember(Member member)
        {
            ValidateMember(member);
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE Member
                    SET Name = @name,
                        Gender = @gender,
                        Phone = @phone,
                        Email = @email,
                        Address = @address
                    WHERE MemberID = @id";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", member.MemberID);
                    AddMemberParameters(cmd, member);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeactivateMember(int memberID)
        {
            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE Member
                    SET Status = 'Inactive'
                    WHERE MemberID = @id";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", memberID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Member> SearchMembers(string keyword)
        {
            List<Member> members = new List<Member>();

            using (NpgsqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT MemberID, Name, Gender, Phone,
                           Email, Address, RegisterDate, Status
                    FROM Member
                    WHERE Name ILIKE @keyword
                       OR Phone ILIKE @keyword
                       OR Email ILIKE @keyword
                    ORDER BY MemberID";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            members.Add(MapMember(reader));
                        }
                    }
                }
            }

            return members;
        }

        private Member MapMember(NpgsqlDataReader reader)
        {
            return new Member
            {
                MemberID = Convert.ToInt32(reader["MemberID"]),
                Name = reader["Name"].ToString() ?? "",
                Gender = reader["Gender"].ToString() ?? "",
                Phone = reader["Phone"].ToString() ?? "",
                Email = reader["Email"].ToString() ?? "",
                Address = reader["Address"].ToString() ?? "",
                RegisterDate = reader.GetDateTime(reader.GetOrdinal("RegisterDate")),
                Status = reader["Status"].ToString() ?? "Active"
            };
        }

        private static void ValidateMember(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.Name))
                throw new InvalidOperationException("Enter the member name.");
            if (member.Status != "Active" && member.Status != "Inactive")
                throw new InvalidOperationException("Invalid member status.");
        }

        private void AddMemberParameters(
            NpgsqlCommand cmd,
            Member member)
        {
            cmd.Parameters.AddWithValue("@name", member.Name);
            cmd.Parameters.AddWithValue("@gender", member.Gender);
            cmd.Parameters.AddWithValue("@phone", member.Phone);
            cmd.Parameters.AddWithValue("@email", member.Email);
            cmd.Parameters.AddWithValue("@address", member.Address);
            cmd.Parameters.AddWithValue("@registerDate", NpgsqlTypes.NpgsqlDbType.Date, member.RegisterDate.Date);
            cmd.Parameters.AddWithValue("@status", member.Status);
        }
    }
}
