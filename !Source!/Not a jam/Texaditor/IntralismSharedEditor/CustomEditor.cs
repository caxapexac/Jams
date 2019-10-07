using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IntralismSharedEditor
{
    /// <summary>
    /// 
    /// </summary>
    public class CompareEvent : IComparer<Event>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Event x, Event y)
        {
            if (x.Time > y.Time)
            {
                return 1;
            }
            else if (x.Time < y.Time)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IComparer<Event> GetComparer()
        {
            return (IComparer<Event>)new CompareEvent();
        }
    }
    /// <summary>
    /// Основной класс кастомного редактора
    /// </summary>
    public static class CustomEditor
    {
        /// <summary>
        /// случайное число int32
        /// </summary>
        public static Random rand = new Random();
        
        /// <summary>
        /// Возвращает объект карты из config'а
        /// </summary>
        /// <param name="data">Текст из конфига</param>
        public static MapData GetMap(string data)
        {
            return JsonConvert.DeserializeObject<MapData>(data);
        }

        /// <summary>
        /// Возвращает текст config'а из объекта карты
        /// </summary>
        /// <param name="data">объект карты</param>
        public static string GetConfig(MapData data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Возвращает список тегов
        /// </summary>
        public static List<string> GetTags()
        {
            return Helpers.tags;
        }

        /// <summary>
        /// Возвращает описание определенного ивента
        /// </summary>
        /// <param name="mapEvent">ивыент</param>
        public static EditorEventFunctionInfo GetEditorEventInfo(MapEvent mapEvent)
        {
            return Helpers.eventsMap.Find(x => x.id == mapEvent.data[0]);
        }

        /// <summary>
        /// Возвращает редактор параметра для ивента. Использовать GetType() и typeof для уточнения класса)
        /// </summary>
        /// <param name="parametr">Параметр</param>
        /// <param name="value">Дата параметра</param>
        public static CustomEventEditorHandler GetParametrEditor(EditorEventFunctionInfo.EditorEventParametr parametr, string value)
        {
            CustomEventEditorHandler editorHandler = null;
            switch (parametr.editor)
            {
                case EditorEventFunctionInfo.EditorEventParametrType.ArcSelector:
                    editorHandler = new CustomEventEditorHandler_ArcSelector();
                    break;
                case EditorEventFunctionInfo.EditorEventParametrType.InputField:
                    editorHandler = new CustomEventEditorHandler_InputField();
                    break;
                case EditorEventFunctionInfo.EditorEventParametrType.Slider:
                    editorHandler = new CustomEventEditorHandler_Slider();
                    break;
                case EditorEventFunctionInfo.EditorEventParametrType.Toggle:
                    editorHandler = new CustomEventEditorHandler_Toggle();
                    break;
            }

            editorHandler.Init(value, parametr.data);

            return editorHandler;
        }
    }
}
