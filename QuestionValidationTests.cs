namespace funcvalidation
{
  using Xunit;
  public class QuestionValidationTests
  {
    [Fact]
    public void ValidateSingleColumnRow()
    {
      var csvRow = "Se";
      var csvColumnType = new int[] { 1 };
      var errorMsg = Validation.ValidateCsvRow(csvColumnType, csvRow.Split(','));
      Assert.NotEmpty(errorMsg);
      Assert.Equal("Length must be 3 or more", errorMsg);
    }

    [Fact]
    public void ValidateTwoColumnRow()
    {
      var csvRow = "Sebastian,Andres";
      var csvColumnType = new int[] { 1, 1 };
      var errorMsg = Validation.ValidateCsvRow(csvColumnType, csvRow.Split(','));
      Assert.Empty(errorMsg);
    }

    [Fact]
    public void ValidateTwoColumnShortSecondColumnRow()
    {
      var csvRow = "Sebastian,An";
      var csvColumnType = new int[] { 1, 1 };
      var errorMsg = Validation.ValidateCsvRow(csvColumnType, csvRow.Split(','));
      Assert.NotEmpty(errorMsg);
      Assert.Equal("Length must be 3 or more", errorMsg);
    }

    [Fact]
    public void ValidateMultiColumnRow()
    {
      var csvRow = "2017-01-01,Sebastian,Andres,No Thanks";
      var csvColumnType = new int[] { 2, 1, 1, 1 };
      var errorMsg = Validation.ValidateCsvRow(csvColumnType, csvRow.Split(','));
      Assert.NotEmpty(errorMsg);
      Assert.Equal("", errorMsg);
    }

    [Fact]
    public void ValidateMultiColumnWithErrorsRow()
    {
      var csvRow = "2017-01-,eb,An,No Thanks";
      var csvColumnType = new int[] { 2, 1, 1, 1 };
      var errorMsg = Validation.ValidateCsvRow(csvColumnType, csvRow.Split(','));
      Assert.NotEmpty(errorMsg);
      Assert.Equal("Input is not a date, Should not be alive at the moment, Length must be 3 or more, Length must be 3 or more", errorMsg);
    }
  }
}