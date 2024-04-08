using System;
using FluentAssertions;
namespace WebApiTest.Core.Misc;

public class DateTimeExtensionUt {


   [Fact]
   public void ToUniversalTimeUt() {
      // Arrange
      var localNow = DateTime.Now;
      // Act
      var utcNow = localNow.ToUniversalTime();
      // Assert
      localNow.Kind.Should().Be(DateTimeKind.Local);
      utcNow.Kind.Should().Be(DateTimeKind.Utc);
      var offset = TimeZoneInfo.Local.GetUtcOffset(localNow);
      utcNow.Should().Be(localNow - offset);
   }

   [Fact]
   public void ToLocaLTimeUt() {
      // Arrange
      var utcNow = DateTime.UtcNow;
      // Act
      var localNow = utcNow.ToLocalTime();
      // Assert
      localNow.Kind.Should().Be(DateTimeKind.Local);
      utcNow.Kind.Should().Be(DateTimeKind.Utc);
      var offset = TimeZoneInfo.Local.GetUtcOffset(localNow);
      localNow.Should().Be(utcNow + offset);
   }
   
/*   

   [Fact]
   public void FormIso8601String20Ut() {
      // Arrange
      string iso = "2023-12-31T23:40:50Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50).ToUniversalTime();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   
   [Fact]
   public void FormIso8601String21Ut() {
      // Arrange
      string iso = "2023-12-31T23:40:50.Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50).ToUniversalTime();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   
   [Fact]
   public void FormIso8601String22Ut() {
      // Arrange
      string iso = "2023-12-31T23:40:50.1Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 100).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }

   [Fact]
   public void FormIso8601String23Ut() {
      // Arrange
      string iso = "2023-12-31T23:40:50.12Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 120).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   
   [Fact]
   public void FormIso8601String24Ut() {
      // Arrange
      string iso = "2023-12-31T23:40:50.123Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 123).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   [Fact]
   public void FormIso8601String25Ut() {
      // Arrange
      string iso = "2023-12-31T23:40:50.1234Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 123)
         .AddMicroseconds(400).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   [Fact]
   public void FormIso8601String26Ut() {
      // Arrange
      string iso = "2023-12-31T23:40:50.12345Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 123)
         .AddMicroseconds(450).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   [Fact]
   public void FormIso8601String27Ut() {
      // Arrange
      string iso = "2023-12-31T23:40:50.123456Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 123)
         .AddMicroseconds(456).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   [Fact]
   public void FormIso8601String28Ut() {
      // Arrange  nanoseconds
      string iso = "2023-12-31T23:40:50.1234567Z";
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 123)
         .AddMicroseconds(456).AddNanoseconds(700).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   [Fact]
   public void FormIso8601String29Ut() {
      // Arrange  nanoseconds
      string iso = "2023-12-31T23:40:50.12345678Z";
      // is cut to  2023-12-31T23:40:50.1234567Z
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 123)
         .AddMicroseconds(456).AddNanoseconds(700).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   [Fact]
   public void FormIso8601String30Ut() {
      // Arrange  nanoseconds
      string iso = "2023-12-31T23:40:50.123456789Z";
      // is cut to  2023-12-31T23:40:50.1234567Z
      var expected = new DateTime(2023, 12, 31, 23, 40, 50, 123)
         .AddMicroseconds(456).AddNanoseconds(700).SetKindUtc();
      // Act
      var utcDateTime = DateTimeExt.FromIso8601String(iso);
      var localDateTime = utcDateTime.ToLocalTime();
      // Assert
      utcDateTime.Should().Be(expected);
      localDateTime.Should().Be(expected.ToLocalTime());
   }
   */
}