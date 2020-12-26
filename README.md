this eextention for .net core to work directly with database using database context, its allow to send sql for create/update/delete and select, also its work well with stored views and procedures

# How its work!

after install plugin ..

- create class model that present row of return data

```sh
      public class Student
        {
            public int ID { get; set; }
            public string LastName { get; set; }
            public string FirstMidName { get; set; }
            public DateTime EnrollmentDate { get; set; }
        }
```

- send sql to database

```sh
    var sql = "SELECT ID, LastName, FirstMidName, EnrollmentDate FROM Student";
    List<Student> students = await _context.ExecuteRawQueryAsync<Student>(sql);
```

- it can send parameters with query

```sh
    var sql = "SELECT ID, LastName, FirstMidName, EnrollmentDate FROM Student WHERE Id = @StudentID";
     object[] parameters =
            {
                new SqlParameter("@StudentID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Input,
                    Value = 4
                }
    };
    List<Student> students = await _context.ExecuteRawQueryAsync<Student>(sql, parameters);
```

. to insert/update/delete row

```sh
    var sql = "INSERT INTO dbo.Student(LastName, FirstMidName, EnrollmentDate)
                            VALUES(@LastName, @FirstName, @CreatedAt)";

   object[] parameters = {
                new SqlParameter("@LastName", SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Input,
                    Value = "San"
                },
                new SqlParameter("@FirstName", SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Input,
                    Value = "Nami"
                },
                new SqlParameter("@CreatedAt", SqlDbType.DateTime2)
                {
                    Direction = ParameterDirection.Input,
                    Value = DateTime.Now
                }
   };
   await _context.ExecuteRawQueryAsync<object>(sql, parameters);
```

- to call stored view

```sh

```

- to call stored procedure

```sh

```

any suggestions for improvement are welcome `<link>` : <https://twitter.com/gheith3>
