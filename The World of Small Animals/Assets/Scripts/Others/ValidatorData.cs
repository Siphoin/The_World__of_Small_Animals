using System;

public static class ValidatorData
{

    public static void CheckValidFieldStats(System.Reflection.FieldInfo prop, object type)
    {
        object valueProp = prop.GetValue(type);
        if (valueProp.IsNumber())
        {
            int value = Convert.ToInt32(valueProp);

            if (value <= 0)
            {
                throw new ValidatorDataException($"invalid field: (Property: {prop.Name}) not be <= 0");
            }
        }

        else if (valueProp.IsString())
        {
            string value = Convert.ToString(valueProp);

            if (string.IsNullOrEmpty(value))
            {
                throw new ValidatorDataException($"invalid field: (Property: {prop.Name}) not be is null or emtry");
            }
        }
    }

    public static void CheckValidPropertyStats(System.Reflection.PropertyInfo prop, object type)
    {
        object valueProp = prop.GetValue(type);

        if (valueProp.IsNumber())
        {
            int value = Convert.ToInt32(valueProp);

            if (value <= 0)
            {
                throw new ValidatorDataException($"invalid field: (Property: {prop.Name}) not be <= 0");
            }
        }

        else if (valueProp.IsString())
        {
            string value = Convert.ToString(valueProp);

            if (string.IsNullOrEmpty(value))
            {
                throw new ValidatorDataException($"invalid field: (Property: {prop.Name}) not be is null or emtry");
            }
        }
    }
}