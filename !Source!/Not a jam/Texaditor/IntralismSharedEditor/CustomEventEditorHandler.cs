using IntralismSharedEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IntralismSharedEditor
{
    public class CustomEventEditorHandler
    {
        public EditorEventFunctionInfo.EditorEventParametr currentParametr;
        public string data = "";
        public List<string> advParametrs = null;

        /// <summary>
        /// Метод Init()
        /// </summary>
        /// <param name="data">Исходная дата</param>
        /// <param name="advParametrs">Параметры</param>
        public virtual void Init(string data, string advParametrs = null)
        {
            this.data = data;
            if (!string.IsNullOrEmpty(advParametrs))
            {
                this.advParametrs = advParametrs.Split(',').OfType<string>().ToList();
            }
        }

        /// <summary>
        /// Метод GetEditedData() возвращает string из параметров для data
        /// </summary>
        public virtual string GetEditedData()
        {
            return data;
        }

        public virtual void Update()
        {

        }
    }
}