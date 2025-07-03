using System.Text.Json;

namespace Domain.Models;

public class TBaseModel<TKey>
{
    public TKey? Id { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    public bool? IsActive { get; set; }
}

public class BaseModel : TBaseModel<string>
{
    public new string Id { get; set; } = string.Empty;
}

public class ExtBaseModel : BaseModel
{
    public string? ExtData { get; set; }
}


public static class BaseModelExt
{
    public static T GetExtData<T>(this ExtBaseModel? baseModel)
        where T : class, new()
    {
        if (baseModel is null
            || string.IsNullOrEmpty(baseModel.ExtData))
        {
            return new();
        }
            return JsonSerializer.Deserialize<T>(baseModel.ExtData)!; // Use JsonSerializer instead of JsonHelper
    }

    public static void UpdateExtData<T>(this ExtBaseModel baseModel, T data)
        where T : class, new()
    {
        if (data is not null && baseModel is not null)
        {
            baseModel.ExtData = JsonSerializer.Serialize(data); // Use JsonSerializer instead of JsonHelper
        }
    }

    public static void UpdateExtData2(this ExtBaseModel baseModel, object data)
    {
        if (data is not null && baseModel is not null)
        {
            var newData = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(baseModel.ExtData ?? "{}") ?? new Dictionary<string, dynamic>();

            var properties = data.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (newData.ContainsKey(prop.Name))
                {
                    newData[prop.Name] = prop.GetValue(data) ?? "";
                }
                else
                {
                    newData.Add(prop.Name, prop.GetValue(data) ?? "");
                }
            }

            baseModel.ExtData = JsonSerializer.Serialize(newData); // Use JsonSerializer instead of JsonHelper
        }
    }
}