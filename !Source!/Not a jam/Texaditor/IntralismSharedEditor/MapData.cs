using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace IntralismSharedEditor
{
    /// <summary>
    /// Класс ивента
    /// </summary>
    public class MapEvent
    {
        /// <summary>
        /// double время эвента
        /// </summary>
        public double time;

        /// <summary>
        /// data[0] - ID метода
        /// data[1] - параметры метода
        /// пример: {"time":209.896347,"data":["SpawnObj","[Up],2"]}
        /// </summary>
        public List<string> data = new List<string>();

        /// <summary>
        /// Пустой эвент
        /// </summary>
        public MapEvent()
        {

        }

        /// <summary>
        /// Эвент, копирующий заданный в параметре
        /// </summary>
        /// <param name="mevent">MapEvent</param>
        /// <param name="isNew"></param>
        public MapEvent(MapEvent mevent, bool isNew = false)
        {
            this.time = System.Math.Round(mevent.time, 3);
            if (mevent.data != null)
            {
                if (isNew)
                {
                    this.data = new List<string>() { mevent.data[0] };
                    string buf = "";
                    for (int i = 1; i < mevent.data.Count; i++)
                    {
                        if(double.TryParse(mevent.data[i], out double d))
                        {
                            buf += mevent.data[i].Replace(",", ".") + ",";
                        }
                        else
                        {
                            buf += mevent.data[i] + ",";
                        }
                    }
                    if (buf.Length > 1) buf = buf.Substring(0, buf.Length - 1);
                    this.data.Add(buf);
                }
                else
                {
                    this.data = new List<string>(mevent.data.Count);
                    mevent.data.ForEach((item) =>
                    {
                        data.Add(item);
                    });
                }
            }
        }

        /// <summary>
        /// Создает эвент с заданным временем и содержанием
        /// </summary>
        /// <param name="newtime">время</param>
        /// <param name="newtype">тип</param>
        public MapEvent(double newtime, List<string> newtype)
        {
            this.time = newtime;
            this.data = newtype;
        }

    }

    /// <summary>
    /// Класс ресурса карты
    /// </summary>
    public class MapResource
    {
        /// <summary>
        /// ID ресурса
        /// </summary>
        public string name = "";
        /// <summary>
        /// Тип ресурса, пока только Sprite
        /// </summary>
        public string type = "";
        /// <summary>
        /// Относительный путь
        /// </summary>
        public string path = "";
        /// <summary>
        /// MD5 сумма ресурса
        /// </summary>
        public string hash = "";

        /// <summary>
        /// Пустой объект ресурса
        /// </summary>
        public MapResource()
        {

        }

        /// <summary>
        /// создаёт объект ресурса карты
        /// </summary>
        /// <param name="name">имя ресурса</param>
        /// <param name="type">тип (обычно Sprite)</param>
        /// <param name="path">путь к ресурсу</param>
        /// <param name="hash">хэш</param>
        public MapResource(string name, string type, string path, string hash)
        {
            this.name = name;
            this.type = type;
            this.path = path;
            this.hash = hash;
        }
    }

    /// <summary>
    /// Основной класс карты, содержит все данные
    /// </summary>
    public class MapData
    {
        /// <summary>
        /// ID должен соответствовать названию папки
        /// </summary>
        public string id;
        /// <summary>
        /// Название карты
        /// </summary>
        public string name = "No Name (new editor)";
        /// <summary>
        /// Описание
        /// </summary>
        public string info = "No Info";
        /// <summary>
        /// Количество рук
        /// </summary>
        public int handCount = 1;
        /// <summary>
        /// Ссылка, заданая пользователем
        /// </summary>
        public string moreInfoURL = "";
        /// <summary>
        /// Скорость
        /// </summary>
        public float speed = 15;
        /// <summary>
        /// Жизни
        /// </summary>
        public int lives = 10;
        /// <summary>
        /// Макс. жизней
        /// </summary>
        public int maxLives = 20;
        /// <summary>
        /// Путь до муз. файла
        /// </summary>
        public string musicFile = "music.ogg";
        /// <summary>
        /// Точная длительность музыки
        /// </summary>
        public double musicTime = 0;
        /// <summary>
        /// Путь до обложки
        /// </summary>
        public string iconFile = "icon.jpg";
        /// <summary>
        /// Скрывать, пока заблокирована
        /// </summary>
        public bool hidden = false;
        /// <summary>
        /// Ресурсы - массив из объектов
        /// </summary>
        public List<MapResource> levelResources = new List<MapResource>() { };
        /// <summary>
        /// Массив тегов
        /// </summary>
        public List<string> tags = new List<string>() { };
        /// <summary>
        /// Условие доступности карты
        /// </summary>
        public List<string> unlockConditions = new List<string>();
        /// <summary>
        /// Массив чекпоинтов (их время в секундах)
        /// </summary>
        public List<float> checkpoints = new List<float>();
        /// <summary>
        /// Массив всех ивентов на карте
        /// </summary>
        public List<MapEvent> events = new List<MapEvent>();

        public int generationType = 2;
        public int environmentType = 3;
        public int[] puzzleSequencesList = { };

        /// <summary>
        /// пустой конструктор
        /// </summary>
        public MapData()
        {

        }

        /// <summary>
        /// коструктор
        /// </summary>
        /// <param name="mapData"></param>
        public MapData(MapData mapData)
        {
            id = mapData.id;
            name = mapData.name;
            info = mapData.info;
            levelResources = mapData.levelResources;
            moreInfoURL = mapData.moreInfoURL;
            speed = mapData.speed;
            lives = mapData.lives;
            maxLives = mapData.maxLives;
            handCount = mapData.handCount;
            musicFile = mapData.musicFile;
            musicTime = mapData.musicTime;
            iconFile = mapData.iconFile;

            unlockConditions = new List<string>(mapData.unlockConditions.Count);
            mapData.unlockConditions.ForEach((item) =>
            {
                if (!string.IsNullOrEmpty(item))
                    unlockConditions.Add(item);
            });

            tags = new List<string>(mapData.tags.Count);
            mapData.tags.ForEach((item) =>
            {
                if (!string.IsNullOrEmpty(item))
                    tags.Add(item);
            });

            hidden = mapData.hidden;

            checkpoints = new List<float>(mapData.checkpoints.Count);
            mapData.checkpoints.ForEach((item) =>
            {
                checkpoints.Add(item);
            });

            events = new List<MapEvent>(mapData.events.Count);
            mapData.events.ForEach((item) =>
            {
                events.Add(new MapEvent(item));
            });
        }
    }
    
    /// <summary>
    /// не используется
    /// </summary>
    public class EditorEventFunctionInfo
    {
        public enum EditorEventParametrType { InputField, Slider, Toggle, ArcSelector }
        public class EditorEventParametr
        {
            public string name;
            public EditorEventParametrType editor;
            public string description;
            public string data = null;

            /// <summary>
            /// Метод EditorEventParametr() является
            /// конструктором
            /// </summary>
            /// <param name="name">Имя параметра</param>
            /// <param name="editor">Тип редактора</param>
            /// <param name="description">Описание параметра</param>
            /// <param name="data">Дополнительные данные параметра</param>
            public EditorEventParametr(string name, EditorEventParametrType editor, string description, string data = null)
            {
                this.name = name;
                this.editor = editor;
                this.description = description;
                this.data = data;
            }
        }

        public List<EditorEventParametr> parameters;
        public string id;
        public string functionDescription;

        /// <summary>
        /// Метод EditorEventFunctionInfo() является
        /// конструктором
        /// </summary>
        /// <param name="id">Точное id ивента</param>
        /// <param name="parameters">Параметры ивента</param>
        /// <param name="functionDescription">Описание ивента</param>
        public EditorEventFunctionInfo(string id, List<EditorEventParametr> parameters, string functionDescription)
        {
            this.id = id;
            this.parameters = parameters;
            this.functionDescription = functionDescription;
        }
    }
}