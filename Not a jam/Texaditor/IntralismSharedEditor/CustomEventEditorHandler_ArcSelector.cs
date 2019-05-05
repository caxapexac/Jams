using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntralismSharedEditor
{
    public class CustomEventEditorHandler_ArcSelector : CustomEventEditorHandler
    {
        private int selected = 0;

        /// <summary>
        /// Метод Init()
        /// </summary>
        /// <param name="data">Исходная дата</param>
        /// <param name="advParametrs">Параметры</param>
        public override void Init(string data, string advParametrs)
        {
            base.Init(data, advParametrs);
            selected = Helpers.patternsMap.IndexOf(Helpers.patternsMap.Find(x => x.Contains(data)));
        }

        /// <summary>
        /// Метод GetEditedData() возвращает string из параметров для data
        /// </summary>
        public override string GetEditedData()
        {
            data = Helpers.patternsMap[selected];
            return "" + data;
        }

        public void Selected(int id)
        {
            //Debug.Log(id);
            selected = id;
        }
    }
}