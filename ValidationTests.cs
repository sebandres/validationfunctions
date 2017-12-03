using Xunit;

namespace funcvalidation
{
  public class ValidationTests
  {
    [Fact]
    public void StringIsGood()
    {
      var valueToValidate = "Bla";
      var error = Validation.ValidateFreeTextInput(valueToValidate);
      Assert.Empty(error);
    }

    [Fact]
    public void StringIsTooShort()
    {
      var valueToValidate = "Bl";
      var error = Validation.ValidateFreeTextInput(valueToValidate);
      Assert.Single(error);
    }

    [Fact]
    public void StringIsEmpty()
    {
      var valueToValidate = "";
      var error = Validation.ValidateFreeTextInput(valueToValidate);
      Assert.Equal<string>(
        new[] { "Cannot be empty", "Length must be 3 or more" },
        error
      );
    }

    [Fact]
    public void StringIsNull()
    {
      string valueToValidate = null;
      var error = Validation.ValidateFreeTextInput(valueToValidate);
      Assert.NotEmpty(error);
    }

    [Fact]
    public void DateIsNull()
    {
      string valueToValidate = null;
      var error = Validation.ValidateDOBInput(valueToValidate);
      Assert.NotEmpty(error);
    }

    [Fact]
    public void DateIsValid()
    {
      string valueToValidate = "1990-01-01";
      var error = Validation.ValidateDOBInput(valueToValidate);
      Assert.Empty(error);
    }

    [Fact]
    public void DateIsLessThan18yo()
    {
      string valueToValidate = "2010-01-01";
      var errors = Validation.ValidateDOBInput(valueToValidate);
      Assert.Single(errors);
      Assert.Equal("Must be over 18", errors[0]);
    }
  }
}
