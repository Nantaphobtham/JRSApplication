using JRSApplication.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

public class ThaiBahtTextConverter
{
    private static readonly string[] NumberTexts = { "", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };
    private static readonly string[] PositionTexts = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

    public string GetBahtText(decimal amount)
    {
        if (amount == 0)
            return "ศูนย์บาทถ้วน";

        string[] parts = amount.ToString("0.00").Split('.');
        string bahtPart = ConvertNumberToThaiText(parts[0]);
        string satangPart = ConvertNumberToThaiText(parts[1]);

        string result = bahtPart + "บาท";

        if (parts[1] == "00")
            result += "ถ้วน";
        else
            result += satangPart + "สตางค์";

        return result;
    }

    private string ConvertNumberToThaiText(string number)
    {
        StringBuilder result = new StringBuilder();

        int len = number.Length;
        int group = 0;

        while (len > 0)
        {
            int groupLength = Math.Min(6, len);
            string groupText = ConvertGroup(number.Substring(len - groupLength, groupLength));
            if (groupText != "")
            {
                if (group > 0)
                    result.Insert(0, "ล้าน");

                result.Insert(0, groupText);
            }

            len -= groupLength;
            group++;
        }

        return result.ToString();
    }

    private string ConvertGroup(string digits)
    {
        StringBuilder result = new StringBuilder();
        int len = digits.Length;

        for (int i = 0; i < len; i++)
        {
            int digit = int.Parse(digits[i].ToString());
            int pos = len - i - 1;

            if (digit == 0)
                continue;

            if (pos == 0 && digit == 1 && len > 1)
                result.Append("เอ็ด");
            else if (pos == 1 && digit == 2)
                result.Append("ยี่");
            else if (pos == 1 && digit == 1)
                result.Append("");
            else
                result.Append(NumberTexts[digit]);

            result.Append(PositionTexts[pos]);
        }

        return result.ToString();
    }
}
