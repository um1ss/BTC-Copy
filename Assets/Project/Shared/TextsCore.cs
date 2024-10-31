namespace Project.Shared
{
    public class Text {
        public readonly string text;

        public Text(string text) {
            this.text = text;
        }

        public string GetRawText() => text;
    }

    public class Text0 : Text {
        public Text0(string key) : base(key) { }

        public string GetText() => GetRawText();
    }

    public class Text1 : Text {
        public Text1(string key) : base(key) { }

        public string GetText(object replace1) =>
            GetRawText().Replace("{1}", replace1.ToString());
    }

    public class Text2 : Text {
        public Text2(string key) : base(key) { }

        public string GetText(object replace1, object replace2) =>
            GetRawText().Replace("{1}", replace1.ToString()).Replace("{2}", replace2.ToString());
    }
}