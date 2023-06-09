using Microsoft.AspNetCore.Mvc;
using MunchWebAPI;

namespace MUNCH.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  DataContextDapper _dapper;
  public UserController(IConfiguration config)
  {
    _dapper = new DataContextDapper(config);
  }

  [HttpGet("TestConnection")]
  public DateTime TestConnection()
  {
    return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
  }

  [HttpPost("AddNewUser")]
  public IActionResult AddUser(UserToAddDto user)
  {

    string sql = @"
    INSERT INTO MUNCH.Users(
      [Email],
      [FirstName],
      [LastName],
      [Password],
      [DateOfBirth],
      [CreatedAt]
    ) VALUES (" +
      "'" + user.Email +
      "', '" + user.FirstName +
      "', '" + user.LastName +
      "', '" + user.Password +
      "', '" + user.DateOfBirth +
      "', '" + DateTime.Now +
    "')";

    try
    {
      if (_dapper.ExecuteSql(sql))
      {
        string NewestUserId = GetNewestUser();
        AddRecordInInfinite_v1Table(NewestUserId);
        return Ok();
      }
      return Problem("Failed to execute SQL.", statusCode: 500);
    }

    catch (Exception ex)
    {
      throw new Exception("Failed to update Users table in database. Error code: " + ex);
    }
  }

  [HttpPost("AddRecordInInfinite_v1Table")]
  public IActionResult AddRecordInInfinite_v1Table(string UserId)
  {
    string sql = @"
    INSERT INTO MUNCH.Infinite_v1 (
      [UserId],
      [task1], 
      [task2], 
      [task3], 
      [task4], 
      [task5], 
      [task6]
      ) VALUES ( " +
      UserId +
      ", 0, 0, 0, 0, 0, 0)";

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }
    throw new Exception("Failed to update Infinite_v1 table in database.");
  }

  [HttpGet("GetUsers")]
  public IEnumerable<User> GetUsers()
  {
    string sql = @"
    SELECT 
      [UserId],
      [Email],
      [FirstName],
      [LastName],
      [Password],
      [DateOfBirth],
      [CreatedAt]
    FROM MUNCH.Users
    ";

    IEnumerable<User> users = _dapper.LoadData<User>(sql);
    return users;
  }

  [HttpGet("GetSingleUser/{userId}")]
  public User GetSingleUser(int userId)
  {
    string sql = @"
    SELECT 
      [UserId],
      [Email],
      [FirstName],
      [LastName],
      [Password],
      [DateOfBirth],
      [CreatedAt]
    FROM MUNCH.Users
    WHERE UserId = " + userId.ToString();

    User user = _dapper.LoadDataSingle<User>(sql);
    return user;
  }

  [HttpGet("GetNewestUser/")]
  public string GetNewestUser()
  {
    string sql = "SELECT MAX(UserId) AS MaxUserId FROM MUNCH.Users";
    string NewestUser = _dapper.LoadDataSingle<string>(sql);
    return NewestUser;
  }



  [HttpGet("GetInfinite_v1Scoreboard")]
  public ScoreboardInfiniteV1 GetInfinite_v1Scoreboard(string userId)
  {
    string sql = @"
      SELECT 
        [userID],
        [task1],
        [task2],
        [task3],
        [task4],
        [task5],
        [task6]
      FROM MUNCH.Infinite_v1
      WHERE UserId = " + userId.ToString();

    ScoreboardInfiniteV1 Scoreboard = _dapper.LoadDataSingle<ScoreboardInfiniteV1>(sql);
    return Scoreboard;
  }

  [HttpGet("GetSumOfScoreBoardInfinite_v1")]
  public ActionResult<string> GetSumOfScoreBoardInfinite_v1(int userId)
  {
    string sql = @"
        SELECT 
            [userID],
            [task1],
            [task2],
            [task3],
            [task4],
            [task5],
            [task6]
        FROM MUNCH.Infinite_v1
        WHERE UserId = " + userId.ToString();

    ScoreboardInfiniteV1 scoreboardInfiniteV1 = _dapper.LoadDataSingle<ScoreboardInfiniteV1>(sql);

    // Calculate the count of true values in the scoreboardInfiniteV1 object
    int count = new[] {
        scoreboardInfiniteV1.Task1,
        scoreboardInfiniteV1.Task2,
        scoreboardInfiniteV1.Task3,
        scoreboardInfiniteV1.Task4,
        scoreboardInfiniteV1.Task5,
        scoreboardInfiniteV1.Task6
    }.Count(value => value);

    return count.ToString();
  }

  [HttpPut("UpdateScoreInInfinite_v1Table")]
  public IActionResult UpdateInInfinite_v1Table(string TaskAndNumber, bool solved, int UserId)
  {
    int solvedInt;
    if (solved)
    {
      solvedInt = 1;
    }
    else
    {
      solvedInt = 0;
    }

    string sql = "UPDATE MUNCH.Infinite_v1 SET [" + TaskAndNumber + "] = " + solvedInt + " WHERE UserId = " + UserId;

    _dapper.ExecuteSql(sql);

    return Ok();
  }
}

