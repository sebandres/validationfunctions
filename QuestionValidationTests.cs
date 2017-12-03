namespace funcvalidation
{
  using Xunit;
  public class QuestionValidationTests
  {
    [Fact]
    public void ValidateSingleColumnRow()
    {
      var csvHeaders = "FirstName";
      var csvRow = "Se";
      var csvColumnType = new int[] { 1 };
      var errorMsg = Validation.ValidateCsvRow(csvHeaders.Split(','), csvColumnType, csvRow.Split(','));
      Assert.NotEmpty(errorMsg);
      Assert.Equal("FirstName: Length must be 3 or more", errorMsg);
    }

    [Fact]
    public void ValidateTwoColumnRow()
    {
      var csvHeaders = "FirstName,SurName";
      var csvRow = "Sebastian,Andres";
      var csvColumnType = new int[] { 1, 1 };
      var errorMsg = Validation.ValidateCsvRow(csvHeaders.Split(','), csvColumnType, csvRow.Split(','));
      Assert.Empty(errorMsg);
    }

    [Fact]
    public void ValidateTwoColumnShortSecondColumnRow()
    {
      var csvHeaders = "FirstName,SurName";
      var csvRow = "Sebastian,An";
      var csvColumnType = new int[] { 1, 1 };
      var errorMsg = Validation.ValidateCsvRow(csvHeaders.Split(','), csvColumnType, csvRow.Split(','));
      Assert.NotEmpty(errorMsg);
      Assert.Equal("SurName: Length must be 3 or more", errorMsg);
    }

    [Fact]
    public void ValidateMultiColumnRow()
    {
      var csvHeaders = "DOB,FirstName,SurName,Question1";
      var csvRow = "2017-01-01,Sebastian,Andres,No Thanks";
      var csvColumnType = new int[] { 2, 1, 1, 1 };
      var errorMsg = Validation.ValidateCsvRow(csvHeaders.Split(','), csvColumnType, csvRow.Split(','));
      Assert.NotEmpty(errorMsg);
      Assert.Equal("DOB: Must be over 18", errorMsg);
    }

    [Fact]
    public void ValidateMultiColumnWithErrorsRow()
    {
      var csvHeaders = "DOB,FirstName,SurName,Question1";
      var csvRow = "2017-01-,eb,An,No Thanks";
      var csvColumnType = new int[] { 2, 1, 1, 1 };
      var errorMsg = Validation.ValidateCsvRow(csvHeaders.Split(','), csvColumnType, csvRow.Split(','));
      Assert.NotEmpty(errorMsg);
      Assert.Equal("DOB: Input is not a date, Should not be alive at the moment; FirstName: Length must be 3 or more; SurName: Length must be 3 or more", errorMsg);
    }
  }
}