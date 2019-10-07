using System.Collections;

namespace IntralismSharedEditor
{
    public class CustomEventEditorHandler_Toggle : CustomEventEditorHandler
    {
        public bool isOn;
        public string text;


        /// <summary>
        /// Метод Init()
        /// </summary>
        /// <param name="data">Исходная дата</param>
        /// <param name="advParametrs">Параметры</param>
        public override void Init(string data, string advParametrs)
        {
            base.Init(data, advParametrs);

            bool value = false;
            bool.TryParse(data, out value);
            isOn = value;

            if (this.advParametrs.Count > 0)
                text = this.advParametrs[0];
        }

        /// <summary>
        /// Метод GetEditedData() возвращает string из параметров для data
        /// </summary>
        public override string GetEditedData()
        {
            return "" + isOn;
        }
    }
}