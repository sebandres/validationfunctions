namespace funcvalidation
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  public class Validation
  {
    public static string ValidateCsvRow(string[] headers, int[] columnTypes, string[] values)
    {
      List<Task<string>> tasks = new List<Task<string>>();
      for (var i = 0; i < columnTypes.Length; i++)
      {
        var columnType = columnTypes[i];
        var header = headers[i];
        var value = values[i];
        tasks.Add(new Task<string>(() =>
        {
          var errors = Questions.ValidateQuestion(columnType, value).Aggregate("", (a, b) => a.Length == 0 ? b : $"{a}, {b}");
          return errors.Length == 0 ? string.Empty : $"{header}: {errors}";
        }));
      }

      tasks.ForEach(t => t.Start());

      return Task.WhenAll(tasks)
              .GetAwaiter()
              .GetResult()
              .Aggregate("", (a, b) => a.Length == 0 ? b : b.Length == 0 ? a : $"{a}; {b}");
    }

    public class Questions
    {
      public static string[] ValidateQuestion(int type, string value)
      {
        switch (type)
        {
          case 1: //Free text input
            {
              return Validation.ValidateFreeTextInput(value);
            }
          case 2: //DOB Field
            {
              return Validation.ValidateDOBInput(value);
            }
          default:
            throw new ArgumentOutOfRangeException(nameof(type), "Unknown question type");
        }
      }
    }

    private class DateValidation
    {
      public static string IsDate(string value)
      {
        return !DateTime.TryParse(value, out var date) ? "Input is not a date" : string.Empty;
      }
    }

    private class BirthDayValidation
    {
      public static string IsAlive(string value)
      {
        return (DateTime.TryParse(value ?? "", out var year) ? year : DateTime.MinValue).Year < 1920 ? "Should not be alive at the moment" : string.Empty;
      }

      public static string Over18(string value)
      {
        return DateTime.Now.Year - (DateTime.TryParse(value ?? "", out var year) ? year : DateTime.MinValue).Year < 18 ? "Must be over 18" : string.Empty;
      }
    }

    private class StringValidation
    {
      public static string RequiredString(string value)
      {
        return String.IsNullOrEmpty(value) ? "Cannot be empty" : string.Empty;
      }

      public static string Min3LengthString(string value)
      {
        return value?.Length < 3 ? "Length must be 3 or more" : string.Empty;
      }
    }

    public static string[] ValidateFreeTextInput(string value)
    {
      return Task.WhenAll<string>(
          Task.Run(() => StringValidation.RequiredString(value)),
          Task.Run(() => StringValidation.Min3LengthString(value))
        )
        .GetAwaiter()
        .GetResult()
        .Where(err => !string.IsNullOrEmpty(err))
        .ToArray();
    }

    public static string[] ValidateDOBInput(string value)
    {
      return Task.WhenAll<string>(
          Task.Run(() => StringValidation.RequiredString(value)),
          Task.Run(() => DateValidation.IsDate(value)),
          Task.Run(() => BirthDayValidation.IsAlive(value)),
          Task.Run(() => BirthDayValidation.Over18(value))
        )
        .GetAwaiter()
        .GetResult()
        .Where(err => !string.IsNullOrEmpty(err))
        .ToArray();
    }
  }
}
