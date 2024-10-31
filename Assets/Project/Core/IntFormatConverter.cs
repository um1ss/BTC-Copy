
public static class IntFormatConverter 
{
    public static string FormatInt(string updateText)
    {
        string text;
        switch (updateText.Length)
        {
            case 7:
                text = $"{updateText[0]}{updateText[1]}" + "A";
                return text;
            case 8:
                text = $"{updateText[0]}{updateText[1]}" + "AA";
                return text;
            case 9:
                text = $"{updateText[0]}{updateText[1]}" + "AAA";
                return text;
            case 10:
                text = $"{updateText[0]}{updateText[1]}" + "B";
                return text;
            case 11:
                text = $"{updateText[0]}{updateText[1]}" + "BB";
                return text;
            case 12:
                text = $"{updateText[0]}{updateText[1]}" + "BBB";
                return text;
            case 13:
                text = $"{updateText[0]}{updateText[1]}" + "C";
                return text;
            case 14:
                text = $"{updateText[0]}{updateText[1]}" + "CC";
                return text;
            case 15:
                text = $"{updateText[0]}{updateText[1]}" + "CCC";
                return text;
            case 16:
                text = $"{updateText[0]}{updateText[1]}" + "D";
                return text;
            case 17:
                text = $"{updateText[0]}{updateText[1]}" + "DD";
                return text;
            case 18:
                text = $"{updateText[0]}{updateText[1]}" + "DDD";
                return text;
            default:
                return updateText;
        }
    }
}
