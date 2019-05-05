using System.Collections;

namespace IntralismSharedEditor
{
    public class CustomEventEditorHandler_InputField : CustomEventEditorHandler
    {
        public string text;

        /// <summary>
        /// Метод Init()
        /// </summary>
        /// <param name="data">Исходная дата</param>
        /// <param name="advParametrs">Параметры</param>
        public override void Init(string data, string advParametrs)
        {
            base.Init(data, advParametrs);

            if (this.advParametrs.Contains("float"))
            {
                float result = float.Parse(this.advParametrs[1], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

                if (!string.IsNullOrEmpty(data))
                    float.TryParse(data, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out result);

                text = "" + result;
            }
            else
                text = data;


        }

        /// <summary>
        /// Метод GetEditedData() возвращает string из параметров для data
        /// </summary>
        public override string GetEditedData()
        {
            return text;
        }
    }
}