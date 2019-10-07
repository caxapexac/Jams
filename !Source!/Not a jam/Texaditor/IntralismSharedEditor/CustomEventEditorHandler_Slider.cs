using System.Collections;

namespace IntralismSharedEditor
{
    public class CustomEventEditorHandler_Slider : CustomEventEditorHandler
    {
        public float minValue;
        public float maxValue;
        public bool wholeNumbers;
        public string mask;
        public float value;

        /// <summary>
        /// Метод Init()
        /// </summary>
        /// <param name="data">Исходная дата</param>
        /// <param name="advParametrs">Параметры</param>
        public override void Init(string data, string advParametrs)
        {
            base.Init(data, advParametrs);


            float minValue = 0;
            float maxValue = 1;
            bool wholeNumbers = false;
            string mask = "0.00";

            float.TryParse(this.advParametrs[0], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out minValue);
            float.TryParse(this.advParametrs[1], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out maxValue);
            bool.TryParse(this.advParametrs[2], out wholeNumbers);
            if (this.advParametrs.Count > 3)
                mask = this.advParametrs[3];

            /*Debug.Log("min: " + minValue);
            Debug.Log("max: " + maxValue);
            Debug.Log("wholeNumbers: " + wholeNumbers);*/

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.wholeNumbers = wholeNumbers;
            this.mask = mask;


            float value = 0;
            float.TryParse(data, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out value);
            this.value = value;
        }

        /// <summary>
        /// Метод GetEditedData() возвращает string из параметров для data
        /// </summary>
        public override string GetEditedData()
        {
            //Debug.Log("input.value" + input.value);
            return "" + value;
        }
    }
}